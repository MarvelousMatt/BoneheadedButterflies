using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool showTutorialUI = true;

    public List<GameObject> tutUI;

    void Start()
    {
        ShowHideTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            showTutorialUI = !showTutorialUI;
        }

        if (Input.GetKeyDown(KeyCode.U) || Input.GetMouseButtonDown(0))
        {
            ShowHideTutorial();
        }
    }

    void ShowHideTutorial()
    {
        for (int i = 0; i < tutUI.Count; i++)
        {
            tutUI[i].SetActive(showTutorialUI);
        }
    }
}
