using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    public enum state
    {
        MAIN, ITEM
    };

    public state thisMode;


    [Header("Temp")]
    public TMP_InputField inputField;

    [Header("Dialogue")]
    public FontManager fontManager;
    public Canvas uiCanvas;
    Inventory inventory;

    public Animator dialogWhole;
    public TMP_Text nameTxt;
    public TMP_Text contentTxt;
    public RectTransform contentTrans;
    public Volume currentVolumeObj;


    [Header("Standing And EffectDirecting")]
    public SpriteRenderer backgroundRender;
    public SoundManager soundManager;
    public DirectingManager dirManager;
    public StandingSpriteManager sprManager;
    public SpriteRenderer[] standingImg = new SpriteRenderer[3];


    [Header("Choice")]
    public GameObject choiceWhole;
    public List<ChoiceSelect> choices;
    //public ChoiceSelect choiceB;
    //public ChoiceSelect choiceC;

    [Header("Storyline")]
    SheetData sheetData;
    public int crtChapter = 1;
    public int crtSubchapter = 1;
    [Space]

    public int sheetBranch = 1; //쓰지 마셈. 이거 쓰는거 아님. 밑에 인덱스 써야됨.
    public int crtBranch = 0;
    public int crtPage = 0;
    public bool isNoNext = false;

    [Header("When END")]
    public GameObject endObj;

    [Header("Others")]
    //public Heart heart;

    public TypeWriter typeWriter;
    public bool canUseScroll = false;
    public bool isPause = false;
    public bool isViewPause = false;
    public bool canClickToNext = true;
    bool isMiniOn = false;

    //temp
    public string myName = "앙졸라스";
    bool hasUnderletter = false;
    public bool isShowThinking = false;

    public PolygonCollider2D col_Enj;
    public PolygonCollider2D col_R;

    bool isAllGrey = false;

    public static float FIXED_HEIGHT = Screen.height; //* 0.45f;
    float startDelaySecond = 0.0f;

    public void ChangeMyName()
    {
        myName = inputField.text;
    }
    public void ChangeIsShortHair(bool isShort)
    {
        sprManager.isShortHair = isShort;
    }

    private void Start()
    {
        //        StartDialogueManager();

    }

    public void StartDialogueManager()
    {
        sheetData = SheetData.instance;
        inventory = uiCanvas.GetComponent<Inventory>();
        contentTrans = contentTxt.GetComponent<RectTransform>();

        crtBranch = sheetData.FindBranchIndex(sheetBranch);

        if (thisMode.Equals(state.MAIN))
        {
            if (crtBranch >= 0)
            {
                // ChangeBackground();
                ChangeDialogue();
            }
        }
        hasUnderletter = UnderLetter.HasUnderLetter(myName);
    }


    void ChangeCharacterName()
    {
        string name = sheetData.storyBlock[crtBranch].block[crtPage].name;

        //GRANTAIRE, ENJOLRAS, X, COMBEFERRE, JOLY, COURFEYRAC, LAMARQUE
        switch (name)
        {
            case "GRANTAIRE"://GRANTAIRE
            case "그랑테르":
                {
                    nameTxt.color = new Color32(185, 222, 125, 255);
                }
                break;
            case "ENJOLRAS":
            case "앙졸라스"://ENJOLRAS
                {
                    name = myName;//앙졸라스
                    nameTxt.color = new Color32(255, 235, 122, 255);
                    //isAllGrey = false;
                }
                break;
            case "COMBEFERRE"://COMBEFERRE
            case "콩브페르":
                {
                    nameTxt.color = new Color32(150, 232, 253, 255);
                }
                break;
            case "JOLY"://JOLY
            case "졸리":
                {
                    nameTxt.color = new Color32(223, 111, 59, 255);
                }
                break;
            case "COURFEYRAC"://COURFEYRAC
            case "쿠르페락":
                {
                    nameTxt.color = new Color32(1, 200, 178, 255);
                }
                break;
            case "LAMARQUE"://LAMARQUE
            case "라마르크":
                {
                    nameTxt.color = new Color32(235, 173, 228, 255);
                }
                break;
            case "MUSICHETTA":
            case "뮈지세타":
            case "카페 주인":
                {
                    nameTxt.color = Color.white;
                }
                break;
            case "BAHOREL":
            case "바오렐":
                {
                    nameTxt.color = new Color32(186, 38, 1, 255);
                }
                break;
            case "JEHAN":
            case "장 프루베르":
                {
                    nameTxt.color = new Color32(245, 152, 152, 255);
                }
                break;
            case "FEUILLY":
            case "푀이":
                {
                    nameTxt.color = new Color32(148, 169, 193, 255);
                }
                break;
            case "BOSSUET":
            case "보쉬에":
                {
                    nameTxt.color = new Color32(168, 131, 114, 255);
                }
                break;

            //186 38 1 바오렐
            //245 152 152 즈앙
            //148 169 193 푀이
            //168 131 114 보쉬에
            default:
                // name_ch = name;
                nameTxt.color = new Color32(194, 194, 194, 255);
                break;
        }

        nameTxt.text = name;
    }


    //대사의 이름 "앙졸라스"를 유저가 정한 이름으로 바꿈
    public string ReplaceEnjolrasName(string content)
    {
        if (myName.Equals("앙졸라스")) return content;

        //string content = currentBlock.block[index].content;
        if (content.Contains("앙졸라스"))
        {
            if (content.Contains("앙졸라스가")) content = content.Replace("앙졸라스가", UnderLetter.SetUnderLetter(myName, '가'));
            if (content.Contains("앙졸라스를")) content = content.Replace("앙졸라스를", UnderLetter.SetUnderLetter(myName, '를'));
            if (content.Contains("앙졸라스는")) content = content.Replace("앙졸라스는", UnderLetter.SetUnderLetter(myName, '는'));
            if (content.Contains("앙졸라스와")) content = content.Replace("앙졸라스와", UnderLetter.SetUnderLetter(myName, '와'));
            content = content.Replace("앙졸라스", myName);
            //currentBlock.block[index].content=content;
            //Debug.Log(currentBlock.block[index].content);
        }
        return content;
    }

    public void ChangeDialogue()
    {
        if (!canClickToNext || isNoNext) return;


        if (crtPage >= sheetData.storyBlock[crtBranch].block.Count)
        {
            if (sheetData.storyBlock[crtBranch].ifEnd.Equals(If_end.CHOICE)) canClickToNext = false;
            else
            {
                Debug.Log("꽉참!!! 다음 어디!!!!");
                return;
            }
        }
        /*if (currentBlock.bgm != null)
        {
            if (SoundManager.instance != null) SoundManager.instance.PlayBGM(currentBlock.bgm, currentBlock.bgmSubtitle, !currentBlock.isBGMNoLoop);
            else soundManager.PlayBGM(currentBlock.bgm, currentBlock.bgmSubtitle, !currentBlock.isBGMNoLoop);
            //if (currentBlock.se != null) SoundManager.instance.PlaySE(currentBlock.se);
        }*/

        if (startDelaySecond > 0)
        {
            StartCoroutine(Delay(startDelaySecond));
            return;
        }
        else
        {

            if (!dialogWhole.gameObject.activeInHierarchy)
            {
                dialogWhole.gameObject.SetActive(true);
                Debug.Log("다이얼로그가 꺼져있어서 켯음. 이거 안 나오도록 코드 수정 하셈.");
            }

        }


        if (typeWriter.isTyping)
        {
            typeWriter.StopTyping();
            return;

        }
        else
        {
            // if (sheetData.storyBlock[crtBranch].block[crtPage].se != null)
            //  {
            // SoundManager.instance.PlaySound(sheetData.storyBlock[crtBranch].block[crtPage].se, sheetData.storyBlock[crtBranch].block[crtPage].seSubtitle);
            //  }
            //if(영어버전)string content =content_ENG
            sheetData.storyBlock[crtBranch].block[crtPage].content = ReplaceEnjolrasName(sheetData.storyBlock[crtBranch].block[crtPage].content);

            string content = sheetData.storyBlock[crtBranch].block[crtPage].content;

            //폰트 변경
            contentTxt.font = fontManager.GetFont(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.fontSize = fontManager.GetFontSize(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.lineSpacing = fontManager.GetLineSpacing(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.characterSpacing = fontManager.GetCharacterSpacing(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.wordSpacing = fontManager.GetWordSpacing(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTrans.anchoredPosition = new Vector2(contentTrans.anchoredPosition.x, -145 + fontManager.GetAddPosY(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString()));

            //변수 치환 {변수명} -> PlayerPrefs.GetString(변수명)
            if (content.Contains("{"))
            {
                string newSentence = "";

                string[] spl = content.Split("{");
                for (int i = 0; i < spl.Length; i++)
                {
                    if (!spl[i].Contains("}"))
                    {
                        newSentence += spl[i];
                        continue;
                    }
                    string[] end = spl[i].Split("}");
                    newSentence += PlayerPrefs.GetString(end[0], "null") + end[1];

                }
                content = newSentence;
                Debug.Log(content);
            }

            typeWriter.StartTyping(content, 0, !sheetData.storyBlock[crtBranch].block[crtPage].name.Equals(""));//(int)currentBlock.block[index].name_ch

            if (isShowThinking)
            {
                sheetData.storyBlock[crtBranch].block[crtPage].thinking = ReplaceEnjolrasName(sheetData.storyBlock[crtBranch].block[crtPage].thinking);
                typeWriter.ThinkingOn(sheetData.storyBlock[crtBranch].block[crtPage].thinking);
            }
            else typeWriter.ThinkingOff();
            //contentTxt.text = currentBlock.block[index].content;

            ChangeCharacterName();

            isAllGrey = false;
            RunCMD(sheetData.storyBlock[crtBranch].block[crtPage].start_cmd);//ChangeSprite보다 뒤에있으면 버그남!!!!
            ChangeSprite();
            if (!sheetData.storyBlock[crtBranch].block[crtPage].move.Equals(""))
            {
                MoveBranchHold(sheetData.storyBlock[crtBranch].block[crtPage].move);
            }
            else crtPage += 1;
            // if (index >= currentBlock.block.Count) isNoNext = true;
            //else isNoNext = false;

        }
    }


    void WhenLastPage()
    {
        /*
        switch (sheetData.storyBlock[crtBranch].ifEnd.ToString())
        {
            case "CHOICE": //choice
                {
                    isNoNext = true;
                    SetChoice();
                }
                break;
            case "NEW": //new
                {
                    ChangeCurrentBlock(currentBlock.newBlock);


                }
                break;
            case "MINIGAME"://minigame
                {
                    isNoNext = true;

                    dialogWhole.SetTrigger("OFF");
                    DisableSprite();
                    StartCoroutine(DisableObj(dialogWhole.gameObject, 0.25f));
                    currentBlock.minigameObj.SetActive(true);
                }
                break;
            case "GIFT"://gift
                {
                    //gift 여는 스크립트-인벤토리or선물 스크립트와 연동하기
                    inventory.StartGiftEvent(currentBlock.itemBlock);

                    //currentBlock = 
                    //ChangeDialogue();
                }
                break;


        if (thisMode.Equals(state.ITEM))
        {
            currentBlock = null;
            isNoNext = true;
        }*/
    }

    //미니게임 등에서 버튼 클릭 시 다음 다이얼로그 넘길때 쓰기.
    public void ChangeCurrentBlock(int newBranch)
    {
        MoveBranch(newBranch.ToString());

    }


    #region 커맨드 실행 관련 함수
    void RunCMD(string CMD)
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
                //변수 저장하기
                //vh_변수A=value
                string[] spl = cmdStr[1].Split('=');
                PlayerPrefs.SetString(spl[0], (spl[1]));

                break;
            case "is":
                //비교
                //is_변수A==1?  CMD1  :  CMD2
                string[] cmdRes = cmdStr[1].Split('?');//변수A==1   ?   CMD1:CMD2
                string[] cmdResult = cmdRes[1].Split(':');//CMD1     :    CMD2
                                                          //string[] cmdVars = cmdStr[1].Split("&&");
                string[] cmdVar = cmdStr[1].Split("==");//변수A    ==    1

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

                SetChoice();
                break;
            case "MOVECMD"://movecmd
                /*
                for (int i = 0; i < currentBlock.itemBlock.Count; i++)
                {
                    string[] spl = currentBlock.itemBlock[i].itemName.Split("==");
                    if (PlayerPrefs.GetString(spl[0], "").Equals(spl[1]))
                    {
                        ChangeCurrentBlock(currentBlock.itemBlock[i].newBlock);
                        return;
                    }
                }
                Debug.LogError("오류 발생! 변수 못 찾음!");
                */
                break;
            case "startdelay":
                // currentBlock.startDelaySecond = float.Parse(cmdStr[1]);
                //    Debug.Log(currentBlock.startDelaySecond);
                break;
            case "minigameobj":
                // currentBlock.minigameObj.SetActive(true);
                break;
            case "bgm":
                if (cmdStr[1].Equals("end")) SoundManager.instance.EndBGM();
                else if (cmdStr[1].Equals("stop")) SoundManager.instance.StopBGM();
                else if (cmdStr[1].Equals("pause")) SoundManager.instance.PauseBGM();
                else if (cmdStr[1].Equals("play")) SoundManager.instance.ReplayBGM();
                break;
            case "se":
                if (cmdStr[1].Equals("stop")) SoundManager.instance.StopSound();
                break;
            case "givefrom":
                cmdStr[1] = PlayerPrefs.GetString(cmdStr[1], "");
                cmdStr[0] = "give_";
                RunCMD_One(cmdStr[0] + cmdStr[1]);
                break;
            case "give":
                //item give
                inventory.AddItem(cmdStr[1]);
                Debug.Log("아이템 획득: " + CMD[1]);
                break;
            case "remove":
                //item remove
                inventory.RemoveItem(cmdStr[1]);
                break;
            default:
                dirManager.RunCMD(cmdStr[0], (cmdStr.Length > 1 ? cmdStr[1] : ""));
                break;

        }


    }
    public void ChangeIsMiniOn(bool isOn)
    {
        isMiniOn = isOn;
    }
    public void RunAfterCMD()
    {
        if (crtPage < 1) return;
        crtPage -= 1;
        RunCMD(sheetData.storyBlock[crtBranch].block[crtPage].after_cmd);
        crtPage += 1;
    }

    #endregion

    void DisableSprite()
    {
        for (int i = 0; i < standingImg.Length; i++)
        {
            standingImg[i].enabled = false;
        }

    }

    void ChangeSprite()
    {
        string[] standing = sheetData.storyBlock[crtBranch].block[crtPage].standing;
        int focusOn = isAllGrey ? -1 : sheetData.storyBlock[crtBranch].block[crtPage].focus;
        if (sheetData.storyBlock[crtBranch].block[crtPage].name.Equals("")) focusOn = -1;

        //  bool isNoName = currentBlock.block[index].isNoName;
        Color gray = new Color32(163, 164, 168, 255);
        // Debug.Log("focusOn: " + focusOn);
        //StandingSpriteManager에서 스프라이트를 찾아옴. (캐 이름, 얼굴 종류 파라미터로 필요.)

        for (int i = 0; i < standingImg.Length; i++)
        {
            string[] spr1 = standing[0].Split('_');
            if (spr1.Length <= 1)
                spr1 = new string[] { "", "" };
            Sprite spr = sprManager.FindSprite(spr1[0], spr1[1]);

            standingImg[i].sprite = spr;
            standingImg[i].color = (focusOn == 0) ? Color.white : gray;
            standingImg[i].enabled = (spr != null && !isMiniOn);

        }


        /*//비맞는
        switch ((int)currentBlock.block[index].name_ch)
        {
            case 0:
                col_Enj.enabled = false;
                col_R.enabled = true;
                break;
            case 1:
                col_Enj.enabled = true;
                col_R.enabled = false;
                break;
            default:
                col_Enj.enabled = false;
                col_R.enabled = false;
                break;
        }*/

    }

    void SetChoice()
    {

        int choiceCount = int.Parse(SheetData.instance.storyBlock[crtBranch].block[crtPage].start_cmd.Split('_')[1]);

        for (int i = 0; i < choices.Count; i++)
        {
            if (i < choiceCount)
            {
                if (SheetData.instance.storyBlock[crtBranch].block[crtPage + (i + 1)].start_cmd.Equals(""))
                {

                    SetChoice(i);
                }
                else
                {//변수가 있는 경우 true일때만 setactive
                    //SheetData.instance.storyBlock[0].block[0].start_cmd
                }
            }
            else
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        choiceWhole.SetActive(false);
        choiceWhole.SetActive(true);

        isNoNext = true;
        canClickToNext = false;

    }

    void SetChoice(int index)
    {
        int page = crtPage + (index + 1);
        choices[index].gameObject.SetActive(true);
        string[] ch = new string[]{
                    SheetData.instance.storyBlock[crtBranch].block[page].name,
                    SheetData.instance.storyBlock[crtBranch].block[page].content,
                    SheetData.instance.storyBlock[crtBranch].block[page].after_cmd,
                    SheetData.instance.storyBlock[crtBranch].block[page].move
                };
        choices[index].SetChoice(ch[0], ch[1], ch[2], ch[3]);
    }

    //선택지 클릭 시
    public void OnButtonClick(int choice)
    {
        startDelaySecond = 1.0f;
        RunCMD(choices[choice].choice_after_cmd);
        MoveBranch(choices[choice].move);
    }

    IEnumerator DisableObj(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);

        yield return null;
    }

    IEnumerator Delay(float seconds)
    {
        canClickToNext = false;
        dialogWhole.gameObject.SetActive(false);
        yield return new WaitForSeconds(seconds);
        Debug.Log("Play again");
        canClickToNext = true;
        dialogWhole.gameObject.SetActive(true);
        startDelaySecond = 0.0f;
        ChangeDialogue();

        yield return null;
    }

    //branch 변경 후 다이얼로그 자동 재생
    public void MoveBranch(string branch)
    {
        if (MoveBranchHold(branch)) ChangeDialogue();
    }

    //branch 변경만. 다음 클릭하면 바뀐 채 다이얼로그 재생.
    public bool MoveBranchHold(string branch)
    {
        return MoveBranchHold(branch, 0);
    }

    public bool MoveBranchHold(string branch, int page)
    {
        if (int.TryParse(branch, out int result))
        {
            //reset and restart
            canClickToNext = true;
            isNoNext = false;
            sheetBranch = result;
            crtBranch = sheetData.FindBranchIndex(sheetBranch);
            crtPage = page;
            return true;
        }
        else if (branch.Equals("END"))
        {

            isNoNext = true;
            dialogWhole.SetTrigger("OFF");
            SoundManager.instance.EndBGM();
            SoundManager.instance.StopSound();
            dirManager.ScreenClear();
            dirManager.StandingClear(0);
            dirManager.StandingClear(1);
            dirManager.StandingClear(2);
            dirManager.DefaultPos();
            dirManager.DisappearMiniCutscene();
            StartCoroutine(DisableObj(dialogWhole.gameObject, 0.25f));
            endObj.SetActive(true);
        }

        return false;
    }


    public TMP_Text tmptext;


    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            isViewPause = !isViewPause;

            uiCanvas.enabled = !isViewPause;
        }

        if (isPause || isViewPause) return;

        //Spacebar or EnterKey, or Click Between bottom 0~700
        if (canClickToNext && !isNoNext && ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.mousePosition.y <= FIXED_HEIGHT && Input.GetMouseButtonDown(0))) ||
            (canUseScroll && Input.mouseScrollDelta.y != 0.0)))
        {
            tmptext.text = Input.mousePosition.y.ToString();
            ChangeDialogue();
        }

#if UNITY_IOS
        if (Input.touchCount > 0)
        {
            if (!isNoNext && (Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).position.y <= FIXED_HEIGHT))
            {
                tmptext.text = Input.GetTouch(0).position.y.ToString();
                ChangeDialogue();
            }
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (!isNoNext && (Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).position.y <= FIXED_HEIGHT))
            {
                tmptext.text = Input.GetTouch(0).position.y.ToString();
                ChangeDialogue();
            }
        }
#endif

    }
}
