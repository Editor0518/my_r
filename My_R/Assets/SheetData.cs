using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public enum If_end
{
    CHOICE, NEW, END, MOVECMD, MINIGAME
}

[System.Serializable]
public class StoryBlock
{
    public int branch;

    public If_end ifEnd = If_end.NEW;
    public List<Block> block;

    public StoryBlock(int branch)
    {
        this.branch = branch;
        this.block = new List<Block>();
    }

    public void AddBlock(Block block)
    {

        if (block.start_cmd.Contains("CHOICE"))
        {
            //  if (!block.content.Contains("<skip>")) block.content += "<skip>";
            ifEnd = If_end.CHOICE;
        }
        else if (block.move.Equals("END"))
        {
            ifEnd = If_end.END;
        }
        else if (block.start_cmd.Contains("MOVECMD"))
        {
            ifEnd = If_end.MOVECMD;
        }
        else if (block.move.Contains("MINIGAME"))
        {
            ifEnd = If_end.MINIGAME;
        }

        this.block.Add(block);
    }

}

[System.Serializable]
public class Block
{
    public string start_cmd;
    public string name;
    [Range(-1, 2)] public int focus;
    public string[] standing = new string[3];
    [TextArea] public string content;
    [TextArea] public string thinking;
    public string font;
    public string after_cmd;
    public string move;

    public Block(string start_cmd, string name, int focus, string left, string center, string right, string content, string thinking, string font, string after_cmd, string move)
    {
        this.start_cmd = start_cmd;
        this.name = name;
        this.focus = focus;
        this.standing = new string[] { left, center, right };
        this.content = content;
        this.thinking = thinking;
        this.font = font;
        this.after_cmd = after_cmd;
        this.move = move;
    }
}

public class SheetData : MonoBehaviour
{
    public static SheetData instance;

    [Header("시나리오")]
    public List<StoryBlock> storyBlock;

    private void Awake()
    {
        instance = this;
    }

    public void AddBlock(int branch, Block block)
    {
        storyBlock[FindBranchIndex(branch)].AddBlock(block);

    }

    public int FindBranchIndex(int branch)
    {
        for (int i = 0; i < storyBlock.Count; i++)
        {
            if (storyBlock[i].branch.Equals(branch))
            {
                return i;
            }
        }

        Debug.Log($"{branch} 번 branch가 존재하지 않습니다!");
        return -1;
    }

    public void AddStoryBlock(StoryBlock storyBlock)
    {
        this.storyBlock.Add(storyBlock);
    }

}

