using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsManager : MonoBehaviour
{
    public void deleteConstraints(Rigidbody rg)
    {
        rg.constraints = RigidbodyConstraints.None;
    }

    public void activeConstraints(Rigidbody rg)
    {
        rg.constraints = RigidbodyConstraints.FreezeAll;
    }
}
