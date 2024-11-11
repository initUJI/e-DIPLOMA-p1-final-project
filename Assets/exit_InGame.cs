using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit_InGame : MonoBehaviour
{
    public GameObject exitPanel;

    private void Start()
    {
        exitPanel.SetActive(false);
    }

    public void exitButtonPressed()
    {
        exitPanel.SetActive(true);
        LeanTween.scale(exitPanel, Vector3.one, 1);
    }


    public void Yes_onExitPanel()
    {
        Application.Quit();

    }

    public void No_onExitPanel()
    {
        exitPanel.SetActive(false);
        LeanTween.scale(exitPanel, Vector3.zero, 1);
    }
}
