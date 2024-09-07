using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWelcome : MonoBehaviour
{
    public void DestroyWelcomeMethod()
    {
        Destroy(transform.gameObject);
    }
}
