using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BlockSet", menuName = "BlockProgramming/BlockSet", order = 1)]
public class BlockSet : ScriptableObject
{
    public int moveForwardQuantity;
   
    public int forQuantity;
    public int endForQuantity;
    
    public int ifQuantity;
    public int endIfQuantity;
    
    public int getHumidityQuantity;

    public int turnQuantity;
    public int rightQuantity;
    public int leftQuantity;

    public int number3_Quantity;
    public int number4_Quantity;
    public int number5_Quantity;
    public int number6_Quantity;
    public int number7_Quantity;
    public int number8_Quantity;
    public int number9_Quantity;
}
