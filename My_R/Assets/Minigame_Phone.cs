using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_Phone : MonoBehaviour
{
    public int currentState = -1;
    [Space]
    public float waitSecToGetCall = 1f;//��� ĳ���Ͱ� ��ȭ ���������� ������ ����ؾ� �Ǵ� �ð�
    public AudioClip soundBeforeEndCall; //��ȭ �������� ����ؾ��ϴ� Ŭ���� �ִ� ��� ��� �� ����
    public string soundBeforeEndCallSubtitle;



    [Header("branch")]
    public int whenAcceptCall = 0;//��ȭ �ޱ� ��ư Ŭ���� ��� �ڵ� �̵��� �б�
    public int whenStartCall = 0; //��� ĳ���Ͱ� ��ȭ ���� �� �ڵ� �̵��� �б�
    public int afterThisEnd = 0; //��ȭ ������ �� �̵��� ��

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
        //DialogueManager.instance.dirManager.BackgroundBlur();
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
            case -1://�ƹ��͵� ��������. ���
                break;
            case 0://��ȭ ���� ����, ��ȭ ���� ��
                CallStartState();
                break;
            case 1://��ȭ ����, ��ȭ ��/������ ǥ�� �ٲ�
                CallTalkState();
                break;
            case 2://��ȭ ������
                CallEndState();
                break;
            case 3: //��ȭ �ɷ�����, ������ ���� ���� ����
                CallHoldState();
                break;

        }
    }

    //0�ܰ�, ��ȭ ���� ���� ����
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
                callingText.text = "��ȭ�� �Ŵ� ��..";
            else if (numOfPoint == 2)
                callingText.text = "��ȭ�� �Ŵ� ��...";
            else
                callingText.text = "��ȭ�� �Ŵ� ��.";

            numOfPoint++;

            if (numOfPoint == 3)
                numOfPoint = 0;

            yield return wait;
        }

        ChangeState(1);
    }

    //1�ܰ� ��ȭ����, ��ȭ��
    public void CallTalkState()
    {
        audioSource.Stop();
        callingText.text = "";
        callStartButton.gameObject.SetActive(false);
        callEndButton.gameObject.SetActive(true);
        callEndButton.interactable = false;
        //DialogueManager.instance.ChangeCurrentBlock(whenStartCall);
    }



    //��ȭ ������
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

        callingText.text = "��ȭ ����.";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "��ȭ ����.";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "��ȭ ����.";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "";
        yield return new WaitForSeconds(0.5f);
        callingText.text = "��ȭ ����.";
        yield return new WaitForSeconds(0.5f);
        phoneBlack.SetActive(true);

        yield return new WaitForSeconds(1.5f);

       // DialogueManager.instance.dirManager.BackgroundReset();
       // DialogueManager.instance.ChangeCurrentBlock(afterThisEnd);
        //DialogueManager.instance.EndMinigame();

    }


    //��ȭ �ɷ���, ����������
    public void CallHoldState()
    {
        callStartButton.gameObject.SetActive(true);
        callEndButton.gameObject.SetActive(true);
        callEndButton.interactable = false;
        callStartButton.interactable = true;
    }

}
