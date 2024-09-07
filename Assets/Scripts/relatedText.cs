using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class relatedText : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    private GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TextMeshProUGUI.text == "" || m_TextMeshProUGUI.text == " ")
        {
            child.SetActive(false);
        }
        else
        {
            child.SetActive(true);
        }
    }
}
