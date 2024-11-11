using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightHandedToggle_Controller : MonoBehaviour
{
    public PlayerSettings playerSettings;


    public void Start()
    {
        gameObject.GetComponent<Toggle>().isOn = playerSettings.isPlayerRightHanded;
    }

    public void onRightHandedToggleChanged()
    {
        bool newState = gameObject.GetComponent<Toggle>().isOn;
        playerSettings.isPlayerRightHanded = newState;
    }

}
