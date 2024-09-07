using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Block", menuName = "Scripteable/Block")]

public class Bloques : ScriptableObject
{
    public BlockType typeBlock;
    public string nameBlock;
    public int tamaño_letra;
}
