using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerSettings", menuName = "PlayerManager/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public bool isPlayerRightHanded;
}