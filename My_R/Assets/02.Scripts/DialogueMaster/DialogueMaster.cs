using System;
using UnityEngine;
using System.Collections.Generic;

public class DialogueMaster : MonoBehaviour
{
    public static DialogueMaster Instance;

    [Header("Managers")]
    public DialogueUIManager dialogueUI;
    public StandingManager standingManager;
    public DirectingManager dirManager;
    public ChoiceManager choiceManager;
    
    public int currentChapter;
    [SerializeField]private int currentBranch;
    public int currentPage;

    public List<Block> currentBlockList;

    public static bool canClickToNext=true;
    public static bool isPause;
    public static bool isAuto = false;  // ✅ 자동 모드 토글
    public static bool isNoNext = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartDialogue(int chapter, int branch)
    {
        currentChapter = chapter;
        currentBranch = branch;
        currentPage = 0;

        canClickToNext = true;
        isNoNext = false;
        
        LoadBranchData();
       
        ContinueDialogue();
    }

    public void MoveBranch(int chapter, int branch)
    {
        StartDialogue(currentChapter, currentBranch);
    }
    
    public void MoveBranchHold(string branch)
    {
        MoveBranchHold(branch, 0);
    }
    
    public void MoveBranchHold(string branch, int page)
    {
        currentPage = page;
        if (int.TryParse(branch, out int result))
        {
            //reset and restart
            canClickToNext = true;
            isNoNext = false;
            
            currentBranch = result;
            currentPage = 0;

            LoadBranchData();
        }
        else if (branch.Contains("END"))
        {

            isNoNext = true;
           // StartCoroutine(WaitUntilEnd(branch));

        }
        else if (branch.Contains("MINIGAME"))
        {
            string[] spl = branch.Split('_');
            if (spl[1].Equals("END"))
            {
                //EndMinigame();
            }
            else if (spl[1].Equals("REOPEN"))
            {
                //currentMinigame.SetActive(false);
                //currentMinigame.SetActive(true);
                //dialogWhole.SetTrigger("OFF");
              //  StartCoroutine(DisableObj(dialogWhole.gameObject, 1.5f));
                canClickToNext = false;
                isNoNext = true;

            }
            else if (spl[1].Contains("PHONE:"))
            {
               // currentMinigame.GetComponent<Minigame_Phone>().ChangeState(int.Parse(spl[1].Replace("PHONE:", "")));

                canClickToNext = false;
                isNoNext = true;

            }
            else
            {
                //StartMinigame(spl[1]);
            }
        }
       
    }
    


    private void LoadBranchData()
    {
        int index = SheetData.instance.FindBranchIndex(currentBranch);
        if (index == -1)
        {
            Debug.LogError($"브랜치 {currentBranch} 데이터를 찾을 수 없습니다.");
            currentBlockList = new List<Block>();
        }
        else
        {
            currentBlockList = SheetData.instance.storyBlock[index].block;
        }
    }
    
    public void ContinueDialogue()
    {
        if (currentPage >= currentBlockList.Count)
        {
            dialogueUI.HideDialogueBox();
            return;
        }

        Block line = currentBlockList[currentPage];
        
        RunCMD(line.start_cmd);

        dialogueUI.DisplayLine(line); // ✅ 대사 출력 요청
        standingManager.UpdateStanding(line);

       
        /*if (isShowThinking)
        {
            sheetData.storyBlock[crtBranch].block[crtPage].thinking = ReplaceEnjolrasName(sheetData.storyBlock[crtBranch].block[crtPage].thinking);
            typeWriter.ThinkingOn(sheetData.storyBlock[crtBranch].block[crtPage].thinking);
        }
        else typeWriter.ThinkingOff();*/
        //contentTxt.text = currentBlock.block[index].content;

        dirManager.ChangeBackground(currentBlockList[currentPage].background);
        dialogueUI.ChangeCharacterName(currentBlockList[currentPage].name);

        //isAllGrey = false;
        //RunCMD(sheetData.storyBlock[crtBranch].block[crtPage].start_cmd);//ChangeSprite���� �ڿ������� ���׳�!!!!
        //ChangeSprite();
        if (!currentBlockList[currentPage].move.Equals(""))
        {
            MoveBranchHold((currentBlockList[currentPage].move));
        }
        currentPage++;


    }
    
    
    public void RunCMD(string CMD)
    {

        if (CMD == "") return;
        CMD = CMD.Replace("; ", ";");
        string[] cmd = CMD.Split(';');
        if (cmd.Length > 0)
        {
            for (int i = 0; i < cmd.Length; i++)
            {
                RunCMD_One(cmd[i]);
            }
        }
    }

