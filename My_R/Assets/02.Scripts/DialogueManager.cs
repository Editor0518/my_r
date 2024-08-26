using System.Collections;
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
    public LightManager lightManager;
    public DirectingManager dirManager;
    public StandingSpriteManager sprManager;
    public SpriteRenderer standingImg_left;
    public SpriteRenderer standingImg_center;
    public SpriteRenderer standingImg_right;


    [Header("Choice")]
    public GameObject choiceWhole;
    public ChoiceSelect choiceA;
    public ChoiceSelect choiceB;
    public ChoiceSelect choiceC;

    [Header("Storyline")]
    public StoryBlock currentBlock;
    public int index = 0;
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
        inventory = uiCanvas.GetComponent<Inventory>();
        contentTrans = contentTxt.GetComponent<RectTransform>();
        if (thisMode.Equals(state.MAIN))
        {
            if (currentBlock != null)
            {
                ChangeBackground();
                ChangeDialogue();
            }
        }
        hasUnderletter = UnderLetter.HasUnderLetter(myName);
    }


    void ChangeCharacterName()
    {
        if (currentBlock.block[index].start_cmd.Contains("name_"))
        {
            string[] cmd = currentBlock.block[index].start_cmd.Split(';');
            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i].Contains("name_"))
                {
                    string[] cmdStr = cmd[i].Split('_');
                    isAllGrey = true;
                    ChangeNameTxtColor(cmdStr[1]);
                    return;
                }
            }

        }
        string name = "";
        if (currentBlock.block[index].isNoName) { nameTxt.text = ""; return; }

        switch (currentBlock.block[index].focusOn)
        {
            case 0:
                name = currentBlock.block[index].left_name_ch.ToString();
                break;
            case 1:
                name = currentBlock.block[index].center_name_ch.ToString();
                break;
            case 2:
                name = currentBlock.block[index].right_name_ch.ToString();
                break;
        }


        ChangeNameTxtColor(name);
    }

    void ChangeNameTxtColor(string name)
    {

        string name_ch = "";

        //GRANTAIRE, ENJOLRAS, X, COMBEFERRE, JOLY, COURFEYRAC, LAMARQUE
        switch (name)
        {
            case "GRANTAIRE"://GRANTAIRE
                {
                    name_ch = "그랑테르";
                    nameTxt.color = new Color32(185, 222, 125, 255);
                }
                break;
            case "ENJOLRAS":
            case "앙졸라스"://ENJOLRAS
                {
                    name_ch = myName;//앙졸라스
                    nameTxt.color = new Color32(255, 235, 122, 255);
                    isAllGrey = false;
                }
                break;
            case "X":
            case ""://X
                {
                    name_ch = "";
                }
                break;
            case "COMBEFERRE"://COMBEFERRE
                {
                    name_ch = "콩브페르";
                    nameTxt.color = new Color32(150, 232, 253, 255);
                }
                break;
            case "JOLY"://JOLY
                {
                    name_ch = "졸리";
                    nameTxt.color = new Color32(223, 111, 59, 255);
                }
                break;
            case "COURFEYRAC"://COURFEYRAC
                {
                    name_ch = "쿠르페락";
                    nameTxt.color = new Color32(1, 200, 178, 255);
                }
                break;
            case "LAMARQUE"://LAMARQUE
                {
                    name_ch = "라마르크 교수";
                    nameTxt.color = new Color32(235, 173, 228, 255);
                }
                break;
            case "MUSICHETTA":
                {
                    name_ch = "뮈지세타";
                    nameTxt.color = Color.white;
                }
                break;
            case "BAHOREL":
                {
                    name_ch = "바오렐";
                    nameTxt.color = new Color32(186, 38, 1, 255);
                }
                break;
            case "JEHAN":
                {
                    name_ch = "장 프루베르";
                    nameTxt.color = new Color32(245, 152, 152, 255);
                }
                break;
            case "FEUILLY":
                {
                    name_ch = "푀이";
                    nameTxt.color = new Color32(148, 169, 193, 255);
                }
                break;
            case "BOSSUET":
                {
                    name_ch = "보쉬에";
                    nameTxt.color = new Color32(168, 131, 114, 255);
                }
                break;

            //186 38 1 바오렐
            //245 152 152 즈앙
            //148 169 193 푀이
            //168 131 114 보쉬에
            default:
                name_ch = name;
                nameTxt.color = new Color32(194, 194, 194, 255);
                break;
        }


        if (currentBlock.block[index].isNameUnkown) name_ch = "???";
        nameTxt.text = name_ch;
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
        if (currentBlock == null) return;

        if (index == currentBlock.block.Count && currentBlock.ifEnd.Equals(if_end.CHOICE)) canClickToNext = false;
        if (currentBlock.bgm != null)
        {
            if (SoundManager.instance != null) SoundManager.instance.PlayBGM(currentBlock.bgm, currentBlock.bgmSubtitle);
            else soundManager.PlayBGM(currentBlock.bgm, currentBlock.bgmSubtitle);
            //if (currentBlock.se != null) SoundManager.instance.PlaySE(currentBlock.se);
        }

        if (currentBlock.startDelaySecond > 0)
        {
            dialogWhole.gameObject.SetActive(false);
            StartCoroutine(Delay(currentBlock.startDelaySecond));
            return;
        }
        else
        {

            if (!dialogWhole.gameObject.activeInHierarchy)
                dialogWhole.gameObject.SetActive(true);


        }


        if (index >= currentBlock.block.Count)
        {
            if (currentBlock.disableObj != null)
            {
                if (currentBlock.disableObj.GetComponent<Animator>())
                {
                    currentBlock.disableObj.GetComponent<Animator>().SetTrigger("OFF");
                    StartCoroutine(DisableObj(currentBlock.disableObj, 0.3f));
                }
                else currentBlock.disableObj.SetActive(false);
            }


            Debug.Log("꽉참");
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
                case "MOVECMD"://movecmd
                    {
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
                    }
                    break;
                case "END"://end
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
                    break;

            }

            if (thisMode.Equals(state.ITEM))
            {
                currentBlock = null;
                isNoNext = true;
            }

            return;
        }
        // else
        {
            if (typeWriter.isTyping)
            {
                typeWriter.StopTyping();

            }
            else
            {
                if (currentBlock.block[index].se != null)
                {
                    SoundManager.instance.PlaySound(currentBlock.block[index].se, currentBlock.block[index].seSubtitle);
                }
                //if(영어버전)string content =content_ENG
                currentBlock.block[index].content = ReplaceEnjolrasName(currentBlock.block[index].content);

                string content = currentBlock.block[index].content;

                //폰트 변경
                contentTxt.font = fontManager.GetFont(currentBlock.block[index].text_font.ToString());
                contentTxt.fontSize = fontManager.GetFontSize(currentBlock.block[index].text_font.ToString());
                contentTxt.lineSpacing = fontManager.GetLineSpacing(currentBlock.block[index].text_font.ToString());
                contentTxt.characterSpacing = fontManager.GetCharacterSpacing(currentBlock.block[index].text_font.ToString());
                contentTxt.wordSpacing = fontManager.GetWordSpacing(currentBlock.block[index].text_font.ToString());
                contentTrans.anchoredPosition = new Vector2(contentTrans.anchoredPosition.x, -145 + fontManager.GetAddPosY(currentBlock.block[index].text_font.ToString()));

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

                typeWriter.StartTyping(content, 0, !currentBlock.block[index].isNoName);//(int)currentBlock.block[index].name_ch

                if (isShowThinking)
                {
                    currentBlock.block[index].thinkingContent = ReplaceEnjolrasName(currentBlock.block[index].thinkingContent);
                    typeWriter.ThinkingOn(currentBlock.block[index].thinkingContent);
                }
                else typeWriter.ThinkingOff();
                //contentTxt.text = currentBlock.block[index].content;

                ChangeCharacterName();

                isAllGrey = false;
                RunCMD(currentBlock.block[index].start_cmd);//ChangeSprite보다 뒤에있으면 버그남!!!!
                ChangeSprite();
                index += 1;
                // if (index >= currentBlock.block.Count) isNoNext = true;
                //else isNoNext = false;
            }
        }
    }

    //미니게임 등에서 버튼 클릭 시 다음 다이얼로그 넘길때 쓰기.
    public void ChangeCurrentBlock(StoryBlock newBlock)
    {
        index = 0;
        isNoNext = false;

        currentBlock = newBlock;

        //reset and restart
        canClickToNext = true;

        ChangeBackground();

        ChangeDialogue();
    }

    void ChangeBackground()
    {
        if (currentBlock.background != null)
        {
            if (!backgroundRender.sprite.Equals(currentBlock.background))
            {
                backgroundRender.sprite = currentBlock.background;

            }
        }

        if (currentBlock.volumeObj != null)
        {
            if (currentBlock.volumeObj != currentVolumeObj)
            {
                if (currentVolumeObj != null) currentVolumeObj.gameObject.SetActive(false);
                currentVolumeObj = currentBlock.volumeObj;
                currentVolumeObj.gameObject.SetActive(true);
            }
        }
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

            case "startdelay":
                currentBlock.startDelaySecond = float.Parse(cmdStr[1]);
                Debug.Log(currentBlock.startDelaySecond);
                break;

            case "minigameobj":
                currentBlock.minigameObj.SetActive(true);
                break;

            case "light":
                lightManager.SetLightGroupActive(cmdStr[1]);
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
        if (index < 1) return;
        index -= 1;
        RunCMD(currentBlock.block[index].after_cmd);
        index += 1;
    }

    #endregion

    void DisableSprite()
    {
        standingImg_left.enabled = (false);
        standingImg_center.enabled = (false);
        standingImg_right.enabled = (false);

    }

    void ChangeSprite()
    {
        int focusOn = isAllGrey ? -1 : currentBlock.block[index].focusOn;

        bool isNoName = currentBlock.block[index].isNoName;
        Color gray = new Color32(163, 164, 168, 255);
        // Debug.Log("focusOn: " + focusOn);
        //StandingSpriteManager에서 스프라이트를 찾아옴. (캐 이름, 얼굴 종류 파라미터로 필요.)
        Sprite spr = sprManager.FindSprite(currentBlock.block[index].left_name_ch.ToString(), currentBlock.block[index].left_face_ch);//.ToString()
        standingImg_left.sprite = spr;
        standingImg_left.color = (focusOn == 0 && !isNoName) ? Color.white : gray;
        standingImg_left.enabled = (spr != null && !isMiniOn);


        spr = sprManager.FindSprite(currentBlock.block[index].center_name_ch.ToString(), currentBlock.block[index].center_face_ch);
        standingImg_center.sprite = spr;
        standingImg_center.color = (focusOn == 1 && !isNoName) ? Color.white : gray;
        standingImg_center.enabled = (spr != null && !isMiniOn);


        spr = sprManager.FindSprite(currentBlock.block[index].right_name_ch.ToString(), currentBlock.block[index].right_face_ch);
        standingImg_right.sprite = spr;
        standingImg_right.color = (focusOn == 2 && !isNoName) ? Color.white : gray;
        standingImg_right.enabled = (spr != null && !isMiniOn);



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
        canClickToNext = false;
        choiceWhole.SetActive(false);
        choiceWhole.SetActive(true);
        choiceB.gameObject.SetActive(currentBlock.choiceB.moveTo != null);
        choiceC.gameObject.SetActive(currentBlock.choiceC.moveTo != null);

        choiceA.SetChoice(currentBlock.choiceA.choiceName, currentBlock.choiceA.clip);
        choiceB.SetChoice(currentBlock.choiceB.choiceName, currentBlock.choiceB.clip);
        choiceC.SetChoice(currentBlock.choiceC.choiceName, currentBlock.choiceC.clip);
    }

    public void OnButtonClick(int choice)
    {
        isNoNext = false;
        index = 0;
        switch (choice)
        {
            case 0:
                {
                    RunCMD_One(currentBlock.choiceA.choiceCmdAfter);
                    currentBlock = currentBlock.choiceA.moveTo;
                }
                break;
            case 1:
                {
                    RunCMD_One(currentBlock.choiceB.choiceCmdAfter);
                    currentBlock = currentBlock.choiceB.moveTo;
                }
                break;
            case 2:
                {
                    RunCMD_One(currentBlock.choiceC.choiceCmdAfter);
                    currentBlock = currentBlock.choiceC.moveTo;
                }
                break;
            case 3:
                {
                    RunCMD_One(currentBlock.choiceD.choiceCmdAfter);
                    currentBlock = currentBlock.choiceD.moveTo;
                }
                break;
        }
        if (currentBlock == null) { Debug.Log("다음블럭 없음"); return; }
        currentBlock.startDelaySecond = 1f;
        canClickToNext = true;
        ChangeDialogue();
    }

    IEnumerator DisableObj(GameObject obj, float seconds)
    {
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
        if ((canClickToNext && !isNoNext) &&
            (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.mousePosition.y <= FIXED_HEIGHT && Input.GetMouseButtonDown(0)) ||
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
