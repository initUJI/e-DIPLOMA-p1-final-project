using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBlacscreenCanvas : MonoBehaviour
{
    public GameObject faderSphere_Gameobject;
    public TextMeshProUGUI faderSphere_Text;


    public void faderSphereNewState(int newState)
    {
        LeanTween.alphaCanvas(faderSphere_Text.GetComponent<CanvasGroup>(), newState, 2);
        LeanTween.alpha(faderSphere_Gameobject, newState, 3);
    }

    public void setNewText(string newText)
    {
        faderSphere_Text.text = newText;
    }
}
