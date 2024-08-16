using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CamCloseUpObj))]
public class Minigame_Task : MonoBehaviour
{
    CamCloseUpObj camCloseUpObj;
    public DialogueManager dialogueManager;
    public StoryBlock storyBlockSuccess;
    public StoryBlock storyBlockFail;

    public MinigamePopUp minigamePopUp;

    public AudioClip bgm;
    public string bgmSubtitle;
    public int markedDone = 0;
    public int totalNeedMarked = 3;

    public bool isEnd = false;


    [Header("Timer")]
    public float timer = 30f;
    public TMP_Text timerText;
    public Image timeBar;


    private void OnEnable()
    {
        //StartGame();
    }

    private void Start()
    {
        camCloseUpObj = GetComponent<CamCloseUpObj>();
        StartGame();
    }

    public void StartGame()
    {
        SoundManager.instance.PlayBGM(bgm, bgmSubtitle);
        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        float time = timer;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        float rgbR = (1 - timeBar.color.r) / timer;
        float rgbG = (-timeBar.color.g) / timer;

        while (time > 0)
        {
            time -= 0.1f;
            timerText.text = ((int)time).ToString();
            timeBar.fillAmount = time / timer;
            //Color color = new Color(timeBar.color.r + rgbR, timeBar.color.g + (timeBar.color.r == 1 ? rgbG : 0), timeBar.color.b);
            //timeBar.color = color;
            yield return wait;
        }

        if (!isEnd) EndGameFail();
    }


    public void AddMarkedDone()
    {
        markedDone++;
        CheckFinished();
    }

    void CheckFinished()
    {
        if (markedDone >= totalNeedMarked)
        {

            StopCoroutine("Timer");
            EndGameSuccess();
            EndGame();

        }
    }

    void EndGameFail()
    {

        dialogueManager.ChangeCurrentBlock(storyBlockFail);
        EndGame();
    }

    void EndGameSuccess()
    {
        dialogueManager.ChangeCurrentBlock(storyBlockSuccess);
    }

    void EndGame()
    {
        camCloseUpObj.cam.GetComponent<Animator>().enabled = true;
        SoundManager.instance.EndBGM();
        isEnd = true;
        camCloseUpObj.CamCloseUpButton();
        if (minigamePopUp != null) minigamePopUp.ClearAllMsg();
        this.gameObject.SetActive(false);
    }

}
