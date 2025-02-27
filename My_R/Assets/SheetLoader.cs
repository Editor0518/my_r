using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class SheetLoader : MonoBehaviour
{
    public static SheetLoader instance;
    public SheetData sheetData;
    string strSheetData;
    static string url = "https://docs.google.com/spreadsheets/d/1zLN0G9wqISQeQfYmFMTT0c_MJNNW0AlWNWzJupQrMYY/export?format=tsv&gid=";

    //A2열부터 M열까지 가져오는 링크

    static string[] gids = { "0", "0", "751234491", "1871790820", "1155518543" };
    //챕터1 = 0
    //챕터2-데이트 = 751234491
    //챕터2-뮈쟁 = 1871790820
    //챕터3 = 1155518543
    public static int chapNumber = 1;

    public static bool isLoading = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartLoadSheet();
    }

    public void StartLoadSheet()
    {
        SheetData.instance.storyBlock = new(0);
        StartCoroutine(LoadSheetOnStart());
    }

    IEnumerator LoadSheetOnStart()
    {
        string sheetURL = url + gids[chapNumber] + "&range=A2:P";
        using (UnityWebRequest www = UnityWebRequest.Get(sheetURL))
        {
            yield return www.SendWebRequest();

            if (www.isDone) strSheetData = www.downloadHandler.text;
            else Debug.Log("Error: " + www.error);
        }
        isLoading = true;
        DisplayText();
        LoadInSheetData();
        DialogueManager.instance.crtChapter = chapNumber;
    }



    void DisplayText()
    {
        Debug.Log(strSheetData);

    }

    public void LoadInSheetData()
    {

        int branch = 0;
        string[] rows = strSheetData.Split('\n');
        for (int i = 0; i < rows.Length; i++)
        {
            //0=branch  1=start_cmd  2=name  3=focus  4=left  5=center  6=right  7=lFACE  8=cFACE  9=rFACE 10=MEMO  11=content  12=thinkingcontent  13=font  14=after_cmd  15=move


            string[] columns = rows[i].Replace("\r", "").Split('\t');
            if (columns[0].Equals(columns[11]) && columns[1].Equals(""))
            {
                Debug.Log("빈 줄입니다!:" + columns[0] + ", " + columns[1] + ", " + columns[2] + ", " + columns[3] + ", " + columns[4] + ", " + columns[5] + ", " + columns[6] + ", " + columns[7] + ", " + columns[8] + ", " + columns[9] + ", " + columns[10]);
                continue;
            }
            if (int.TryParse(columns[0], out int newBranch))//branch 새로 생성
            {
                branch = newBranch;
                //branch	start_cmd	name	focus	left	center	
                //right	dialog	font	after_cmd	move
                StoryBlock storyBlock = new StoryBlock(branch);
                // Debug.Log(columns[0] + ", " + columns[1] + ", " + columns[2] + ", " + columns[3] + ", " + columns[4] + ", " + columns[5] + ", " + columns[6] + ", " + columns[7] + ", " + columns[8] + ", " + columns[9] + ", " + columns[10]);
                Block block = MakeBlock(columns);
                storyBlock.AddBlock(block);
                sheetData.AddStoryBlock(storyBlock);

            }
            else
            {
                if (columns[0].Equals("//")) continue; //주석. 처리X, 스킵
                                                       //{ //하위 branch로

                Block block = MakeBlock(columns);
                sheetData.AddBlock(branch, block);
                //}
                //else continue; //주석. 처리X, 스킵
            }
        }
        isLoading = false;
        GameObject dialogueManagerObj = GameObject.FindGameObjectWithTag("DialogueManager");
        if (dialogueManagerObj != null)
        {
            dialogueManagerObj.GetComponent<DialogueManager>().StartDialogueManager();
        }
        else
            Debug.Log("DialogueManager를 찾을 수 없습니다!");
    }

    string background = "";

    Block MakeBlock(string[] columns)
    {
        int focus = int.TryParse(columns[3], out int focusOn) ? focusOn : -1;

        if (!columns[4].Equals(""))
        {
            columns[4] += "_" + columns[7];
        }
        if (!columns[5].Equals(""))
        {
            columns[5] += "_" + columns[8];
        }
        if (!columns[6].Equals(""))
        {
            columns[6] += "_" + columns[9];
        }


        if (columns[1].Contains("background_"))
        {

            background = ExtractBackgroundName(columns[1]);
            columns[1] = columns[1].Replace("background_" + background + ";", "");
            columns[1] = columns[1].Replace("background_" + background + "", "");

        }
        if (columns[1].Contains("CHOICE_"))
        {

            string choice = "CHOICE_" + ExtractChoice(columns[1]);
            //aftercmd
            if (columns[14].Equals("")) columns[14] = choice;
            else columns[14] += ";" + choice;
            columns[1] = columns[1].Replace(choice, "");
        }
        if (columns[1].Contains("MOVECMD_"))
        {
            string moveCmd = "MOVECMD_" + ExtractMoveCmd(columns[1]);
            //aftercmd
            if (columns[14].Equals("")) columns[14] = moveCmd;
            else columns[14] += ";" + moveCmd;
            columns[1] = columns[1].Replace(moveCmd, "");


        }
        if (columns[14].Contains("MINIGAME_"))
        {
            string isNoNext = "isNoNext_false";
            if (columns[1].Equals("")) columns[1] = isNoNext;
            else columns[14] += ";" + isNoNext;

        }
        Block block = new Block(background, columns[1], columns[2], focus, columns[4], columns[5], columns[6], columns[11], columns[12], columns[13], columns[14], columns[15]);
        return block;
    }



    static string ExtractBackgroundName(string input)
    {
        Match match = Regex.Match(input, @"background_([^;_]+)");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    static string ExtractChoice(string input)
    {
        Match match = Regex.Match(input, @"CHOICE_([^;_]+)");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    static string ExtractMoveCmd(string input)
    {
        Match match = Regex.Match(input, @"MOVECMD_([^;_]+)");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

}
