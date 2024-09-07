using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainButtonsController : MonoBehaviour
{
    TMP_Text textMeshPro;
    

    private void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
    }

    public void Pressed()
    {
        StartCoroutine(c_Pressed());

        if(textMeshPro.text.Equals("Pause"))
        {
            textMeshPro.text = "Unpause";
        } else if(textMeshPro.text.Equals("Unpause"))
        {
            textMeshPro.text = "Pause";
        }
    }

    private IEnumerator c_Pressed()
    {
        /*
        Debug.Log("Button : Wait 2 seconds");
        yield return new WaitForSeconds(2);
        Debug.Log("Go !");
        */

        transform.localScale = transform.localScale - new Vector3(0, 0.3f, 0);
        gameObject.transform.parent.gameObject.GetComponent<MainBlock>().Execute();
        yield return new WaitForSeconds(0.5f);
        transform.localScale = transform.localScale + new Vector3(0, 0.3f, 0);
    }
}
