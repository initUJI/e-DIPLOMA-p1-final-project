using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowController : MonoBehaviour
{
    Material material;  
    bool glowing;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        glowing = false;
    }

    private void ToogleGlow()
    {
        if(!glowing)
        {
            material.EnableKeyword("_EMISSION");
        } else
        {
            material.DisableKeyword("_EMISSION");
        }
    }
}
