using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new typeblock", menuName = "Scripteable/BlockType")]

public class BlockType : ScriptableObject
{
    public string Type;
    public Material material;
    public Mesh forma;
    public string nodeRightName;
    public string nodeNextName;

}
