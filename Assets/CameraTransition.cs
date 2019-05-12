using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] Animator transition;
    void Start()
    {
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        transition.CrossFadeInFixedTime("Transparent", 1);
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().CrossFadeInFixedTime("CameraNear", 3);
        transition.CrossFadeInFixedTime("Opaque", 3);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(3);
    }
}
