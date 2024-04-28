using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    public Animator dialogWhole;
    public TMP_Text nameTxt;
    public TMP_Text contentTxt;

    [Header("Standing")]
    public StandingSpriteManager sprManager;
    public Image standingImg;


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

    public static float FIXED_HEIGHT = Screen.height * 0.45f;
    private void Start()
    {
        if (currentBlock != null)
        {
            ChangeDialogue();
        }
    }

    string CharacterName()
    {
        int num = ((int)currentBlock.block[index].name_ch);
        string name_ch = "???";
        if (!currentBlock.block[index].isOn) return "";
        switch (num)
        {
            case 0:
                {
                    name_ch = "그랑테르";
                }
                break;
            case 1:
                {
                    name_ch = "앙졸라스";
                }
                break;
            case 2:
                { 
                    name_ch = "";
                    break;
                }
            case 3:
                {
                    name_ch = "콩브페르";
                }
                break;
            case 4:
                {
                    name_ch = "쿠르페락";
                }
                break;
            case 5:
                {
                    name_ch = "졸리";
                }
                break;
            case 6:
                {
                    name_ch = "바오렐";
                }
                break;
            case 7:
                {
                    name_ch = "보쉬에";
                }
                break;
            case 8:
                {
                    name_ch = "에포닌";
                }
                break;
            case 9:
                {
                    name_ch = "루소";
                }
                break;
        }
        return name_ch;
    }


    public void ChangeDialogue()
    {
        if (currentBlock == null) return;

        if (currentBlock.startDelaySecond > 0 && index.Equals(0))
        {
            dialogWhole.gameObject.SetActive(false);
            StartCoroutine(Delay(currentBlock.startDelaySecond));
            return;
        }
        else
        {
            if (!dialogWhole.gameObject.activeInHierarchy) dialogWhole.gameObject.SetActive(true);
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
                case "HEART": //heart
                    {
                        //heart.HeartUp(currentBlock.heartAdd);
                        currentBlock = currentBlock.newBlock;
                        //reset and restart

                        ChangeDialogue();
                    }
                    break;
                case "NEW": //new
                    {

                        currentBlock = currentBlock.newBlock;
                        //reset and restart

                        ChangeDialogue();
                    }
                    break;
                case "MINIGAME"://minigame
                    {
                        isNoNext = true;
                        dialogWhole.SetTrigger("OFF");
                        StartCoroutine(DisableObj(dialogWhole.gameObject, 0.25f));
                    }
                    break;
            }
            return;
        }
        else {
            if (typeWriter.isTyping) {
                typeWriter.StopTyping();
               
            }
            else { 
            typeWriter.StartTyping(currentBlock.block[index].content, (int)currentBlock.block[index].name_ch, currentBlock.block[index].isOn);
            //contentTxt.text = currentBlock.block[index].content;
            
            nameTxt.text = CharacterName();
            ChangeSprite();
            index += 1;
            }
        }
    }

    void ChangeSprite(){
        //StandingSpriteManager에서 스프라이트를 찾아옴. (캐 이름, 얼굴 종류 파라미터로 필요.)
        Sprite spr = sprManager.FindSprite(currentBlock.block[index].name_ch.ToString(), currentBlock.block[index].face_ch.ToString());
        standingImg.sprite = spr;
    }
    void SetChoice(){
        choiceWhole.SetActive(false);
        choiceWhole.SetActive(true);
        choiceB.gameObject.SetActive(currentBlock.choiceB.moveTo != null);
        choiceC.gameObject.SetActive(currentBlock.choiceC.moveTo != null);

        choiceA.SetChoice(currentBlock.choiceA.choiceName, currentBlock.choiceA.clip);
        choiceB.SetChoice(currentBlock.choiceB.choiceName, currentBlock.choiceB.clip);
        choiceC.SetChoice(currentBlock.choiceC.choiceName, currentBlock.choiceC.clip);
    }

    public void OnButtonClick(int choice) {
        isNoNext = false;
        index = 0;
        switch (choice) {
            case 0:
                {
                    currentBlock = currentBlock.choiceA.moveTo;
                }
                break;
            case 1:
                {
                    currentBlock = currentBlock.choiceB.moveTo;
                }
                break;
            case 2:
                {
                    currentBlock = currentBlock.choiceC.moveTo;
                }
                break;
        }
        if (currentBlock == null) { Debug.Log("다음블럭 없음"); return; }
        currentBlock.startDelaySecond = 1f;
        ChangeDialogue();
    }

    IEnumerator DisableObj(GameObject obj, float seconds) {
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
        //Spacebar or EnterKey, or Click Between bottom 0~700
        if (!isNoNext&&(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)||(Input.mousePosition.y<= FIXED_HEIGHT && Input.GetMouseButtonDown(0))))
        {
            tmptext.text = Input.mousePosition.y.ToString();
            ChangeDialogue();
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
