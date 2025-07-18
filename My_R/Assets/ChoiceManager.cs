using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public DialogueMaster dialogueMaster;
    [Header("Choice")]
    public GameObject choiceWhole;
    public List<ChoiceSelect> choices;


    private void Start()
    {
        choiceWhole.SetActive(false);
    }

    public void SetChoice(string count)
    {
        Debug.Log("Choice start:");
        int choiceCount = int.Parse(count);

        for (int i = 0; i < choices.Count; i++)
        {
            if (i < choiceCount)
            {
                if (dialogueMaster.currentBlockList[dialogueMaster.currentPage+ (i + 1)].start_cmd.Equals(""))
                {
                    SetChoiceOne(i);
                }
                else
                {//������ �ִ� ��� true�϶��� setactive
                    string condition = dialogueMaster.currentBlockList[dialogueMaster.currentPage+ (i + 1)].start_cmd;

                    if (condition.Contains("=="))
                    {
                        string[] spl = condition.Split("==");
                        if (PlayerPrefs.GetString(spl[0], "").Equals(spl[1]))
                        {
                            SetChoiceOne(i);
                        }
                        else
                        {
                            choices[i].gameObject.SetActive(false);
                        }
                    }
                    else if (condition.Contains("!="))
                    {
                        string[] spl = condition.Split("!=");
                        if (!PlayerPrefs.GetString(spl[0], "").Equals(spl[1]))
                        {
                            SetChoiceOne(i);
                        }
                        else
                        {
                            choices[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        choiceWhole.SetActive(false);
        choiceWhole.SetActive(true);

        DialogueMaster.isNoNext = true;
        DialogueMaster.canClickToNext = false;

    }

    void SetChoiceOne(int index)
    {
        int page = dialogueMaster.currentPage + (index + 1);
        choices[index].gameObject.SetActive(true);
        string[] ch = new string[]{
            dialogueMaster.currentBlockList[page].name,
            dialogueMaster.currentBlockList[page].content,
            dialogueMaster.currentBlockList[page].after_cmd,
            dialogueMaster.currentBlockList[page].move
                };
        
        Debug.Log("name:"+ch[0]);
        Debug.Log("content:"+ch[1]);
        Debug.Log("after:"+ch[2]);
        Debug.Log("move:"+ch[3]);
        choices[index].SetChoice(ch[0], ch[1], ch[2], ch[3]);
    }
    
    public void OnButtonClick(int choice)
    {
        // startDelaySecond = 1.0f;
        // Debug.Log("Choice�� after_cmd: " + choices[choice].choice_after_cmd);
        dialogueMaster.RunCMD(choices[choice].choice_after_cmd);
        dialogueMaster.MoveBranch(dialogueMaster.currentChapter, int.Parse(choices[choice].move));
    }
}
