// DialogueUIManager.cs
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text contentText;
    public GameObject dialogueBox;
    public TypeWriter typeWriter;
    
    public bool isNoNext;
    bool canScroll = true;
    
   string myName="앙졸라스";


    [Header("Middle Text")] public GameObject middleWhole;
    public TMP_Text middleText;
    
    public void TextBoxMiddle(bool isOn, string text="")
    {
        middleText.text = text;
        middleWhole.SetActive(isOn);
     //   dialogueBox.SetActive(!isOn);
    }
    
    public void DisplayLine(Block line)
    {
        nameText.text = line.name;
        typeWriter.StartTyping(line.content);
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
    }
    
    private float coolDownTimer = 0f;
    public float scrollCooldown = 0.1f;  // 쿨다운 시간 (초)

    
    void Update()
    {
        if (!DialogueMaster.canClickToNext) return;
        if (DialogueMaster.isNoNext) return;

        bool inputDetected = false;

        if (Input.GetMouseButtonDown(0)) inputDetected = true;
        else if (Input.GetKeyDown(KeyCode.Space)) inputDetected = true;
        else if (Input.GetKeyDown(KeyCode.Return)) inputDetected = true;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) inputDetected = true;

        if (canScroll && Input.mouseScrollDelta.y != 0.0f)
        {
            inputDetected = true;
            canScroll = false;
            coolDownTimer = 0;
        }

        if (inputDetected)
        {
            if (typeWriter.isTyping)
            {
                typeWriter.StopTyping();
            }
            else
            {
                DialogueMaster.Instance.ContinueDialogue();
            }
        }

        // 🔁 스크롤 쿨타임 회복 처리
        if (!canScroll)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= scrollCooldown)
            {
                canScroll = true;
            }
        }
    }

    
    public void ChangeCharacterName(string name)
    {
        
        //GRANTAIRE, ENJOLRAS, X, COMBEFERRE, JOLY, COURFEYRAC, LAMARQUE
        switch (name)
        {
            case "GRANTAIRE"://GRANTAIRE
            case "그랑테르":
                {
                    nameText.color = new Color32(185, 222, 125, 255);
                }
                break;
            case "ENJOLRAS":
            case "앙졸라스": //ENJOLRAS
            {
                    name = myName;//플레이어 이름
                    nameText.color = new Color32(255, 235, 122, 255);
                    //isAllGrey = false;
                }break;
            case "COMBEFERRE": //COMBEFERRE
            case "콩브페르":
            {
                nameText.color = new Color32(150, 232, 253, 255);
                
            }
                break;
            case "JOLY": //JOLY
            case "졸리":
            {
                nameText.color = new Color32(223, 111, 59, 255);
            }
                break;
            case "COURFEYRAC": //COURFEYRAC
            case "쿠르페락":
            {
                nameText.color =
                    new Color32(1, 200, 178, 255);
            }
                break;
            case "LAMARQUE": //LAMARQUE
            case "라마르크 교수":
            {
                nameText.color = new Color32(235, 173, 228, 255);
            }
                break;
            case "MUSICHETTA":
            case "뮈지세타":
            case "카페 주인":
            {
                nameText.color = Color.white;
            }
                break;
            case "BAHOREL":
            case "바오렐":
                {
                    nameText.color = new Color32(186, 38, 1, 255);
                }
                break;
            case "JEHAN":
            case "장 프루베르":
                {
                    nameText.color = new Color32(245, 152, 152, 255);
                }
                break;
            case "FEUILLY":
            case "푀이":
                {
                    nameText.color = new Color32(148, 169, 193, 255);
                }
                break;
            case "BOSSUET":
            case "보쉬에":
                {
                    nameText.color = new Color32(168, 131, 114, 255);
                }
                break;

            //186 38 1 �ٿ���
            //245 152 152 ���
            //148 169 193 ǣ��
            //168 131 114 ������
            default:
                // name_ch = name;
                nameText.color = new Color32(194, 194, 194, 255);
                break;
        }

        nameText.text = name;
    }


    
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


    
}
