using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    [SerializeField] Animator transition;
    public Text timerText;
    public float time;

    public AudioClip secondMainTheme;

    bool loading = false;

    private float timer;
    AudioSource audioSource;
    bool switchedTheme = false;

    // Start is called before the first frame update
    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(HideTransition());
    }

    IEnumerator HideTransition()
    {
        yield return new WaitForSeconds(1);
        transition.CrossFadeInFixedTime("Transparent", 3);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        time = Mathf.Clamp(time, 0, 5000);

        float minutes = Mathf.Floor(time / 60); 
        float seconds = time % 60;
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (time == 0 && !loading)
        {
            StartCoroutine(Transition());
            return;
        }

        if (!audioSource.isPlaying  && !switchedTheme)
        {
            switchedTheme = true;
            audioSource.clip = secondMainTheme;
            audioSource.Play();
            audioSource.loop = true;
        }
    }

    IEnumerator Transition()
    {
        loading = true;
        transition.CrossFadeInFixedTime("Opaque", 3);
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(4);
    }
}
