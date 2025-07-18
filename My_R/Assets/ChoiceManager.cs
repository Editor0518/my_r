using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public DialogueMaster dialogueMaster;
    [Header("Choice")] public GameObject choiceWhole;
    public List<ChoiceSelect> choices;
    bool isChoiceClosed=true;

    public void SetIsChoiceClosed(bool value)
    {
        isChoiceClosed  = value;
    }
    
    private void Start()
    {
        choiceWhole.SetActive(false);
    }

    public void SetChoice(string count, int page)
    {
        Debug.Log("Choice start:");
        int choiceCount = int.Parse(count);
        List<Block> blocks = dialogueMaster.GetCurrentBlock();
        for (int i = 0; i < choices.Count; i++)
        {
            if (i < choiceCount)
            {
               
                if (blocks[page+i].start_cmd.Equals(""))
                {
                    SetChoiceOne(i, page);
                }
                else
                {//������ �ִ� ��� true�϶��� setactive
                    string condition = blocks[page].start_cmd;

                    if (condition.Contains("=="))
                    {
                        string[] spl = condition.Split("==");
                        if (PlayerPrefs.GetString(spl[0], "").Equals(spl[1]))
                        {
                            SetChoiceOne(i, page);
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
                            SetChoiceOne(i, page);
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
        
        choiceWhole.SetActive(true);

        DialogueMaster.isNoNext = true;
        DialogueMaster.canClickToNext = false;
        SetIsChoiceClosed(false);
    }

    void SetChoiceOne(int index, int currentPage)
    {
        List<Block> blocks = dialogueMaster.GetCurrentBlock();
        int page = currentPage + (index);
        choices[index].gameObject.SetActive(true);
        string[] ch = new string[]{
            blocks[page].name,
            blocks[page].content,
            blocks[page].after_cmd,
            blocks[page].move
                };
        
//        Debug.Log("name:"+ch[0]);
     //   Debug.Log("content:"+ch[1]);
      //  Debug.Log("after:"+ch[2]);
       // Debug.Log("move:"+ch[3]);
        choices[index].SetChoice(ch[0], ch[1], ch[2], ch[3]);
    }
    
    public void OnButtonClick(int choice)
    {
        StartCoroutine(IEOnButtonClick(choice));
    }

    IEnumerator IEOnButtonClick(int choice)
    {
        yield return new WaitUntil(() => isChoiceClosed);
          
        
        // startDelaySecond = 1.0f;
        // Debug.Log("Choice�� after_cmd: " + choices[choice].choice_after_cmd);
        dialogueMaster.RunCMD(choices[choice].choice_after_cmd);
        dialogueMaster.MoveBranch(dialogueMaster.currentChapter, int.Parse(choices[choice].move));
        
    }
}
