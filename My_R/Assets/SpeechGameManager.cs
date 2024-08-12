using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechGameManager : MonoBehaviour
{
    public SpeechSchedule speechSchedule;

    public SpeechBubble speechEnj;
    public List<SpeechBubble> speechQuestion;


    public int currentSpeechIndex = 0;
    public string currentSpeech = "";
    public int currentTextIndex = 0;

    public string currentSpeechQuestion = "";
    public int currentTextIndexQuestion = 0;


    [Header("Heart Bar")]
    public Image heartBarFill;
    public float rise = 0.1f;
    public float fall = 0.02f;

    SpeechMode mode;


    private void Start()
    {
        ShowNextSpeech();
    }

    public void ShowNextSpeech()
    {
        if (currentSpeechIndex >= speechSchedule.speechList.Count) { return; }
        mode = speechSchedule.speechList[currentSpeechIndex].mode;
        //버블 세팅하기
        if (speechSchedule.speechList[currentSpeechIndex].mode.Equals(SpeechMode.TALK))
        {
            currentSpeech = speechSchedule.speechList[currentSpeechIndex].speech;
            currentTextIndex = 0;
            speechEnj.SetSpeech(currentSpeech);
        }

        heartBarFill.fillAmount += rise;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//스페이스바 1, 0
        {
            Clicked('1');
        }
        if (Input.GetMouseButtonDown(0))//왼쪽 마우스 8
        {
            Clicked('8');
        }
        if (Input.GetMouseButtonDown(1))//오른쪽 마우스 9
        {
            Clicked('9');
        }
    }

    void Clicked(char number)
    {
        if (mode.Equals(SpeechMode.TALK))
        {
            if (currentSpeech[currentTextIndex] == number)//성공
            {
                speechEnj.SucessBubble(currentTextIndex);

            }
            else//실패
            {
                speechEnj.FailedBubble(currentTextIndex);
                heartBarFill.fillAmount -= fall;
            }


            if (currentSpeechIndex + 1 < speechSchedule.speechList.Count
                && speechSchedule.speechList[currentSpeechIndex + 1].mode.Equals(SpeechMode.QUESTION)
                && currentTextIndex == speechSchedule.speechList[currentSpeechIndex + 1].startIndex)
            {
                Debug.Log("index");
                currentSpeechIndex++;
                currentSpeechQuestion = speechSchedule.speechList[currentSpeechIndex].speech;
                currentTextIndexQuestion = 0;
                speechEnj.PauseBubble(currentTextIndex, currentSpeech);
                speechQuestion[0].gameObject.SetActive(true);
                speechQuestion[0].SetSpeech(currentSpeechQuestion);
                mode = speechSchedule.speechList[currentSpeechIndex].mode;
            }

            currentTextIndex++;



            if (currentTextIndex >= currentSpeech.Length)
            {
                currentSpeechIndex++;
                ShowNextSpeech();
            }


        }
        else
        {
            if (currentSpeechQuestion[currentTextIndexQuestion] == number)//성공
            {
                speechQuestion[0].SucessBubble(currentTextIndexQuestion);

            }
            else//실패
            {
                speechQuestion[0].FailedBubble(currentTextIndexQuestion);
                heartBarFill.fillAmount -= fall;
            }
            currentTextIndexQuestion++;

            if (currentTextIndexQuestion >= currentSpeechQuestion.Length)
            {
                speechSchedule.speechList.RemoveAt(currentSpeechIndex);
                currentSpeechIndex--;
                speechEnj.RestartBubble();
                speechQuestion[0].gameObject.SetActive(false);
                mode = speechSchedule.speechList[currentSpeechIndex].mode;
            }
        }
    }


}
