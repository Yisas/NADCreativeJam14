using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FadingBird : MonoBehaviour
{
    public float chirpTimeMin;
    public float chirpTimeMax;
    public AudioClip[] chirpSounds;

    float timer;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = timer = Random.Range(chirpTimeMin, chirpTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = Random.Range(chirpTimeMin, chirpTimeMax);
            audioSource.PlayOneShot(chirpSounds[Random.Range(0, chirpSounds.Length - 1)]);
        }
    }
}
