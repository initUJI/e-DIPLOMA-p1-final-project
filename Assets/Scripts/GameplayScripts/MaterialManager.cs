using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] Material[] materials;
    private Renderer objectRenderer;
    bool glowing = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        glowing = false;
        objectRenderer.material = materials[0];
    }

    
    public void ToogleGlow()
    {
        if(!glowing)
        {
            objectRenderer.material = materials[1];
        } else
        {
            objectRenderer.material = materials[0];
        }
        glowing = !glowing;
    }
}
