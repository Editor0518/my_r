using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class Block
{

    public enum character
    {
        GRANTAIRE, ENJOLRAS, X, JOLY, COMBEFERRE, COURFEYRAC, JEHAN, FEUILLY, BAHOREL, LAMARQUE, MUSICHETTA

    };
    public enum font
    {
        DEFAULT, DRUNKTALK, DRUNKSERIOUS, ACTINGTONE, MAD

    };

    /*
    public enum face { 
        DEFAULT, SMILE, CLOSED, DEPRESSED, FORCE, HEAD_DOWN, CYNICAL, NOTICE, SORRY, DEPRESSED_2, DRUNK_EYE
    };*/

    public string start_cmd;
    [TextArea] public string content;
    public string thinkingContent;
    //[TextArea] public string content_ENG;
    //[TextArea] public string content;
    //[TextArea] public string content;
    //[TextArea] public string content;

    public bool isNoName = false;
    public bool isNameUnkown = false;
    [Range(0, 2)] public int focusOn = 0;//현재 포커스된 위치

    public character left_name_ch = character.ENJOLRAS;
    public string left_face_ch;
    [Space]
    public character center_name_ch = character.X;
    public string center_face_ch;

    public character right_name_ch = character.X;
    public string right_face_ch;


    [Space]
    public font text_font;
    //public face face_ch;
    public string after_cmd;
    public AudioClip se;
    public string seSubtitle;

    public Block(string content)
    {
        isNoName = false;
        focusOn = 0;//현재 포커스된 위치

        left_name_ch = character.ENJOLRAS;
        left_face_ch = "";
        center_name_ch = character.X;
        center_face_ch = "";

        right_name_ch = character.X;
        right_face_ch = "";

        text_font = font.DEFAULT;
        start_cmd = "";
        thinkingContent = "";
        after_cmd = "";
        se = null;
        this.content = content;

    }

    public Block()
    {
        isNoName = false;
        focusOn = 0;//현재 포커스된 위치

        left_name_ch = character.ENJOLRAS;
        left_face_ch = "";
        center_name_ch = character.X;
        center_face_ch = "";

        right_name_ch = character.X;
        right_face_ch = "";

        text_font = font.DEFAULT;
        start_cmd = "";
        thinkingContent = "";
        after_cmd = "";
        se = null;
        content = "";
    }
}

[Serializable]
public class ItemBlock
{
    public string itemName;
    public StoryBlock newBlock;

    public ItemBlock(string itemName, StoryBlock newBlock)
    {
        this.itemName = itemName;
        this.newBlock = newBlock;
    }
}

[Serializable]
public class ChoiceBlock
{
    public string choiceName;
    public StoryBlock moveTo;
    public string choiceCmdOnWhen;
    public string choiceCmdAfter;
    public AudioClip clip;

}

public enum if_end
{
    CHOICE, NEW, MINIGAME, GIFT, MOVECMD, XXX, END
}

public class StoryBlock : MonoBehaviour
{
    public float startDelaySecond = 0.0f;

    public Sprite background;
    public Volume volumeObj;
    public AudioClip bgm;
    public AudioClip ambience;
    public string bgmSubtitle;

    public List<Block> block = new List<Block>(1);


    [Header("Do Last")]
    [HideInInspector] public if_end ifEnd;

    [HideInInspector] public ChoiceBlock choiceA;

    [HideInInspector] public ChoiceBlock choiceB;

    [HideInInspector] public ChoiceBlock choiceC;

    [HideInInspector] public ChoiceBlock choiceD;

    [HideInInspector][Range(-20, 150)] public int heartAdd = 0;

    [HideInInspector] public StoryBlock newBlock;

    [HideInInspector] public GameObject minigameObj;

    [HideInInspector] public List<ItemBlock> itemBlock;
    //[HideInInspector] public ItemBlock itemBlock;

    public GameObject disableObj;

    [HideInInspector] public List<ChoiceBlock> newBlockList;



}
