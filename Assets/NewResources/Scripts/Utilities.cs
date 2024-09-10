using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static List<string> BlockListToStringList(List<BlockObject> blockList)
    {
        List<string> blockNames = new List<string>();

        foreach (BlockObject block in blockList)
        {
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
        foreach (string blockName in blockList)
        {
            bool found = false;

            foreach (BlockObject block in allBlockObjects)
            {
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