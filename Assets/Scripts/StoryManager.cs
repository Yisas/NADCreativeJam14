using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] Image image;
    [SerializeField] Sprite[] panels;

    int panelIndex = 0;
    bool loading = false;


    void Start()
    {
        transition.GetComponent<Image>().enabled = true;
        transition.CrossFadeInFixedTime("Transparent", 1.5f);
        image.sprite = panels[panelIndex];
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (panelIndex == panels.Length - 1 && !loading)
                StartCoroutine(Transition());
            else
                image.sprite = panels[++panelIndex];
        }
    }

    IEnumerator Transition()
    {
        loading = true;
        transition.CrossFadeInFixedTime("Opaque", 2);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(2);
    }
}
