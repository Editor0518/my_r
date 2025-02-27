using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_Phone : MonoBehaviour
{
    public int currentState = -1;
    [Space]
    public float waitSecToGetCall = 1f;//상대 캐릭터가 전화 받을때까지 유저가 대기해야 되는 시간
    public AudioClip soundBeforeEndCall; //통화 끊기전에 대기해야하는 클립이 있는 경우 재생 후 종료
    public string soundBeforeEndCallSubtitle;



    [Header("branch")]
    public int whenAcceptCall = 0;//전화 받기 버튼 클릭한 경우 자동 이동할 분기
    public int whenStartCall = 0; //상대 캐릭터가 전화 받은 후 자동 이동할 분기
    public int afterThisEnd = 0; //전화 끊어진 뒤 이동할 곳

    [Header("Object")]
    public GameObject phoneBlack;
    public Image standingImg;
    public TMP_Text callingText;
    public Button callStartButton;
    public Button callEndButton;
    [Space]
    public AudioClip seEndCall;
    public AudioClip seStartCall;
    public AudioSource audioSource;

    private void OnEnable()
    {
        standingImg.color = Color.black;
        phoneBlack.SetActive(false);
        DialogueManager.instance.dirManager.BackgroundBlur();
        ChangeState(currentState);
    }

    Color gray = new Color32(163, 164, 168, 255);


    public void ChangeStandingImg(Sprite sprite, bool focusOn)
    {
        standingImg.sprite = sprite;
        standingImg.color = (focusOn) ? Color.white : gray;
    }

    public void ChangeState(int state)
    {
        currentState = state;
        StateSetting();
    }

    public void StateSetting()
    {
        switch (currentState)
        {
            case -1://아무것도 하지않음. 대기
                break;
            case 0://전화 직접 걸음, 전화 연결 중
                CallStartState();
                break;
            case 1://전화 받음, 통화 중/프로필 표정 바뀜
                CallTalkState();
                break;
            case 2://전화 끊어짐
                CallEndState();
                break;
            case 3: //전화 걸려왔음, 받을지 말지 선택 가능
                CallHoldState();
                break;

        }
    }

    //0단계, 전화 직접 걸은 상태
    public void CallStartState()
    {
        StartCoroutine(CCallStartState());

    }

    IEnumerator CCallStartState()
    {
        callStartButton.gameObject.SetActive(false);
        callEndButton.gameObject.SetActive(true);
        callEndButton.interactable = false;
        callingText.color = Color.white;

        WaitForSeconds wait = new WaitForSeconds(0.5f);
        float timer = 0f;
        int numOfPoint = 0;

        audioSource.loop = true;
        audioSource.clip = seStartCall;
        audioSource.Play();

        while (timer < waitSecToGetCall)
        {


            timer += 0.5f;

            if (numOfPoint == 1)
                callingText.text = "전화를 거는 중..";
            else if (numOfPoint == 2)
                callingText.text = "전화를 거는 중...";
            else
                callingText.text = "전화를 거는 중.";

            numOfPoint++;

            if (numOfPoint == 3)
                numOfPoint = 0;

            yield return wait;
        }

        ChangeState(1);
    }

    //1단계 전화받음, 통화중
    public void CallTalkState()
    {
        audioSource.Stop();
        callingText.text = "";
        callStartButton.gameObject.SetActive(false);
        callEndButton.gameObject.SetActive(true);
        callEndButton.interactable = false;
        DialogueManager.instance.ChangeCurrentBlock(whenStartCall);
    }



    //전화 끊어짐
    public void CallEndState()
    {
        StartCoroutine(CCallEndState());
    }

    IEnumerator CCallEndState()
    {
        //yield return WaitUntil !SoundManager.instance.soundAudio.isPlaying;
        //wait until soundmanager's soundAudio isPlaying false
        if (soundBeforeEndCall != null)
        {
            SoundManager.instance.PlaySound(soundBeforeEndCall, soundBeforeEndCallSubtitle);
            yield return new WaitUntil(() => SoundManager.instance.soundAudio.isPlaying == false);
        }
        audioSource.loop = false;
        audioSource.clip = seEndCall;
        audioSource.Play();
        callStartButton.gameObject.SetActive(false);
        callEndButton.gameObject.SetActive(false);

        callingText.color = new Color32(231, 64, 64, 255);

        callingText.text = "통화 종료.";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "통화 종료.";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "통화 종료.";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "통화 종료.";
        yield return new WaitForSeconds(0.5f);
        phoneBlack.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        DialogueManager.instance.dirManager.BackgroundReset();
        DialogueManager.instance.ChangeCurrentBlock(afterThisEnd);
        DialogueManager.instance.EndMinigame();

    }


    //전화 걸려옴, 받을지말지
    public void CallHoldState()
    {
        callStartButton.gameObject.SetActive(true);
        callEndButton.gameObject.SetActive(true);
        callEndButton.interactable = false;
        callStartButton.interactable = true;
    }

}
