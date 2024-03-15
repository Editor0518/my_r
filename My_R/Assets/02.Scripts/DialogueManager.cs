using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    public Animator dialogWhole;
    public TMP_Text nameTxt;
    public TMP_Text contentTxt;

    [Header("Choice")]
    public GameObject choiceWhole;
    public ChoiceSelect choiceA;
    public ChoiceSelect choiceB;
    public ChoiceSelect choiceC;

    [Header("Storyline")]
    public StoryBlock currentBlock;
    public int index = 0;
    public bool isNoNext = false;

    [Header("Others")]
    public Heart heart;
    public TypeWriter typeWriter;

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
        string name_ch = "???";
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
            case 2:
                {
                    name_ch = "";//x
                }
                break;
            case 3:
                {
                    name_ch = "ÄáºêÆä¸£";
                }
                break;
            case 4:
                {
                    name_ch = "Äí¸£Æä¶ô";
                }
                break;
            case 5:
                {
                    name_ch = "Á¹¸®";
                }
                break;
            case 6:
                {
                    name_ch = "¹Ù¿À·¼";
                }
                break;
            case 7:
                {
                    name_ch = "º¸½¬¿¡";
                }
                break;
            case 8:
                {
                    name_ch = "¿¡Æ÷´Ñ";
                }
                break;
            case 9:
                {
                    name_ch = "·ç¼Ò";
                }
                break;
        }
        return name_ch;
    }

    void ChangeDialogue()
    {
        if (currentBlock == null) return;

        if (currentBlock.startDelaySecond > 0 && index.Equals(0))
        {
            dialogWhole.gameObject.SetActive(false);
            StartCoroutine(Delay(currentBlock.startDelaySecond));
            return;
        }
        else
        {
            if (!dialogWhole.gameObject.activeInHierarchy) dialogWhole.gameObject.SetActive(true);
        }
        typeWriter.StartTyping(currentBlock.block[index].content, (int)currentBlock.block[index].name_ch);
        //contentTxt.text = currentBlock.block[index].content;
        nameTxt.text = CharacterName();
        index++;

        if (index >= currentBlock.block.Count)
        {
            if (currentBlock.disableObj != null) {
                if (currentBlock.disableObj.GetComponent<Animator>()) {
                    currentBlock.disableObj.GetComponent<Animator>().SetTrigger("OFF");
                    StartCoroutine(DisableObj(currentBlock.disableObj, 0.3f));
                }
                else currentBlock.disableObj.SetActive(false);
            }
            Debug.Log("²ËÂü");
            isNoNext = false;
            index = 0;
            switch (currentBlock.ifEnd.ToString())
            {
                case "CHOICE": //choice
                    {
                        isNoNext = true;
                        SetChoice();
                    }
                    break;
                case "HEART": //heart
                    {
                        heart.HeartUp(currentBlock.heartAdd);
                        currentBlock = currentBlock.newBlock;
                        //reset and restart

                        ChangeDialogue();
                    }
                    break;
                case "NEW": //new
                    {

                        currentBlock = currentBlock.newBlock;
                        //reset and restart

                        ChangeDialogue();
                    } break;
                case "MINIGAME"://minigame
                    {
                        isNoNext = true;
                        dialogWhole.SetTrigger("OFF");
                        StartCoroutine(DisableObj(dialogWhole.gameObject, 0.25f));
                    }
                    break;
            }
            return;
        }

    }


    void SetChoice(){
        choiceWhole.SetActive(false);
        choiceWhole.SetActive(true);
        choiceB.gameObject.SetActive(currentBlock.choiceB.moveTo != null);
        choiceC.gameObject.SetActive(currentBlock.choiceB.moveTo != null);

        choiceA.SetChoice(currentBlock.choiceA.choiceName, currentBlock.choiceA.clip);
        choiceB.SetChoice(currentBlock.choiceB.choiceName, currentBlock.choiceB.clip);
        choiceC.SetChoice(currentBlock.choiceC.choiceName, currentBlock.choiceC.clip);
    }

    public void OnButtonClick(int choice) {
        isNoNext = false;
        index = 0;
        switch (choice) {
            case 0:
                {
                    currentBlock = currentBlock.choiceA.moveTo;
                }
                break;
            case 1:
                {
                    currentBlock = currentBlock.choiceB.moveTo;
                }
                break;
            case 2:
                {
                    currentBlock = currentBlock.choiceC.moveTo;
                }
                break;
        }
        
        currentBlock.startDelaySecond = 1f;
        ChangeDialogue();
    }

    IEnumerator DisableObj(GameObject obj, float seconds) {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);

        yield return null;
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentBlock.startDelaySecond = 0.0f;
        ChangeDialogue();

        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isNoNext&&Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ChangeDialogue();
        }


    }
}
