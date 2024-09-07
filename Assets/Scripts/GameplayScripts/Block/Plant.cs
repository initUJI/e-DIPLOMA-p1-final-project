using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [HideInInspector] public GameObject character;
    [HideInInspector] public bool humidityChecked = false;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.CompareTag("Character"))
        {
            character = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       /* Debug.Log(other.tag);
        if (other.CompareTag("Character"))
        {
            character = null;
        }*/
    }

    public bool characterInPlant()
    {
        //Debug.Log(character);
        return character != null;
    }
}
