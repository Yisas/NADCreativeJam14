using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnvironmentElement
{
    public string type = null;
    public GameObject[] variants = null;
    public float spawnChance = 1;
    public int minAmount = 1;
    public int maxAmount = 1;
    public float minSize = 1;
    public float maxSize = 1;
    public float minHeight = 0;
    public float maxHeight = 0;
}

public class ProceduralGeneration : MonoBehaviour
{

    [Tooltip("Radius within which the environment is generated")]
    [SerializeField] private float generationRadius = 50;

    [Tooltip("Base object that is placed as chunks are generated")]
    [SerializeField] private GameObject chunk = null;

    [SerializeField] private EnvironmentElement[] environment = null;

    private float chunkSize = 0; // size of each chunk
    private Dictionary<Vector3, GameObject> generatedChunks = new Dictionary<Vector3, GameObject>(); // neighboring ungenerated chunk positions
    private HashSet<Vector3> ungeneratedChunks = new HashSet<Vector3>(); // neighboring ungenerated chunk positions
    private Transform player; // transform of the player

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

	private void Start()
    {
        GenerateChunk(Vector3.zero);
    }
	
	private void Update()
    {
           GenerateNewChunks();
    }

    // Generate the environmental elements in the chunk given chunk.
    private void GenerateEnvironmentOnChunk(GameObject chunk)
    {
        if (chunk.transform.position == Vector3.zero)
            return;
        var minX = chunk.transform.position.x - chunkSize / 2;
        var maxX = chunk.transform.position.x + chunkSize / 2;
        var minZ = chunk.transform.position.z - chunkSize / 2;
        var maxZ = chunk.transform.position.z + chunkSize / 2;
        foreach (EnvironmentElement element in environment)
        {
            if (Random.value > element.spawnChance)
                continue;
            var amount = Random.Range(element.minAmount, element.maxAmount + 1);
            for (int i = 0; i < amount; ++i)
            {
                var variant = element.variants[Random.Range(0, element.variants.Length)];
                var instance = Instantiate(variant, Vector3.zero, Quaternion.identity);
                instance.transform.SetParent(chunk.transform);
                instance.transform.localScale = Vector3.one * Random.Range(element.minSize, element.maxSize);
                instance.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                var extents = instance.GetComponent<Collider>().bounds.extents;
                var position = Vector3.zero;
                do
                {
                    var x = Random.Range(minX, maxX);
                    var z = Random.Range(minZ, maxZ);
                    var height = Random.Range(element.minHeight, element.maxHeight);
                    position = new Vector3(x, height, z);
                }
                while (Physics.CheckBox(position, extents, Quaternion.identity, LayerMask.GetMask("Environment")));
                instance.transform.position = position;
            }
        }
    }

    // Generate a new chunk at the given position.
    // This assumes that the chunk does not already exists.
    private void GenerateChunk(Vector3 position)
    {
        var instance = Instantiate(chunk, position, Quaternion.identity);
        instance.transform.SetParent(transform);
        if (chunkSize == 0)
        {
            chunkSize = instance.GetComponentInChildren<Collider>().bounds.size.x;
        }
        GenerateEnvironmentOnChunk(instance);
        generatedChunks.Add(position, instance);
        var neighborOffsets = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, 1),
            new Vector3(-1, 0, 0),
            new Vector3(-1, 0, -1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),
        };
        foreach (Vector3 offset in neighborOffsets)
        {
            var chunkPosition = position + offset * chunkSize;
            if (!generatedChunks.ContainsKey(chunkPosition))
            {
                ungeneratedChunks.Add(chunkPosition);
            }
        }
    }

    // Generate new environment chunks if necessary.
    private void GenerateNewChunks()
    {
        if (player == null)
            return;
        var generationNeeded = false;
        foreach (Vector3 chunkPosition in new HashSet<Vector3>(ungeneratedChunks))
        {
            if (Vector3.Distance(chunkPosition, player.position) <= generationRadius)
            {
                GenerateChunk(chunkPosition);
                ungeneratedChunks.Remove(chunkPosition);
                generationNeeded = true;
            }
        }
        if (generationNeeded)
        {
            GenerateNewChunks();
        }
    }
}
