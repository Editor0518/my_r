using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameTxt;
    public TMP_Text contentTxt;

    public StoryBlock currentBlock;
    public int index = 0;

    private void Start()
    {
        if (currentBlock != null)
        {
            ChangeDialogue();
        }
    }

    string CharacterName()
    {
        int num = ((int)currentBlock.block[index].name_ch);
        string name_ch="???";
        switch (num)
        {
            case 0:
                {
                    name_ch = "±×¶ûÅ×¸£";
                }
                break;
            case 1:
                {
                    name_ch = "¾ÓÁ¹¶ó½º";
                }
                break;

        }
        return name_ch;
    }

    void ChangeDialogue()
    {
        if (index >= currentBlock.block.Count) {
            Debug.Log("²ËÂü");
            return;
        }

        contentTxt.text = currentBlock.block[index].content;
        nameTxt.text = CharacterName();
        index++;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ChangeDialogue();
        }


    }
}
