using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainButtonsController : MonoBehaviour
{
    public void Pressed()
    {
        StartCoroutine(c_Pressed());
    }

    private IEnumerator c_Pressed()
    {
        transform.localScale = transform.localScale - new Vector3(0, 0.3f, 0);
        gameObject.transform.parent.gameObject.GetComponent<MainBlock>().Execute();
        yield return new WaitForSeconds(0.5f);
        transform.localScale = transform.localScale + new Vector3(0, 0.3f, 0);
    }
}
