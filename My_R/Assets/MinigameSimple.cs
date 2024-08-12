using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSimple : MonoBehaviour
{
    public Minigame_Task minigame_Task;
    public CamCloseUpObj camCloseUpObj;


    public enum MinigameType
    {
        Drag,
        TimingClick,
        Hold
    }

    public MinigameType minigameType;
    public List<Image> circles;
    public bool doMarkedDone = true;
    public GameObject backPanel;

    [Header("For Drag")]
    [Description("circles는 시작부터 끝 지점을 표시하는데 쓰입니다.")]
    public RectTransform line;
    public int currentCircle = 0;

    [Header("For Timing Click && Hold")]
    [Description("TIMING: circles는 시작 크기와 타이밍 맞춰야 하는 지점을 표시하는데 쓰입니다." +
        "HOLD: circles는 시작 크기와 도착 지점을 표시하는데 쓰입니다.")]
    float circleSize = 300;
    public float needTime = 3f;

    [Description("")]


    [Header("etc")]
    public GameObject toEnable;
    public GameObject toDisable;

    bool startMove = false;
    bool isEnd = false;

    private void Start()
    {
        if (line != null) line.gameObject.SetActive(false);
        startMove = false;

        if (minigameType == MinigameType.Hold)
            ResetHold();
        if (minigameType == MinigameType.Drag)
        {
            currentCircle = 0;
        }
    }

    public void StartGame()
    {
        if (backPanel != null) backPanel.SetActive(true);
        if (minigameType == MinigameType.TimingClick) StartTimingClick();
    }

    #region Drag

    public void OnPointerEnterStart(int index)
    {
        line.gameObject.SetActive(true);
        circles[index].color = Color.red;
        LineFollowPointer();
    }

    public void OnPointerEnterEnd(int index)
    {
        if (!startMove) return;
        circles[index].color = Color.green;
        LineUnfollowPointer();
        currentCircle += 2;
        if (currentCircle >= circles.Count) EndGame();
        else DragAgain();
    }

    void DragAgain()
    {
        for (int i = 0; i < currentCircle; i++)
        {
            circles[i].gameObject.SetActive(false);
        }
        circles[currentCircle].gameObject.SetActive(true);
        circles[currentCircle + 1].gameObject.SetActive(true);
        startMove = false;
        isEnd = false;
        if (line != null) line.gameObject.SetActive(false);
    }

    void LineFollowPointer()
    {
        startMove = true;
        Input.mousePosition.Set(circles[currentCircle].transform.position.x, circles[currentCircle].transform.position.y, 0);
        line.anchoredPosition = circles[currentCircle].rectTransform.anchoredPosition;

    }


    void LineUnfollowPointer()
    {
        startMove = false;
        isEnd = true;
        line.sizeDelta = new Vector2(line.sizeDelta.x, Vector2.Distance(circles[currentCircle].transform.position, circles[currentCircle + 1].transform.position) * 100);
        Vector2 dir = new Vector2(circles[currentCircle + 1].transform.position.x - circles[currentCircle].transform.position.x, circles[currentCircle + 1].transform.position.y - circles[currentCircle].transform.position.y);
        line.up = dir;

    }


    private void Update()
    {
        if (minigameType == MinigameType.Drag)
        {
            if (!startMove || isEnd) return;
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 dir = new Vector2(mousePos.x - circles[currentCircle].transform.position.x, mousePos.y - circles[currentCircle].transform.position.y);
            line.up = dir;
            Vector2 scale = new Vector2(line.sizeDelta.x, Vector2.Distance(circles[currentCircle].transform.position, mousePos) * 100);
            Debug.Log(scale.y);
            line.sizeDelta = scale;
        }
    }

    #endregion

    #region TimingClick


    public void StartTimingClick()
    {
        circleSize = circles[0].rectTransform.sizeDelta.x;
        ResetTimingClick();
    }

    public void OnTimingClick()
    {
        isEnd = true;
        StopCoroutine("TimingClick");
        float delta = 15f;
        if (circles[0].rectTransform.sizeDelta.x <= circles[1].rectTransform.sizeDelta.x + delta && circles[0].rectTransform.sizeDelta.x >= circles[1].rectTransform.sizeDelta.x - delta)
        {
            Debug.Log("Good");
            EndGame();
        }
        else
        {
            ResetTimingClick();
        }

    }

    void ResetTimingClick()
    {
        circles[0].rectTransform.sizeDelta = new Vector2(circleSize, circleSize);
        isEnd = false;
        StopCoroutine("TimingClick");
        StartCoroutine("TimingClick");
    }

    IEnumerator TimingClick()
    {
        float timeToWait = 0.05f;
        WaitForSeconds wait = new WaitForSeconds(timeToWait);
        float start = 0.0f;
        float sizeToAdd = (circles[0].rectTransform.sizeDelta.x - circles[1].rectTransform.sizeDelta.x) / needTime * timeToWait;
        // Debug.Log(sizeToAdd);
        while (circles[0].rectTransform.sizeDelta.x > 0 && !isEnd)
        {

            start += Time.deltaTime;
            circles[0].rectTransform.sizeDelta -= new Vector2(sizeToAdd, sizeToAdd);
            yield return wait;
        }
        if (!isEnd) ResetTimingClick();
        yield return null;
    }

    #endregion

    #region Hold

    public void OnPointerDownHold()
    {
        Debug.Log("DOWN");
        if (startMove) return;
        startMove = true;
        StartCoroutine("CircleHold");
    }

    IEnumerator CircleHold()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        float time = 0;
        float sizeToAdd = circles[1].rectTransform.sizeDelta.x / needTime * 0.05f;
        while (time < needTime)
        {
            time += 0.05f;
            circles[0].rectTransform.sizeDelta += new Vector2(sizeToAdd, sizeToAdd);
            yield return wait;
        }
        if (startMove)
        {
            isEnd = true;
            EndGame();
        }
    }

    public void OnPointerUpHold()
    {
        Debug.Log("UP");
        StopCoroutine("CircleHold");
        ResetHold();
    }

    void ResetHold()
    {
        startMove = false;
        circles[0].rectTransform.sizeDelta = Vector2.zero;
    }

    #endregion

    public void EndGame()
    {

        if (doMarkedDone)
        {

            if (camCloseUpObj != null) camCloseUpObj.CamReturn();//여기에 MarkedDone이 대신 있음
            else MarkedDone();
        }
        if (backPanel != null) backPanel.SetActive(false);
        if (toDisable != null) toDisable.SetActive(false);
        if (toEnable != null) toEnable.SetActive(true);
    }

    public void MarkedDone()
    {
        minigame_Task.AddMarkedDone();
    }

}
