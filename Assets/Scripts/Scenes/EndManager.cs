using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField] Animator transition;


    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        StartCoroutine(Transition());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    IEnumerator Transition()
    {
        transition.CrossFadeInFixedTime("Transparent", 3);
        yield return new WaitForSeconds(3.5f);
    }
}