    void RunCMD_One(string CMD)
    {
        string[] cmdStr = CMD.Split('_');
        switch (cmdStr[0])
        {
            case "vh":
                //���� �����ϱ�
                //vh_����A=value

                string[] spl = cmdStr[1].Split('=');
                PlayerPrefs.SetString(spl[0], (spl[1]));
                PlayerPrefs.Save();
                Debug.Log("���� �����: " + spl[0] + ": " + PlayerPrefs.GetString(spl[0], "null"));
                break;
            case "is":
                //��
                //is_����A==1?  CMD1  :  CMD2
                string[] cmdRes = cmdStr[1].Split('?');//����A==1   ?   CMD1:CMD2
                string[] cmdResult = cmdRes[1].Split(':');//CMD1     :    CMD2
                                                          //string[] cmdVars = cmdStr[1].Split("&&");
                string[] cmdVar = cmdStr[1].Split("==");//����A    ==    1

                if (cmdVar[0] == cmdVar[1])
                {
                    RunCMD_One(cmdResult[0]);
                }
                else
                {
                    RunCMD_One(cmdResult[1]);
                }

                break;
            case "CHOICE": //choice

                isNoNext = true;
                canClickToNext = false;

                choiceManager.SetChoice(cmdStr[1]);
                break;
            case "MOVECMD"://movecmd

                for (int i = 0; i < int.Parse(cmdStr[1]); i++)
                {
                    string cmd = currentBlockList[currentPage+ (i + 1)].start_cmd;

                    if (cmd.Contains("=="))
                    {
                        string[] split = cmd.Split("==");
                        Debug.Log("�� �� : " + split[0] + ", " + PlayerPrefs.GetString(split[0], "") + "==?" + (split[1]));
                        if (PlayerPrefs.GetString(split[0], "null").Equals(split[1]))
                        {
                            currentBlockList[currentPage].move = currentBlockList[currentPage + (i + 1)].move;
                            Debug.Log("����� ����:" + currentBlockList[currentPage+(i + 1)].move);
                            RunCMD(currentBlockList[currentPage + (i + 1)].after_cmd);//Move���� �տ� �־�� ���� �ȳ�
                            //MoveBranchHold(currentBlockList[currentPage].move, -1);
                            //MoveBranchHold(SheetData.instance.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            //  crtPage--;

                            break;
                        }
                    }
                    else if (cmd.Contains("!="))
                    {
                        string[] split = cmd.Split("!=");
                        if (!PlayerPrefs.GetString(split[0], "null").Equals(split[1]))
                        {
                            currentBlockList[currentPage].move = currentBlockList[currentPage + (i + 1)].move;
                            // MoveBranchHold(SheetData.instance.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            RunCMD(currentBlockList[currentPage+ (i + 1)].after_cmd);
                            Debug.Log("����� ����:" + currentBlockList[currentPage+ (i + 1)].move);
                           // MoveBranchHold(currentBlockList[currentPage].move, -1);
                            //crtPage--;
                            break;
                        }
                    }
                    else Debug.LogError("���� �߻�! ���� �� ã��!");

                }

                break;
            case "startdelay":
                //startDelaySecond = float.Parse(cmdStr[1]);
                //    Debug.Log(currentBlock.startDelaySecond);
                break;
            case "minigameobj":

                // currentBlock.minigameObj.SetActive(true);
                break;
            case "givefrom":
                cmdStr[1] = PlayerPrefs.GetString(cmdStr[1], "");
                cmdStr[0] = "give_";
                RunCMD_One(cmdStr[0] + cmdStr[1]);
                break;
            case "give":
                //item give
                //inventory.AddItem(cmdStr[1]);
                Debug.Log("������ ȹ��: " + CMD[1]);
                break;
            case "remove":
                //item remove
                //inventory.RemoveItem(cmdStr[1]);
                break;
            case "isNoNext":
                if (cmdStr[1].Equals("true"))
                {
                    isNoNext = true;
                }
                else
                {
                    isNoNext = false;
                }
                break;
            default:
                dirManager.RunCMD(cmdStr[0], (cmdStr.Length > 1 ? cmdStr[1] : ""));
//                Debug.Log("run cmd");
                break;
            case "midText":
                if (cmdStr[1].Equals("on")) dialogueUI.TextBoxMiddle(true, currentBlockList[currentPage].content);
                else dialogueUI.TextBoxMiddle(false);//off
                
                break;
        }


    }
    public void ChangeIsMiniOn(bool isOn)
    {
       // isMiniOn = isOn;
    }
    public void RunAfterCMD()
    {
        if (currentPage < 1) return;
        currentPage -= 1;
        RunCMD(currentBlockList[currentPage].after_cmd);
        currentPage += 1;
    }

    

}