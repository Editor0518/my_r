using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CamCloseUpObj))]
public class Minigame_Task : MonoBehaviour
{
    CamCloseUpObj camCloseUpObj;
    public int branchSuccess;
    public int branchFail;

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
        SoundManager.instance.PlayBGM(bgm, bgmSubtitle, true);
        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        float time = timer;
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (time > 0)
        {
            time -= 0.1f;
            timerText.text = ((int)time).ToString();
            timeBar.fillAmount = time / timer;
            float t = 1 - (time / timer);
            timeBar.color = Color.Lerp(Color.green, Color.yellow, Mathf.Clamp01(t * 2));
            if (t > 0.5f)
            {
                timeBar.color = Color.Lerp(Color.yellow, Color.red, Mathf.Clamp01((t - 0.5f) * 2));
            }
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

        DialogueManager.instance.ChangeCurrentBlock(branchFail);
        EndGame();
    }

    void EndGameSuccess()
    {
        DialogueManager.instance.ChangeCurrentBlock(branchSuccess);
    }

    void EndGame()
    {
        camCloseUpObj.cam.GetComponent<Animator>().enabled = true;
        SoundManager.instance.EndBGM();
        isEnd = true;
        camCloseUpObj.CamCloseUpButton();
        if (minigamePopUp != null) minigamePopUp.ClearAllMsg();
        // this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

}
