using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWelcome : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public void NextWelcomeMethod()
    {
        if (text1.activeInHierarchy) 
        {
            text1.SetActive(false);
            text2.SetActive(true);
        } 
        else if (text2.activeInHierarchy)
        {
            text2.SetActive(false);
            text3.SetActive(true);
        } 
        else if (text3.activeInHierarchy)
        {
            text3.SetActive(false);
            text4.SetActive(true);
        } 
        else
        {
            text4.SetActive(false);
            text1.SetActive(true);
        }
    }

    public void PreviousWelcomeMethod()
    {
        if (text1.activeInHierarchy) 
        {
            text1.SetActive(false);
            text4.SetActive(true);
        } 
        else if (text2.activeInHierarchy)
        {
            text2.SetActive(false);
            text1.SetActive(true);
        } 
        else if (text3.activeInHierarchy)
        {
            text3.SetActive(false);
            text2.SetActive(true);
        } 
        else
        {
            text4.SetActive(false);
            text3.SetActive(true);
        }
    }

}
