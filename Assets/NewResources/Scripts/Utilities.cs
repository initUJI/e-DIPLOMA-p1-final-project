using System.Collections.Generic;
using System.Linq;
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


    public static List<BlockObject> StringListToBlockList(List<string> blockList)
    {
        List<BlockObject> blocks = new List<BlockObject>();

        // Cargar todos los BlockObject desde la carpeta "Resources/Blocks"
        BlockObject[] allBlockObjects = Resources.LoadAll<BlockObject>("Blocks");

        // Buscar los BlockObject que coincidan con los nombres dados en blockNames
        for (int i = 0; i < blockList.Count; i++)
        {
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
    }

}