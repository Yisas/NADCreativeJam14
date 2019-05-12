using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    [SerializeField] Animator transition;
    public Text timerText;
    public float time;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        transition.CrossFadeInFixedTime("Transparent", 3);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        time = Mathf.Clamp(time, 0, 5000);

        float minutes = Mathf.Floor(time / 60); 
        float seconds = time % 60;
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
