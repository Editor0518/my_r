using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DialogueMaster : MonoBehaviour
{
    public static DialogueMaster Instance;

    [Header("Managers")]
    public MinigameHolder minigameHolder;
    public SheetData sheetData;
    public DialogueUIManager dialogueUI;
    public StandingManager standingManager;
    public DirectingManager dirManager;
    public ChoiceManager choiceManager;
    public LogManager logManager;
    
    public int currentChapter;
    [SerializeField]private int currentBranch;
    private int currentSheetBranchIndex;
    public int currentPage;
    
    public static bool canClickToNext=true;
    public static bool isPause;
    public static bool isAuto = false;  // ✅ 자동 모드 토글
    public static bool isNoNext = false;

    public GameObject episodeEnd;
    public Transform minigameHolderParent;
    private GameObject minigamePrefeb;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    
    public List<Block> GetCurrentBlock()
    {
        return sheetData.storyBlock[currentSheetBranchIndex].block;
    }


    public void StartDialogue(int chapter, int branch)
    {
        ResetCurrent(chapter, branch, 0);
        //StartCoroutine(StartDialogueCoroutine());
        ContinueDialogue();
    }

    public void ResetCurrent(int chapter, int branch, int page)
    {
        currentChapter = chapter;
        currentBranch = branch;
        currentPage = page;
        currentSheetBranchIndex = sheetData.FindBranchIndex(currentBranch);
        canClickToNext = true;
        isNoNext = false;
    }


    public void MoveBranch(int chapter, int branch)
    {
        Debug.Log("branch move to: "+branch);
        StartDialogue(currentChapter, branch);
    }
    
    public void MoveBranchHold(string branch)
    {
        MoveBranchHold(branch, 0);
    }

    public void EndTab(int nextChapter, int nextBranch)
    {
        if (currentChapter == nextChapter)
        {
            //pass
            ResetCurrent(currentChapter, nextChapter, nextBranch);
            Debug.Log("브랜치 변경");
        }
        else
        {
            //change chapter, reload needed
            Debug.Log("챕터 탭 변경");
        }
        
        EpisodeEnd.nextChapter = nextChapter;
        EpisodeEnd.nextEpisodeBranch = nextBranch;
        episodeEnd.SetActive(true);
    }
    
    
    
    public void MoveBranchHold(string branch, int page)
    {
        Debug.Log("move branch hold ->"+page);
        currentPage = page;
        if (int.TryParse(branch, out int result))
        {
            //reset and restart
            ResetCurrent(currentChapter, result, 0);
        }
        else if (branch.Contains("END"))
        {

            isNoNext = true;
            if(branch.Contains('_')){
            string[] chapBranch = branch.Split("END_")[1].Split("-");
            int nextChapter = int.Parse(chapBranch[0]);
            int nextBranch = int.Parse(chapBranch[1]);
            EndTab(nextChapter, nextBranch);
            
            }
            else
            {
                //완전 끝
                EndTab(-1,-1);
            }
            //StartCoroutine(WaitUntilEnd(branch));

        }
        else if (branch.Contains("MINIGAME"))
        {
            string[] spl = branch.Split('_');
            if (spl[1].Equals("END"))
            {
                EndMinigame();
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
                
                StartMinigame(spl[1]);
            }
        }
       
    }
    
    bool isLoaded = false;

    public void StartMinigame(string minigameName)
    {
        //MinigameHolder에서 불러와서 실행하기
        GameObject prefeb = minigameHolder.FindMinigame(minigameName);
        if(prefeb != null){
            dialogueUI.HideDialogueBox();
            standingManager.HideStandings();
            minigamePrefeb = Instantiate(prefeb, minigameHolderParent);
            minigamePrefeb.SetActive(true);
        }
        else
        {
            Debug.LogWarning(minigameName+"을 찾을 수 없습니다!");
            return;
        }
    }

    public void EndMinigame()
    {
        //미니게임 오브젝트 삭제하기
        for (int i = 0; i < minigameHolderParent.childCount; i++)
        {
            Destroy(minigameHolderParent.GetChild(i).gameObject);
        }
        
        minigamePrefeb = null;
        dialogueUI.ShowDialogueBox();//필요 없을수도
        standingManager.ShowStandings();
    }
   
    
    public void ContinueDialogue()
    {
        List<Block> blocks = GetCurrentBlock();
        
        if (currentPage >= blocks.Count)
        {
            dialogueUI.HideDialogueBox();
            return;
        }

        Block line = blocks[currentPage];
        
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

        dirManager.ChangeBackground(blocks[currentPage].background);
        dialogueUI.ChangeCharacterName(blocks[currentPage].name);

        //isAllGrey = false;
        //RunCMD(sheetData.storyBlock[crtBranch].block[crtPage].start_cmd);//ChangeSprite���� �ڿ������� ���׳�!!!!
        //ChangeSprite();
        if (!blocks[currentPage].move.Equals(""))
        {
            Debug.Log("something is in move ["+blocks[currentPage].move+"]");
            MoveBranchHold((blocks[currentPage].move));
            return;
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

                choiceManager.SetChoice(cmdStr[1], currentPage);
                break;
            case "MOVECMD"://movecmd

                for (int i = 0; i < int.Parse(cmdStr[1]); i++)
                {
                    string cmd = sheetData.storyBlock[currentSheetBranchIndex].block[currentPage+ (i + 1)].start_cmd;

                    if (cmd.Contains("=="))
                    {
                        string[] split = cmd.Split("==");
                        Debug.Log("�� �� : " + split[0] + ", " + PlayerPrefs.GetString(split[0], "") + "==?" + (split[1]));
                        if (PlayerPrefs.GetString(split[0], "null").Equals(split[1]))
                        {
                            sheetData.storyBlock[currentSheetBranchIndex].block[currentPage].move = sheetData.storyBlock[currentSheetBranchIndex].block[currentPage + (i + 1)].move;
                            Debug.Log("����� ����:" + sheetData.storyBlock[currentSheetBranchIndex].block[currentPage+(i + 1)].move);
                            RunCMD(sheetData.storyBlock[currentSheetBranchIndex].block[currentPage + (i + 1)].after_cmd);//Move���� �տ� �־�� ���� �ȳ�
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
                            sheetData.storyBlock[currentSheetBranchIndex].block[currentPage].move = sheetData.storyBlock[currentSheetBranchIndex].block[currentPage + (i + 1)].move;
                            // MoveBranchHold(SheetData.instance.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            RunCMD(sheetData.storyBlock[currentSheetBranchIndex].block[currentPage+ (i + 1)].after_cmd);
                            Debug.Log("����� ����:" + sheetData.storyBlock[currentSheetBranchIndex].block[currentPage+ (i + 1)].move);
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
                
                if (cmdStr[1].Equals("on")) dialogueUI.TextBoxMiddle(true, sheetData.storyBlock[currentSheetBranchIndex].block[currentPage].content);
                else dialogueUI.TextBoxMiddle(false);//off
                
                break;
                
        }


    }

    void SetLog()
    {
        //logManager.AddChoiceAtLog("", ""); dont use this make new
    }
    public void ChangeIsMiniOn(bool isOn)
    {
       // isMiniOn = isOn;
    }
    public void RunAfterCMD()
    {
        if (currentPage < 1) return;
        int afterPage = currentPage - 1;
        if (sheetData.storyBlock.Count <= currentSheetBranchIndex) return;
        if (sheetData.storyBlock[currentSheetBranchIndex].block.Count <= afterPage) return;
        RunCMD(sheetData.storyBlock[currentSheetBranchIndex].block[afterPage].after_cmd);
        
    }

    

}