using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Temp")]
    public TMP_InputField inputField;
    public GameObject notReadyObj;

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

    [Header("Current MINIGAME")]
    public Transform minigameParentTrans;
    public MinigameHolder minigameHolder;
    public GameObject currentMinigame;
    public int minigameState = -1; //1의 경우 전화

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //        StartDialogueManager();
        //set standingDftPos
        for (int i = 0; i < standingImg.Length; i++)
        {
            standingDftPos[i] = standingImg[i].transform.parent.localPosition;
        }
    }

    public void StartDialogueManager()
    {
        sheetData = SheetData.instance;
        inventory = uiCanvas.GetComponent<Inventory>();
        contentTrans = contentTxt.GetComponent<RectTransform>();

        crtBranch = sheetData.FindBranchIndex(sheetBranch);


        if (crtBranch >= 0)
        {
            // ChangeBackground();
            ChangeDialogue();
        }

        hasUnderletter = UnderLetter.HasUnderLetter(myName);
        notReadyObj.SetActive(false);
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

            if (content.Contains("{"))
            {
                // Debug.Log("치환 전: " + content);

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\{(.*?)\}");
                content = regex.Replace(content, match =>
                {
                    string key = match.Groups[1].Value.Trim(); // 변수명 추출 및 공백 제거
                                                               // Debug.Log($"치환 시도: {key}");

                    // PlayerPrefs에서 값 가져오기
                    string value = PlayerPrefs.GetString(key, "null");
                    // Debug.Log($"PlayerPrefs에서 가져온 값: {value}");

                    // inventory에서 찾기
                    string item = inventory.FindItemName(value);
                    // Debug.Log($"inventory에서 찾은 값: {item}");

                    if (item != "null")
                    {
                        value = item;
                    }

                    return value;
                });

                // Debug.Log("치환 후: " + content);
            }


            typeWriter.StartTyping(content, 0, !sheetData.storyBlock[crtBranch].block[crtPage].name.Equals(""));//(int)currentBlock.block[index].name_ch

            if (isShowThinking)
            {
                sheetData.storyBlock[crtBranch].block[crtPage].thinking = ReplaceEnjolrasName(sheetData.storyBlock[crtBranch].block[crtPage].thinking);
                typeWriter.ThinkingOn(sheetData.storyBlock[crtBranch].block[crtPage].thinking);
            }
            else typeWriter.ThinkingOff();
            //contentTxt.text = currentBlock.block[index].content;

            dirManager.ChangeBackground(sheetData.storyBlock[crtBranch].block[crtPage].background);
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
        canClickToNext = true;
        isNoNext = false;
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
                PlayerPrefs.Save();
                Debug.Log("변수 저장됨: " + spl[0] + ": " + PlayerPrefs.GetString(spl[0], "null"));
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

                SetChoice(cmdStr[1]);
                break;
            case "MOVECMD"://movecmd

                for (int i = 0; i < int.Parse(cmdStr[1]); i++)
                {
                    string cmd = sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].start_cmd;

                    if (cmd.Contains("=="))
                    {
                        string[] split = cmd.Split("==");
                        Debug.Log("값 비교 : " + split[0] + ", " + PlayerPrefs.GetString(split[0], "") + "==?" + (split[1]));
                        if (PlayerPrefs.GetString(split[0], "null").Equals(split[1]))
                        {
                            sheetData.storyBlock[crtBranch].block[crtPage].move = sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move;
                            Debug.Log("변경된 무브:" + sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            RunCMD(sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].after_cmd);//Move보다 앞에 있어야 버그 안남
                            MoveBranchHold(sheetData.storyBlock[crtBranch].block[crtPage].move, -1);
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
                            sheetData.storyBlock[crtBranch].block[crtPage].move = sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move;
                            // MoveBranchHold(SheetData.instance.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            RunCMD(sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].after_cmd);
                            Debug.Log("변경된 무브:" + sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            MoveBranchHold(sheetData.storyBlock[crtBranch].block[crtPage].move, -1);
                            //crtPage--;
                            break;
                        }
                    }
                    else Debug.LogError("오류 발생! 변수 못 찾음!");

                }

                break;
            case "startdelay":
                startDelaySecond = float.Parse(cmdStr[1]);
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
                inventory.AddItem(cmdStr[1]);
                Debug.Log("아이템 획득: " + CMD[1]);
                break;
            case "remove":
                //item remove
                inventory.RemoveItem(cmdStr[1]);
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

    Vector3[] standingDftPos = new Vector3[3];

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
            //string[] spr1 = standing[0].Split('_');
            //if (spr1.Length <= 1)
            //    spr1 = new string[] { "", "" };
            //Sprite spr = sprManager.FindSprite(spr1[0], spr1[1]);

            string[] spl = standing[i].Split('_');
            if (spl.Length <= 1)
                spl = new string[] { "", "" };

            Sprite spr = sprManager.FindSprite(spl[0], spl[1]);

            standingImg[i].sprite = spr;
            standingImg[i].color = (focusOn == i) ? Color.white : gray;
            standingImg[i].enabled = (spr != null && !isMiniOn);

            if (minigameState.Equals(1) && i.Equals(1))
            {
                currentMinigame.GetComponent<Minigame_Phone>().ChangeStandingImg(spr, focusOn == 1);
                standingImg[i].enabled = false;
            }

        }

        //2명만 있는 경우 중간으로 이동시키기
        if (standingImg[1].enabled && standingImg[0].enabled && !standingImg[2].enabled)
        {
            standingImg[1].transform.parent.localPosition = new Vector3(3.2f, standingImg[1].transform.parent.localPosition.y, standingImg[1].transform.parent.localPosition.z);
            standingImg[0].transform.parent.localPosition = new Vector3(-3.2f, standingImg[0].transform.parent.localPosition.y, standingImg[0].transform.parent.localPosition.z);

        }
        else if (standingImg[1].enabled && standingImg[2].enabled && !standingImg[0].enabled)
        {
            standingImg[2].transform.parent.localPosition = new Vector3(3.2f, standingImg[2].transform.parent.localPosition.y, standingImg[2].transform.parent.localPosition.z);
            standingImg[1].transform.parent.localPosition = new Vector3(-3.2f, standingImg[1].transform.parent.localPosition.y, standingImg[1].transform.parent.localPosition.z);

        }
        else
        {
            //compare with default position
            for (int i = 0; i < standingImg.Length; i++)
            {
                if (standingImg[i].enabled)
                {
                    if (standingImg[i].transform.parent.localPosition != standingDftPos[i])
                    {
                        standingImg[i].transform.parent.localPosition = standingDftPos[i];
                    }
                }
            }
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

    void SetChoice(string count)
    {

        int choiceCount = int.Parse(count);

        for (int i = 0; i < choices.Count; i++)
        {
            if (i < choiceCount)
            {
                if (sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].start_cmd.Equals(""))
                {

                    SetChoice(i);
                }
                else
                {//변수가 있는 경우 true일때만 setactive
                    string condition = sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].start_cmd;

                    if (condition.Contains("=="))
                    {
                        string[] spl = condition.Split("==");
                        if (PlayerPrefs.GetString(spl[0], "").Equals(spl[1]))
                        {
                            SetChoice(i);
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
                            SetChoice(i);
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

        isNoNext = true;
        canClickToNext = false;

    }

    void SetChoice(int index)
    {
        int page = crtPage + (index + 1);
        choices[index].gameObject.SetActive(true);
        string[] ch = new string[]{
                    sheetData.storyBlock[crtBranch].block[page].name,
                    sheetData.storyBlock[crtBranch].block[page].content,
                    sheetData.storyBlock[crtBranch].block[page].after_cmd,
                    sheetData.storyBlock[crtBranch].block[page].move
                };
        choices[index].SetChoice(ch[0], ch[1], ch[2], ch[3]);
    }

    //선택지 클릭 시
    public void OnButtonClick(int choice)
    {
        startDelaySecond = 1.0f;
        // Debug.Log("Choice의 after_cmd: " + choices[choice].choice_after_cmd);
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
        //Debug.Log("Play again");
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
        crtPage = page;
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
        else if (branch.Contains("END"))
        {

            isNoNext = true;
            StartCoroutine(WaitUntilEnd(branch));

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
                currentMinigame.SetActive(false);
                currentMinigame.SetActive(true);
                dialogWhole.SetTrigger("OFF");
                StartCoroutine(DisableObj(dialogWhole.gameObject, 1.5f));
                canClickToNext = false;
                isNoNext = true;

            }
            else if (spl[1].Contains("PHONE:"))
            {
                currentMinigame.GetComponent<Minigame_Phone>().ChangeState(int.Parse(spl[1].Replace("PHONE:", "")));

                canClickToNext = false;
                isNoNext = true;

            }
            else
            {
                StartMinigame(spl[1]);
            }
        }
        return false;
    }

    public void StartMinigame(string name)
    {
        DestroyChildInMinigameParent();
        StartCoroutine(WaitUntilMinigame(name));
        Debug.Log("미니게임 시작. 인스턴스 생성.");
    }

    public void EndMinigame()
    {
        Debug.Log("미니게임 끝. 인스턴스 삭제.");
        if (currentMinigame != null)
        {
            DestroyChildInMinigameParent();
        }

        currentMinigame = null;
        DestroyChildInMinigameParent();
        minigameState = 0;
    }

    public TMP_Text tmptext;

    IEnumerator WaitUntilMinigame(string name)
    {
        yield return new WaitUntil(() => !typeWriter.isTyping);
        yield return new WaitForSeconds(0.5f);


        currentMinigame = Instantiate(minigameHolder.FindMinigame(name));
        currentMinigame.transform.SetParent(minigameParentTrans);
        if (!currentMinigame.activeInHierarchy) currentMinigame.SetActive(true);
        dialogWhole.SetTrigger("OFF");
        StartCoroutine(DisableObj(dialogWhole.gameObject, 0.25f));
        if (currentMinigame.GetComponent<Minigame_Phone>())
        {
            minigameState = 1;
        }


        canClickToNext = false;
        isNoNext = true;

    }

    IEnumerator WaitUntilEnd(string branch)
    {
        yield return new WaitUntil(() => !typeWriter.isTyping);
        yield return new WaitForSeconds(1f);
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

        //챕터 종료창 설정
        string[] spl = branch.Split('_');
        if (spl.Length == 1)
        {
            EpisodeEnd.nextChapter = -1;
            endObj.SetActive(true);
        }
        else
        {
            string[] nums = spl[1].Split('-');
            EpisodeEnd.nextChapter = int.Parse(nums[0]);
            EpisodeEnd.nextEpisodeBranch = int.Parse(nums[1]);
            endObj.SetActive(true);
        }
    }

    public void DestroyChildInMinigameParent()
    {
        foreach (Transform child in minigameParentTrans)
        {
            Destroy(child.gameObject);
        }
    }

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
