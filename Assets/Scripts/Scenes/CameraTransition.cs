﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] Animator transition;
    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        transition.CrossFadeInFixedTime("Transparent", 3);
        yield return new WaitForSeconds(4);
        transition.CrossFadeInFixedTime("Opaque", 3);
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(3);
    }
}
