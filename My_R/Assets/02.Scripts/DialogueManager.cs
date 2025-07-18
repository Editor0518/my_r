using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    public int sheetBranch = 1; //���� ����. �̰� ���°� �ƴ�. �ؿ� �ε��� ��ߵ�.
    public int crtBranch = 0;
    public int crtPage = 0;
    public bool isNoNext = false;

    [Header("When END")]
    public GameObject endObj;

    [Header("Current MINIGAME")]
    public Transform minigameParentTrans;
    public MinigameHolder minigameHolder;
    public GameObject currentMinigame;
    public int minigameState = -1; //1�� ��� ��ȭ

    [Header("Others")]
    //public Heart heart;

    public TypeWriter typeWriter;
    public bool canUseScroll = false;
    public bool isPause = false;
    public bool isViewPause = false;
    public bool canClickToNext = true;
    bool isMiniOn = false;


    //temp
    public string myName = "������";
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
            case "앙졸라스": //ENJOLRAS
            {
                    name = myName;//플레이어 이름
                    nameTxt.color = new Color32(255, 235, 122, 255);
                    //isAllGrey = false;
                }break;
            case "COMBEFERRE": //COMBEFERRE
            case "콩브페르":
            {
                nameTxt.color = new Color32(150, 232, 253, 255);
                
            }
                break;
            case "JOLY": //JOLY
            case "졸리":
            {
                nameTxt.color = new Color32(223, 111, 59, 255);
            }
                break;
            case "COURFEYRAC": //COURFEYRAC
            case "쿠르페락":
            {
                nameTxt.color =
                    new Color32(1, 200, 178, 255);
            }
                break;
            case "LAMARQUE": //LAMARQUE
            case "라마르크 교수":
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

            //186 38 1 �ٿ���
            //245 152 152 ���
            //148 169 193 ǣ��
            //168 131 114 ������
            default:
                // name_ch = name;
                nameTxt.color = new Color32(194, 194, 194, 255);
                break;
        }

        nameTxt.text = name;
    }


    //����� �̸� "������"�� ������ ���� �̸����� �ٲ�
    public string ReplaceEnjolrasName(string content)
    {
        string defaultName = "앙졸라스";
        
        if (myName.Equals(defaultName)) return content;

        // 조사 대상: '이', '가', '을', '를', '은', '는', '로', '와', '과', '으로' 등 필요한 조사 추가
        string pattern = $@"{Regex.Escape(defaultName)}(으로|와는|과는|이랑은|랑은|이라도|라도|이든|보다|[이가을를은는로와과든])";


        // 정규식 대체 함수 사용
        content = Regex.Replace(content, pattern, match =>
        {
            string josa = match.Groups[1].Value;
            return UnderLetter.SetUnderLetter(myName, josa);
        });

        // 조사 없는 순수 '앙졸라스'도 교체
        content = content.Replace(defaultName, myName);

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
                Debug.Log("����!!! ���� ���!!!!");
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
                Debug.Log("���̾�αװ� �����־ ����. �̰� �� �������� �ڵ� ���� �ϼ�.");
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
            //if(�������)string content =content_ENG
            sheetData.storyBlock[crtBranch].block[crtPage].content = ReplaceEnjolrasName(sheetData.storyBlock[crtBranch].block[crtPage].content);

            string content = sheetData.storyBlock[crtBranch].block[crtPage].content;

            //��Ʈ ����
            contentTxt.font = fontManager.GetFont(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.fontSize = fontManager.GetFontSize(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.lineSpacing = fontManager.GetLineSpacing(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.characterSpacing = fontManager.GetCharacterSpacing(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTxt.wordSpacing = fontManager.GetWordSpacing(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString());
            contentTrans.anchoredPosition = new Vector2(contentTrans.anchoredPosition.x, -145 + fontManager.GetAddPosY(sheetData.storyBlock[crtBranch].block[crtPage].font.ToString()));

            if (content.Contains("{"))
            {
                // Debug.Log("ġȯ ��: " + content);

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\{(.*?)\}");
                content = regex.Replace(content, match =>
                {
                    string key = match.Groups[1].Value.Trim(); // ������ ���� �� ���� ����
                                                               // Debug.Log($"ġȯ �õ�: {key}");

                    // PlayerPrefs���� �� ��������
                    string value = PlayerPrefs.GetString(key, "null");
                    // Debug.Log($"PlayerPrefs���� ������ ��: {value}");

                    // inventory���� ã��
                    string item = inventory.FindItemName(value);
                    // Debug.Log($"inventory���� ã�� ��: {item}");

                    if (item != "null")
                    {
                        value = item;
                    }

                    return value;
                });

                // Debug.Log("ġȯ ��: " + content);
            }

            //, 0, !sheetData.storyBlock[crtBranch].block[crtPage].name.Equals("")
            typeWriter.StartTyping(content);//(int)currentBlock.block[index].name_ch

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
            RunCMD(sheetData.storyBlock[crtBranch].block[crtPage].start_cmd);//ChangeSprite���� �ڿ������� ���׳�!!!!
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
                    //gift ���� ��ũ��Ʈ-�κ��丮or���� ��ũ��Ʈ�� �����ϱ�
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

    //�̴ϰ��� ��� ��ư Ŭ�� �� ���� ���̾�α� �ѱ涧 ����.
    public void ChangeCurrentBlock(int newBranch)
    {
        canClickToNext = true;
        isNoNext = false;
        MoveBranch(newBranch.ToString());

    }


    #region Ŀ�ǵ� ���� ���� �Լ�
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

                SetChoice(cmdStr[1]);
                break;
            case "MOVECMD"://movecmd

                for (int i = 0; i < int.Parse(cmdStr[1]); i++)
                {
                    string cmd = sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].start_cmd;

                    if (cmd.Contains("=="))
                    {
                        string[] split = cmd.Split("==");
                        Debug.Log("�� �� : " + split[0] + ", " + PlayerPrefs.GetString(split[0], "") + "==?" + (split[1]));
                        if (PlayerPrefs.GetString(split[0], "null").Equals(split[1]))
                        {
                            sheetData.storyBlock[crtBranch].block[crtPage].move = sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move;
                            Debug.Log("����� ����:" + sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            RunCMD(sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].after_cmd);//Move���� �տ� �־�� ���� �ȳ�
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
                            Debug.Log("����� ����:" + sheetData.storyBlock[crtBranch].block[crtPage + (i + 1)].move);
                            MoveBranchHold(sheetData.storyBlock[crtBranch].block[crtPage].move, -1);
                            //crtPage--;
                            break;
                        }
                    }
                    else Debug.LogError("���� �߻�! ���� �� ã��!");

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
                Debug.Log("������ ȹ��: " + CMD[1]);
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
        //StandingSpriteManager���� ��������Ʈ�� ã�ƿ�. (ĳ �̸�, �� ���� �Ķ���ͷ� �ʿ�.)


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

        //2���� �ִ� ��� �߰����� �̵���Ű��
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

        /*//��´�
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
                {//������ �ִ� ��� true�϶��� setactive
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

    //������ Ŭ�� ��
    public void OnButtonClick(int choice)
    {
        startDelaySecond = 1.0f;
        // Debug.Log("Choice�� after_cmd: " + choices[choice].choice_after_cmd);
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

    //branch ���� �� ���̾�α� �ڵ� ���
    public void MoveBranch(string branch)
    {
        if (MoveBranchHold(branch)) ChangeDialogue();
    }

    //branch ���游. ���� Ŭ���ϸ� �ٲ� ä ���̾�α� ���.
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
        Debug.Log("�̴ϰ��� ����. �ν��Ͻ� ����.");
    }

    public void EndMinigame()
    {
        Debug.Log("�̴ϰ��� ��. �ν��Ͻ� ����.");
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

        //é�� ����â ����
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
