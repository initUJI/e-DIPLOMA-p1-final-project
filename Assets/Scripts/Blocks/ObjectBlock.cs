using TMPro;
using UnityEngine;

public class ObjectBlock : Block, WithAssociatedString
{

    [SerializeField] string associatedString;
    public string getAssociatedString()
    {
        return associatedString;
    }

    public void setAssociatedString(string s)
    {
        associatedString = s;

        TMP_Text textMeshPro = GetComponentInChildren<TMP_Text>();
        if (textMeshPro != null)
        {
            textMeshPro.text = associatedString;
        }
    }

    public override void Start()
    {
        base.Start();
        TMP_Text textMeshPro = GetComponentInChildren<TMP_Text>();
        if (textMeshPro != null)
        {
            textMeshPro.text = associatedString;
        }

    }
}
