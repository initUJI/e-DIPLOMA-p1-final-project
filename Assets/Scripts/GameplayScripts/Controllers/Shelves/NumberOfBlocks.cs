using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfBlocks : MonoBehaviour
{
    private int numberOfFor = 0;
    private int numberOfForward = 0;
    private int numberOfGetHumidity = 0;
    private int numberOfIf = 0;
    private int numberOfLeft = 0;
    private int numberOfNumber = 0;
    private int numberOfObject = 0;
    private int numberOfRight = 0;
    private int numberOfTurn = 0;
    private int numberOfEndFor = 0;
    private int numberOfEndIf = 0;

    private int numberOfNumber2 = 0;
    private int numberOfNumber3 = 0;
    private int numberOfNumber4 = 0;
    private int numberOfNumber5 = 0;
    private int numberOfNumber6 = 0;
    private int numberOfNumber7 = 0;
    private int numberOfNumber8 = 0;
    private int numberOfNumber9 = 0;
    public int returnNumberOfBlocks(GameObject block, int n = 0)
    {
        switch (block.name)
        {
            case "TurnBlock": return numberOfTurn;
            case "TurnBlock(Clone)": return numberOfTurn;
            case "TurnRightBlock": return numberOfRight;
            case "TurnRightBlock(Clone)": return numberOfRight;
            case "ObjectBlock": return numberOfObject;
            case "ObjectBlock(Clone)": return numberOfObject;
            case "NumberBlock":
                switch (n)
                {
                    case 9: return numberOfNumber9;
                    case 8: return numberOfNumber8;
                    case 7: return numberOfNumber7;
                    case 6: return numberOfNumber6;
                    case 5: return numberOfNumber5;
                    case 4: return numberOfNumber4;
                    case 3: return numberOfNumber3;
                    case 2: return numberOfNumber2;
                    default: return numberOfNumber;
                }
            case "NumberBlock(Clone)":
                switch (n)
                {
                    case 9: return numberOfNumber9;
                    case 8: return numberOfNumber8;
                    case 7: return numberOfNumber7;
                    case 6: return numberOfNumber6;
                    case 5: return numberOfNumber5;
                    case 4: return numberOfNumber4;
                    case 3: return numberOfNumber3;
                    case 2: return numberOfNumber2;
                    default: return numberOfNumber;
                }

            case "TurnLeftBlock": return numberOfLeft;
            case "TurnLeftBlock(Clone)": return numberOfLeft;
            case "IfBlock": return numberOfIf;
            case "IfBlock(Clone)": return numberOfIf;
            case "GetHumidityBlock": return numberOfGetHumidity;
            case "GetHumidityBlock(Clone)": return numberOfGetHumidity;
            case "ForwardBlock": return numberOfForward;
            case "ForwardBlock(Clone)": return numberOfForward;
            case "ForBlock": return numberOfFor;
            case "ForBlock(Clone)": return numberOfFor;
            case "EndIfBlock": return numberOfEndIf;
            case "EndIfBlock(Clone)": return numberOfEndIf;
            case "EndForBlock": return numberOfEndFor;
            case "EndForBlock(Clone)": return numberOfEndFor;
            default: return 0;
        }
    }

    public void substractNumberOfBlocks(GameObject block, int n = 0)
    {
        switch (block.name)
        {
            case "TurnBlock": numberOfTurn--; break;
            case "TurnBlock(Clone)": numberOfTurn--; break;
            case "TurnRightBlock": numberOfRight--; break;
            case "TurnRightBlock(Clone)": numberOfRight--; break;
            case "ObjectBlock": numberOfObject--; break;
            case "ObjectBlock(Clone)": numberOfObject--; break;
            case "NumberBlock":
                switch (n)
                {
                    case 9: numberOfNumber9--; break;
                    case 8: numberOfNumber8--; break;
                    case 7: numberOfNumber7--; break;
                    case 6: numberOfNumber6--; break;
                    case 5: numberOfNumber5--; break;
                    case 4: numberOfNumber4--; break;
                    case 3: numberOfNumber3--; break;
                    case 2: numberOfNumber2--; break;
                    default: numberOfNumber--; break;
                }
                break;
            case "NumberBlock(Clone)":
                switch (n)
                {
                    case 9: numberOfNumber9--; break;
                    case 8: numberOfNumber8--; break;
                    case 7: numberOfNumber7--; break;
                    case 6: numberOfNumber6--; break;
                    case 5: numberOfNumber5--; break;
                    case 4: numberOfNumber4--; break;
                    case 3: numberOfNumber3--;Debug.Log("aqui"); break;
                    case 2: numberOfNumber2--; break;
                    default: numberOfNumber--; break;
                }
                break;
            case "TurnLeftBlock": numberOfLeft--; break;
            case "TurnLeftBlock(Clone)": numberOfLeft--; break;
            case "IfBlock": numberOfIf--; break;
            case "IfBlock(Clone)": numberOfIf--; break;
            case "GetHumidityBlock": numberOfGetHumidity--; break;
            case "GetHumidityBlock(Clone)": numberOfGetHumidity--; break;
            case "ForwardBlock": numberOfForward--; break;
            case "ForwardBlock(Clone)": numberOfForward--; break;
            case "ForBlock": numberOfFor--; break;
            case "ForBlock(Clone)": numberOfFor--; break;
            case "EndIfBlock": numberOfEndIf--; break;
            case "EndIfBlock(Clone)": numberOfEndIf--; break;
            case "EndForBlock": numberOfEndFor--; break;
            case "EndForBlock(Clone)": numberOfEndFor--; break;
            default: break;
        }
    }

    public void sumNumberOfBlocks(GameObject block, int n = 0)
    {
        switch (block.name)
        {
            case "TurnBlock": numberOfTurn++; break;
            case "TurnBlock(Clone)": numberOfTurn++; break;
            case "TurnRightBlock": numberOfRight++; break;
            case "TurnRightBlock(Clone)": numberOfRight++; break;
            case "ObjectBlock": numberOfObject++; break;
            case "ObjectBlock(Clone)": numberOfObject++; break;
            case "NumberBlock":
                switch (n)
                {
                    case 9: numberOfNumber9++; break;
                    case 8: numberOfNumber8++; break;
                    case 7: numberOfNumber7++; break;
                    case 6: numberOfNumber6++; break;
                    case 5: numberOfNumber5++; break;
                    case 4: numberOfNumber4++; break;
                    case 3: numberOfNumber3++; break;
                    case 2: numberOfNumber2++; break;
                    default: numberOfNumber++; break;
                }
                break;
            case "NumberBlock(Clone)":
                switch (n)
                {
                    case 9: numberOfNumber9++; break;
                    case 8: numberOfNumber8++; break;
                    case 7: numberOfNumber7++; break;
                    case 6: numberOfNumber6++; break;
                    case 5: numberOfNumber5++; break;
                    case 4: numberOfNumber4++; break;
                    case 3: numberOfNumber3++; break;
                    case 2: numberOfNumber2++; break;
                    default: numberOfNumber++; break;
                }
                break;
            case "TurnLeftBlock": numberOfLeft++; break;
            case "TurnLeftBlock(Clone)": numberOfLeft++; break;
            case "IfBlock": numberOfIf++; break;
            case "IfBlock(Clone)": numberOfIf++; break;
            case "GetHumidityBlock": numberOfGetHumidity++; break;
            case "GetHumidityBlock(Clone)": numberOfGetHumidity++; break;
            case "ForwardBlock": numberOfForward++; break;
            case "ForwardBlock(Clone)": numberOfForward++; break;
            case "ForBlock": numberOfFor++; break;
            case "ForBlock(Clone)": numberOfFor++; break;
            case "EndIfBlock": numberOfEndIf++; break;
            case "EndIfBlock(Clone)": numberOfEndIf++; break;
            case "EndForBlock": numberOfEndFor++; break;
            case "EndForBlock(Clone)": numberOfEndFor++; break;
            default: break;
        }
    }

    public bool blocksNo()
    {
        numberOfFor = 0;
        numberOfForward = 0;
        numberOfGetHumidity = 0;
        numberOfIf = 0;
        numberOfLeft = 0;
        numberOfNumber = 0;
        numberOfObject = 0;
        numberOfRight = 0;
        numberOfTurn = 0;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;

        return true;
    }

    public bool blocksForTest()
    {
        numberOfFor = 10;
        numberOfForward = 10;
        numberOfGetHumidity = 10;
        numberOfIf = 10;
        numberOfLeft = 10;
        numberOfNumber = 10;
        numberOfObject = 10;
        numberOfRight = 10;
        numberOfTurn = 10;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;

        return true;
    }

    public void blocksForLevel1()
    {
        numberOfFor = 0;
        numberOfForward = 7;
        numberOfGetHumidity = 2;
        numberOfIf = 0;
        numberOfLeft = 2;
        numberOfNumber = numberOfFor;
        numberOfObject = 0;
        numberOfRight = 2;
        numberOfTurn = 2;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;
    }

    public void blocksForLevel2()
    {
        numberOfFor = 0;
        numberOfForward = 7;
        numberOfGetHumidity = 2;
        numberOfIf = 0;
        numberOfLeft = 2;
        numberOfNumber = numberOfFor;
        numberOfObject = 0;
        numberOfRight = 2;
        numberOfTurn = 2;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;
    }

    public void blocksForLevel3()
    {
        numberOfFor = 1;
        numberOfForward = 1;
        numberOfGetHumidity = 2;
        numberOfIf = 0;
        numberOfLeft = 0;
        numberOfNumber = numberOfFor;
        numberOfObject = 0;
        numberOfRight = 0;
        numberOfTurn = 0;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;
    }

    public void blocksForLevel4()
    {
        numberOfFor = 1;
        numberOfForward = 1;
        numberOfGetHumidity = 2;
        numberOfIf = 1;
        numberOfLeft = 2;
        numberOfNumber = numberOfFor;
        numberOfObject = 1;
        numberOfRight = 2;
        numberOfTurn = 2;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;
    }

    public void blocksForLevel5()
    {
        numberOfFor = 1;
        numberOfForward = 3;
        numberOfGetHumidity = 2;
        numberOfIf = 1;
        numberOfLeft = 2;
        numberOfNumber = numberOfFor;
        numberOfObject = 1;
        numberOfRight = 2;
        numberOfTurn = 2;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;
    }

    public void blocksForLevel6()
    {
        numberOfFor = 1;
        numberOfForward = 2;
        numberOfGetHumidity = 2;
        numberOfIf = 1;
        numberOfLeft = 2;
        numberOfNumber = numberOfFor;
        numberOfObject = 1;
        numberOfRight = 2;
        numberOfTurn = 2;
        numberOfEndFor = numberOfFor;
        numberOfEndIf = numberOfIf;
        numberOfNumber2 = numberOfNumber;
        numberOfNumber3 = numberOfNumber;
        numberOfNumber4 = numberOfNumber;
        numberOfNumber5 = numberOfNumber;
        numberOfNumber6 = numberOfNumber;
        numberOfNumber7 = numberOfNumber;
        numberOfNumber8 = numberOfNumber;
        numberOfNumber9 = numberOfNumber;
    }
}
