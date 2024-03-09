using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class Block
{

    public enum character
    {
        GRANTAIRE, ENJOLRAS

    };

    [TextArea] public string content;
    public character name_ch;
    
}


[Serializable]
public class ChoiceBlock {
    public string choiceName;
    public StoryBlock moveTo;
}

public enum if_end
{
    CHOICE, HEART, MINIGAME
}

public class StoryBlock : MonoBehaviour
{


    public List<Block> block;


    [Header("Do Last")]
    [HideInInspector] [SerializeField] if_end ifEnd;
    [HideInInspector] [SerializeField] List<ChoiceBlock> choice;
    [HideInInspector] [SerializeField] [Range(-20, 100)] int heartAdd=0;
    [HideInInspector] [SerializeField] GameObject minigameObj;
    

}
