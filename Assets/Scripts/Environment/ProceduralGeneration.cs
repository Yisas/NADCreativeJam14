using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour {

    [Tooltip("Base object that is placed as chunks are generated")]
    [SerializeField] private GameObject chunk;

    [Tooltip("Radius within which the environment is generated")]
    [SerializeField] private float generationRadius;

    private float chunkSize = 0; // size of each chunk
    private HashSet<Vector3> generatedChunks = new HashSet<Vector3>(); // neighboring ungenerated chunk positions
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

    // Generate a new chunk at the given position.
    // This assumes that the chunk does not already exists.
    private void GenerateChunk(Vector3 position)
    {
        var instance = Instantiate(chunk, position, Quaternion.identity);
        instance.transform.SetParent(transform);
        if (chunkSize == 0)
        {
            chunkSize = instance.GetComponent<Collider>().bounds.size.x;
        }
        generatedChunks.Add(position);
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
            Debug.Log(chunkPosition);
            if (!generatedChunks.Contains(chunkPosition))
            {
                ungeneratedChunks.Add(chunkPosition);
            }
        }
    }

    // Generate new environment chunks if necessary.
    void GenerateNewChunks()
    {
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
