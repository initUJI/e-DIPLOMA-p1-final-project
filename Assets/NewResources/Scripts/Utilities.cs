using System.Collections.Generic;

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

}