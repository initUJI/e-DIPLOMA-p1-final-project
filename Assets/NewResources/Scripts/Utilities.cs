using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Utilities
{
    public static List<string> BlockListToStringList(List<BlockObject> blockList)
    {
        List<string> blockNames = new List<string>();

        for (int i = 0; i < blockList.Count; i++)
        {
            BlockObject block = blockList[i];
            blockNames.Add(block.blockName);
        }

        return blockNames;
    }

    public static List<BlockObject> StringListToBlockList(List<string> blockList, List<BlockObject> options)
    {
        List<BlockObject> blocks = new List<BlockObject>();

        for (int i = 0; i < blockList.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < options.Count; j++)
            {
                BlockObject block = options[j];
                if (block.blockName == blockList[i])
                {
                    blocks.Add(block);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.LogError("BlockObject with blockName " + blockList[i] + " not found.");
            }
        }

        return blocks;
    }

    public static List<BlockObject> StringListToBlockList(List<string> blockList)
    {
        List<BlockObject> blocks = new List<BlockObject>();

        Debug.Log("BlockList: " + blockList.Count);

        for (int i = 0; i < blockList.Count; i++)
        {
            Debug.Log("BlockName: " + blockList[i]);
            BlockObject block = Resources.Load<BlockObject>("MoveForward");
            Debug.Log("Block: " + block);
            blocks.Add(block);
        }

        return blocks;
    }

        /* // Cargar todos los BlockObject desde la carpeta "Resources/Blocks"
        BlockObject[] allBlockObjects = Resources.LoadAll<BlockObject>("Blocks");

        for (int i = 0; i < allBlockObjects.Length; i++)
        {
            Debug.Log("BlockName: " + allBlockObjects[i].blockName);
        }

        // Buscar los BlockObject que coincidan con los nombres dados en blockNames
        for (int i = 0; i < blockList.Count; i++)
        {
            Debug.Log("BlockName: " + blockList[i]);
            string blockName = blockList[i];
            bool found = false;

            // Iterar sobre el array de BlockObjects
            for (int j = 0; j < allBlockObjects.Length; j++)
            {
                BlockObject block = allBlockObjects[j];
                if (block.blockName == blockName)
                {
                    blocks.Add(block);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.LogError("BlockObject with blockName " + blockName + " not found.");
            }
        }

        return blocks;
    } */

}