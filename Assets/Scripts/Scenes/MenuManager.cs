﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Animator transition;

    bool loading = false;

    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        transition.CrossFadeInFixedTime("Transparent", 2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.anyKeyDown && !loading)
        {
            StartCoroutine(Transition());
        }
    }
    
    IEnumerator Transition()
    {
        loading = true;
        transition.CrossFadeInFixedTime("Opaque", 2);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }
}
