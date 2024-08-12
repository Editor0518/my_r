using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public enum state
    {
        MAIN, ITEM
    };

    public state thisMode;


    [Header("Dialogue")]
    public FontManager fontManager;
    public Canvas uiCanvas;
    Inventory inventory;

    public Animator dialogWhole;
    public TMP_Text nameTxt;
    public TMP_Text contentTxt;
    public RectTransform contentTrans;


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



    public static float FIXED_HEIGHT = Screen.height; //* 0.45f;
    private void Start()
    {
        inventory = uiCanvas.GetComponent<Inventory>();
        contentTrans = contentTxt.GetComponent<RectTransform>();
        if (thisMode.Equals(state.MAIN))
        {
            if (currentBlock != null)
            {
                ChangeDialogue();
            }
        }
        hasUnderletter = UnderLetter.HasUnderLetter(myName);
    }


    void ChangeCharacterName()
    {

        if (currentBlock.block[index].isNoName) { nameTxt.text = ""; return; }
        string name = "";
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

        string name_ch = "";

        //GRANTAIRE, ENJOLRAS, X, COMBEFERRE, JOLY, COURFEYRAC, LAMARQUE
        switch (name)
        {
            case "GRANTAIRE"://GRANTAIRE
                {
                    name_ch = "�׶��׸�";
                    nameTxt.color = new Color32(185, 222, 125, 255);
                }
                break;
            case "ENJOLRAS"://ENJOLRAS
                {
                    name_ch = myName;//������
                    nameTxt.color = new Color32(255, 235, 122, 255);
                }
                break;
            case "X"://X
                {
                    name_ch = "";
                }
                break;
            case "COMBEFERRE"://COMBEFERRE
                {
                    name_ch = "����丣";
                    nameTxt.color = new Color32(150, 232, 253, 255);
                }
                break;
            case "JOLY"://JOLY
                {
                    name_ch = "����";
                    nameTxt.color = new Color32(223, 111, 59, 255);
                }
                break;
            case "COURFEYRAC"://COURFEYRAC
                {
                    name_ch = "�����";
                    nameTxt.color = new Color32(1, 200, 178, 255);
                }
                break;
            case "LAMARQUE"://LAMARQUE
                {
                    name_ch = "�󸶸�ũ ����";
                    nameTxt.color = new Color32(235, 173, 228, 255);
                }
                break;
            //186 38 1 �ٿ���
            //245 152 152 ���
            //148 169 193 ǣ��
            //168 131 114 ������
            default:
                nameTxt.color = new Color32(194, 194, 194, 255);
                break;
        }

        if (currentBlock.block[index].isNameUnkown) name_ch = "???";
        nameTxt.text = name_ch;
    }


    //����� �̸� "������"�� ������ ���� �̸����� �ٲ�
    string ReplaceEnjolrasName(string content)
    {
        if (myName.Equals("������")) return content;

        //string content = currentBlock.block[index].content;
        if (content.Contains("������"))
        {
            if (content.Contains("�����󽺰�")) content = content.Replace("�����󽺰�", UnderLetter.SetUnderLetter(myName, '��'));
            if (content.Contains("�����󽺸�")) content = content.Replace("�����󽺸�", UnderLetter.SetUnderLetter(myName, '��'));
            if (content.Contains("�����󽺴�")) content = content.Replace("�����󽺴�", UnderLetter.SetUnderLetter(myName, '��'));
            if (content.Contains("�����󽺿�")) content = content.Replace("�����󽺿�", UnderLetter.SetUnderLetter(myName, '��'));
            content = content.Replace("������", myName);
            //currentBlock.block[index].content=content;
            //Debug.Log(currentBlock.block[index].content);
        }
        return content;
    }

    public void ChangeDialogue()
    {
        if (currentBlock == null) return;

        if (index == currentBlock.block.Count && currentBlock.ifEnd.Equals(if_end.CHOICE)) canClickToNext = false;
        if (currentBlock.bgm != null)
        {
            if (SoundManager.instance != null) SoundManager.instance.PlayBGM(currentBlock.bgm, currentBlock.bgmSubtitle);
            else soundManager.PlayBGM(currentBlock.bgm, currentBlock.bgmSubtitle);
            //if (currentBlock.se != null) SoundManager.instance.PlaySE(currentBlock.se);
        }
        if (currentBlock.background != null)
        {
            if (!backgroundRender.sprite.Equals(currentBlock.background))
            {
                backgroundRender.sprite = currentBlock.background;

            }
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


            Debug.Log("����");
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
                        //gift ���� ��ũ��Ʈ-�κ��丮or���� ��ũ��Ʈ�� �����ϱ�
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
                        Debug.LogError("���� �߻�! ���� �� ã��!");
                    }
                    break;
                case "END"://end
                    {
                        isNoNext = true;
                        dialogWhole.SetTrigger("OFF");
                        StartCoroutine(DisableObj(dialogWhole.gameObject, 0.25f));
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
                //if(�������)string content =content_ENG
                currentBlock.block[index].content = ReplaceEnjolrasName(currentBlock.block[index].content);

                string content = currentBlock.block[index].content;

                //��Ʈ ����
                contentTxt.font = fontManager.GetFont(currentBlock.block[index].text_font.ToString());
                contentTxt.fontSize = fontManager.GetFontSize(currentBlock.block[index].text_font.ToString());
                contentTxt.lineSpacing = fontManager.GetLineSpacing(currentBlock.block[index].text_font.ToString());
                contentTxt.characterSpacing = fontManager.GetCharacterSpacing(currentBlock.block[index].text_font.ToString());
                contentTxt.wordSpacing = fontManager.GetWordSpacing(currentBlock.block[index].text_font.ToString());
                contentTrans.anchoredPosition = new Vector2(contentTrans.anchoredPosition.x, -145 + fontManager.GetAddPosY(currentBlock.block[index].text_font.ToString()));

                //���� ġȯ {������} -> PlayerPrefs.GetString(������)
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

                RunCMD(currentBlock.block[index].start_cmd);//ChangeSprite���� �ڿ������� ���׳�!!!!
                ChangeSprite();
                index += 1;
                // if (index >= currentBlock.block.Count) isNoNext = true;
                //else isNoNext = false;
            }
        }
    }

    //�̴ϰ��� ��� ��ư Ŭ�� �� ���� ���̾�α� �ѱ涧 ����.
    public void ChangeCurrentBlock(StoryBlock newBlock)
    {
        index = 0;
        currentBlock = newBlock;

        isNoNext = false;

        //reset and restart

        ChangeDialogue();
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
                Debug.Log("������ ȹ��: " + CMD[1]);
                break;
            case "name":
                nameTxt.text = ReplaceEnjolrasName(cmdStr[1]);
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
        standingImg_left.gameObject.SetActive(false);
        standingImg_center.gameObject.SetActive(false);
        standingImg_right.gameObject.SetActive(false);

    }

    void ChangeSprite()
    {
        int focusOn = currentBlock.block[index].focusOn;
        bool isNoName = currentBlock.block[index].isNoName;
        Color gray = new Color32(163, 164, 168, 255);
        // Debug.Log("focusOn: " + focusOn);
        //StandingSpriteManager���� ��������Ʈ�� ã�ƿ�. (ĳ �̸�, �� ���� �Ķ���ͷ� �ʿ�.)
        Sprite spr = sprManager.FindSprite(currentBlock.block[index].left_name_ch.ToString(), currentBlock.block[index].left_face_ch);//.ToString()
        standingImg_left.sprite = spr;
        standingImg_left.color = (focusOn == 0 && !isNoName) ? Color.white : gray;
        standingImg_left.gameObject.SetActive(spr != null && !isMiniOn);


        spr = sprManager.FindSprite(currentBlock.block[index].center_name_ch.ToString(), currentBlock.block[index].center_face_ch);
        standingImg_center.sprite = spr;
        standingImg_center.color = (focusOn == 1 && !isNoName) ? Color.white : gray;
        standingImg_center.gameObject.SetActive(spr != null && !isMiniOn);


        spr = sprManager.FindSprite(currentBlock.block[index].right_name_ch.ToString(), currentBlock.block[index].right_face_ch);
        standingImg_right.sprite = spr;
        standingImg_right.color = (focusOn == 2 && !isNoName) ? Color.white : gray;
        standingImg_right.gameObject.SetActive(spr != null && !isMiniOn);



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
        if (currentBlock == null) { Debug.Log("������ ����"); return; }
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
#if UNITY_EDITOR
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
            if ((canClickToNext && !isNoNext)) ChangeDialogue();
        }

#endif
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
