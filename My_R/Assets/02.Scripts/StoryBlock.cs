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
        GRANTAIRE, ENJOLRAS, X, COMBEFERRE, COURFEYRAC, JOLY, BAHOREL, BOSSUET, EPONINE, ROUSSEAU

    };

    public enum face { 
        DEFAULT, SMILE, CLOSED, DEPRESSED, FORCE, HEAD_DOWN, CYNICAL, NOTICE, SORRY, DEPRESSED_2, DRUNK_EYE
    };

    public string start_cmd;
    [TextArea] public string content;
    public bool isOn = true;
    public character name_ch;
    public face face_ch;
    public string after_cmd;
}


[Serializable]
public class ChoiceBlock {
    public string choiceName;
    public StoryBlock moveTo;
    public AudioClip clip;
}

public enum if_end
{
    CHOICE, HEART, NEW, MINIGAME
}

public class StoryBlock : MonoBehaviour
{
    public float startDelaySecond = 0.0f;

    public Sprite background;

    public List<Block> block;


    [Header("Do Last")]
    [HideInInspector] public if_end ifEnd;

    [HideInInspector] public ChoiceBlock choiceA;

    [HideInInspector] public ChoiceBlock choiceB;

    [HideInInspector] public ChoiceBlock choiceC;

    [HideInInspector] [Range(-20, 150)] public  int heartAdd=0;

    [HideInInspector] public StoryBlock newBlock;

    [HideInInspector] public GameObject minigameObj;

    public GameObject disableObj;

}
