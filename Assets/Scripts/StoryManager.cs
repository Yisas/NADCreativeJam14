using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite[] panels;

    int panelIndex = 0;

    void Start()
    {
        image.sprite = panels[panelIndex];
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (panelIndex == panels.Length - 1)
                SceneManager.LoadScene(2);
            else
                image.sprite = panels[++panelIndex];
        }
    }
}
