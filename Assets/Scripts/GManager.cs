using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    [SerializeField] Animator transition;
    public Text timerText;
    public float time;

    public AudioClip secondMainTheme;

    private float timer;
    AudioSource audioSource;
    bool switchedTheme = false;

    // Start is called before the first frame update
    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        transition.CrossFadeInFixedTime("Transparent", 3);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        time = Mathf.Clamp(time, 0, 5000);

        float minutes = Mathf.Floor(time / 60); 
        float seconds = time % 60;
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        if (!audioSource.isPlaying  && !switchedTheme)
        {
            switchedTheme = true;
            audioSource.clip = secondMainTheme;
            audioSource.Play();
            audioSource.loop = true;
        }
    }
}
