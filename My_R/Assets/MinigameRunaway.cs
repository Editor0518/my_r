using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MinigameRunaway : MonoBehaviour
{
    
    
    [Header("Minigame Runaway")]
    public DirectingManager dirManager;
    public MinigameRunChoiceData choiceData;
    public ScreenShaker screenShaker;

    public Animator buttonsWhole;
    public Button leftButton;
    public Button middleButton;
    public Button rightButton;
    public Button turnBackButton;

    [Header("timer")] public Image timerImg;
    private float timer;
    private bool isFirstTimer = true;
    private bool isSelected = false;
    public Image noTouchPanel;

    private int chapter=2;
    private int branch;
    int branchRunRealFast=12; //(17 이하 / 후, 이 정도면 충분히 멀어졌겠지?)
    int branchRunFast=13; //(25 이하 / 이 정도면... 충분히 멀어졌겠지?)
    int branchRunNormal=14; //(지침부터)
    int branchRunSlow=14; //(지침부터)

    [Header("message text")] public RectTransform messageRect;
    Animator messageAnimator;
    public TMP_Text messageText;

    private HashSet<int> visited = new HashSet<int>();
    private HashSet<(int from, int to)> visitedEdges = new();

    private int currentNode;
    private int fromNode;
    private Dictionary<Button, int> remainingClicks = new();

    public int debugCurrentNode;

    public AudioClip runningClip;

    private int runCount = 0;
    private bool isHeartbeatPlayed = false;
    public AudioClip heartbeatSe;
    public string bgm;


    private readonly Dictionary<(int from, int to), string> specialMessages = new()
    {
        //from -> to, msg
        { (2, 3), "<color=yellow><b>왼쪽</b></color> 길에 다리가 있네. 건너는 게 좋겠어." },
        { (16, 3), "<color=yellow><b>가운데</b></color> 길에 다리가 있네. 건너는 게 좋겠어." },
        { (12, 3), "<color=yellow><b>오른쪽</b></color> 길에 다리가 있네. 건너는 게 좋겠어." },

        { (12, 11), "<color=yellow><b>왼쪽</b></color> 길에 다리가 있네. 건너는 게 좋겠어." },
        { (20, 11), "<color=yellow><b>가운데</b></color> 길에 다리가 있네. 건너는 게 좋겠어." },
        

        { (10, 11), "아까 이쪽 다리를 건너온 것 같은데... <color=yellow><b>잘못 온 것 같아.</b></color>" },
        { (4, 3), "아까 이쪽 다리를 건너온 것 같은데... <color=yellow><b>잘못 온 것 같아.</b></color>" },
        { (11, 10), "조금만 더 멀어지면 될 것 같아. <color=yellow><b>마지막으로 뛰자!</b></color>" },
        { (9, 10), "조금만 더 멀어지면 될 것 같아. <color=yellow><b>마지막으로 뛰자!</b></color>" },
        { (1, 0), "여긴... 아까 도망쳤던 광장이잖아! <color=yellow><b>다시 돌아가자!</b></color>" },
        { (2, 1), "안돼! 출발했던 곳으로 돌아와버렸어! <color=yellow><b>다시 돌아가자!</b></color>" },
        { (21, 1), "안돼! 출발했던 곳으로 돌아와버렸어! <color=yellow><b>다시 돌아가자!</b></color>" },

        { (15, 21), "직진하면 출발했던 곳이니까 <color=yellow><b>오른쪽</b></color>으로 가보는 게 좋겠어." },
        { (22, 9), "아무래도 최대한 멀어지려면 <color=yellow><b>왼쪽</b></color>으로 가는 게 낫겠지." },
        
        { (14, 13), "여긴 길이 복잡해서, 잘못 고르면 뱅뱅 돌 수도 있겠어. 센 강이 <color=yellow><b>직진</b></color>이었던가..." },
        { (19, 13), "여긴 길이 복잡해서, 잘못 고르면 뱅뱅 돌 수도 있겠어. 센 강이 <color=yellow><b>오른쪽</b></color>이었던가..." },
        { (12, 13), "여긴 <color=yellow><b>갈림길</b></color>이네. 길이 복잡해서, 잘못 고르면 뱅뱅 돌 수도 있겠어." },
        
        { (20, 19), "왠지 길을 잘못 든 것 같은데... <color=yellow><b>어느 방향</b></color>으로 가야하지?" },
       
        //
    };

    Coroutine timerCoroutine;

    void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timerImg.fillAmount = 1f;
        isSelected = false;
        timerCoroutine = StartCoroutine(Timer());
    }


    IEnumerator EnableButtonsWithDelay(float delay)
    {
        buttonsWhole.SetTrigger("on");
        yield return new WaitForSeconds(delay);

        if (leftButton.gameObject.activeSelf)
            SetButtonInteractable(leftButton, true);
        if (middleButton.gameObject.activeSelf)
            SetButtonInteractable(middleButton, true);
        if (rightButton.gameObject.activeSelf)
            SetButtonInteractable(rightButton, true);
    }


    IEnumerator Timer()
    {
        float duration = isFirstTimer?10f:5f;
        timer = duration;
        isFirstTimer = false;

        while (timer > 0f && !isSelected)
        {
            timer -= Time.deltaTime;
            timerImg.fillAmount = timer / duration;
            yield return null;
        }

        if (!isSelected)
        {
            // ⏱️ 시간이 다 되면 무작위 버튼 클릭
            List<Button> activeButtons = new();
            if (leftButton.gameObject.activeSelf) activeButtons.Add(leftButton);
            if (middleButton.gameObject.activeSelf) activeButtons.Add(middleButton);
            if (rightButton.gameObject.activeSelf) activeButtons.Add(rightButton);

            if (activeButtons.Count > 0)
            {
                Button randomButton = activeButtons[UnityEngine.Random.Range(0, activeButtons.Count)];
                StartCoroutine(HighlightAndClick(randomButton));
            }
        }
    }

    IEnumerator HighlightAndClick(Button button)
    {
        isSelected = true;

        // 시각적으로 강조하기: 예시로 배경 색을 노란색으로 잠시 바꾸자
        var originalColorBlock = button.colors;
        var highlightColorBlock = button.colors;
        highlightColorBlock.normalColor = Color.yellow;
        highlightColorBlock.highlightedColor = Color.yellow;
        button.colors = highlightColorBlock;
        noTouchPanel.enabled = true;

        yield return new WaitForSeconds(1f); // 1초간 하이라이트

        noTouchPanel.enabled = false;
        button.colors = originalColorBlock; // 색상 복원

        // 버튼 클릭 호출 (리스너 강제 실행)
        button.onClick?.Invoke();
    }


    private void Start()
    {
      if(dirManager==null) dirManager = DialogueMaster.Instance.dirManager;
        screenShaker=dirManager.backgroundRender.GetComponent<ScreenShaker>();
        messageAnimator = messageRect.GetComponent<Animator>();
        SetupGame();
    }

    public void SetupGame()
    {
        isHeartbeatPlayed = false;
        SoundManager.instance.StopAmbience();
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlayBGM(bgm);
        //방문들 초기화
        visited.Clear();
        visitedEdges.Clear();
        
        runCount = 0;
        currentNode = 1;
        fromNode = 0;
        remainingClicks.Clear();
        SetMessageText("<color=yellow><b>도망쳐야 해!</b></color> 어디로 가지?");
        UpdateChoices(currentNode, fromNode);
    }

    void SetMessageText(string text)
    {
        float delta = 190f;
        messageText.text = text;
        messageRect.sizeDelta = new Vector2(messageText.preferredWidth + delta, messageRect.sizeDelta.y);
        messageAnimator.SetTrigger("on");
    }

    public void UpdateChoices(int current, int from)
    {
        StartTimer();
        SoundManager.instance.PlaySound("running 3");
        currentNode = current;
        fromNode = from;

        
        dirManager.ChangeBackground(choiceData.GetBackground(current), "");

        debugCurrentNode = currentNode;

        var choices = choiceData.GetChoices(current, from);
        bool hasVisited = false;

        if (choices == null || choices.Count == 0)
        {
            leftButton.gameObject.SetActive(false);
            middleButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
            turnBackButton.gameObject.SetActive(true);
            if (specialMessages.TryGetValue((from, current), out string specialMsg))
            {
                SetMessageText(specialMsg);
            }
            else
                SetMessageText("헉! <color=yellow><b>막다른 길</b></color>이잖아!");

            turnBackButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            Debug.Log("막다른 길:" + currentNode);
            return;
        }

        turnBackButton.gameObject.SetActive(false);

        // 🔻 버튼은 일단 모두 비활성화 상태로 세팅
        SetButtonInteractable(leftButton, false);
        SetButtonInteractable(middleButton, false);
        SetButtonInteractable(rightButton, false);

        SetButton(leftButton, choices[0]);
        int nodeIdMid = choices.Count >= 2 ? choices[1] : -1;
        int nodeIdRight = choices.Count >= 3 ? choices[2] : -1;

        SetButton(middleButton, nodeIdMid);
        SetButton(rightButton, nodeIdRight);

        int available = 0;
        if (choices[0] != -1) available++;
        if (choices.Count >= 2 && choices[1] != -1) available++;
        if (choices.Count >= 3 && choices[2] != -1) available++;

        if (choices.Count == 3)
            hasVisited = visited.Contains(choices[0]) || visited.Contains(choices[1]) || visited.Contains(choices[2]);
        else if (choices.Count == 2)
            hasVisited = visited.Contains(choices[0]) || visited.Contains(choices[1]);
        else
            hasVisited = visited.Contains(choices[0]);

        runCount++;

        if (!isHeartbeatPlayed && runCount > 25)
        {
            SoundManager.instance.PlayAmbience(heartbeatSe, "");
            isHeartbeatPlayed = true;
        }

        if (specialMessages.TryGetValue((from, current), out string specialMessage))
        {
            SetMessageText(specialMessage);
        }
        else if (available == 1)
        {
            string[] msgs = new[]
            {
                "길이 하나뿐이네. <color=yellow><b>뛰자!</b></color>",
                "여긴 다른 선택지가 없어. <color=yellow><b>뛰는 수밖에!</b></color>",
                "이쪽밖에 없어. <color=yellow><b>달리자!</b></color>",
                "<color=yellow><b>직진만 가능</b></color>하네. 고민할 시간 없어.",
                "뒤 돌아볼 시간 없어! <color=yellow><b>빨리 가자!</b></color>",
                "여기선 <color=yellow><b>멈출 수 없어!</b></color> 계속 가자!",
            };
            int index=UnityEngine.Random.Range(0, msgs.Length);

            SetMessageText(msgs[index]);
        }
        else if (hasVisited)
        {
            string[] msgs = new[]
            {
                "저쪽은 아까 <color=yellow><b>왔던 길</b></color> 같다. 어디로 갈까?",
                "<color=yellow><b>여기 와봤던가?</b></color> 낯이 익다. 어디로 갈까?",
                "저쪽은 아까 <color=yellow><b>왔던 길</b></color>이네. 어디로 가지?",
                "저쪽은 <color=yellow><b>방금 지나온 길</b></color> 같기도 하고…"
            };
            int index=UnityEngine.Random.Range(0, msgs.Length);
            
            SetMessageText(msgs[index]);
        }
        else
        {
            string[] msgs = new[]
            {
                "<color=yellow><b>갈림길</b></color>이다. 어디로 갈까?",
                "<color=yellow><b>갈림길</b></color>이 나왔네. 잘 선택해야겠어.",
                "<color=yellow><b>갈림길</b></color>이네. 어느 쪽으로 가지?",
                "길이 <color=yellow><b>여러 갈래</b></color>로 나있다. 어디로 갈까?",
                
            };
            int index=UnityEngine.Random.Range(0, msgs.Length);
            SetMessageText(msgs[index]);
        }

        // ✅ 0.5초 후 버튼들 활성화
        StartCoroutine(EnableButtonsWithDelay(0.5f));
    }

    void SetButtonInteractable(Button button, bool interactable)
    {
        button.interactable = interactable;

        for (int i = 0; i < button.transform.childCount; i++)
        {
            button.transform.GetChild(i).gameObject.SetActive(interactable);
        }
    }
    
    Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out var color))
            return color;
        return Color.white;
    }

    void SetButton(Button button, int nodeId)
    {
        if (nodeId == -1)
        {
            button.gameObject.SetActive(false);
            return;
        }

        button.gameObject.SetActive(true);
        SetButtonInteractable(button, true);

        int weight = choiceData.GetEdgeWeight(currentNode, nodeId);

        if (!remainingClicks.ContainsKey(button))
            remainingClicks[button] = weight;

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = $"{nodeId} ({remainingClicks[button]})";
        
        bool edgeVisited = visitedEdges.Contains((currentNode, nodeId));

        if (edgeVisited)
        {
            SetButtonInteractable(button, true);
            var colors = button.colors;
            Color visitedColor=HexToColor("#282E6F");//짙은 남색
            colors.normalColor = visitedColor;
            colors.highlightedColor = visitedColor;
            buttonText.color = Color.gray;
            button.colors = colors;
        }
        else
        {
            var colors = button.colors;
            colors.normalColor = Color.black;
            colors.highlightedColor = Color.black;
            buttonText.color = colors.normalColor;
            button.colors = colors;
        }


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            screenShaker?.Shake(); // ✅ 탁탁 흔들기!

            remainingClicks[button]--;

            if (remainingClicks[button] > 0)
            {
                StartTimer();
                buttonText.text = $"{nodeId} ({remainingClicks[button]})";

                // 모든 버튼 잠금, 해당 버튼만 활성
                SetButtonInteractable(leftButton, false);
                SetButtonInteractable(middleButton, false);
                SetButtonInteractable(rightButton, false);
                SetButtonInteractable(button, true);
                SoundManager.instance.PlaySound("running 3");
                runCount++;

                if (currentNode == 10)
                    SetMessageText($"조금만 더 가면 돼. <color=yellow><b>마지막으로 뛰자!</b></color> {remainingClicks[button]}");
                else
                    SetMessageText($"좀 거리가 있네. <color=yellow><b>뛰자!</b></color> {remainingClicks[button]}");
            }
            else
            {
                remainingClicks.Remove(button);
                MoveTo(nodeId);
            }
        });
    }


    void MoveTo(int newNode)
    {
        Debug.Log($"Moved from {currentNode} to {newNode}");
        debugCurrentNode = newNode;

        visited.Add(currentNode);
        visitedEdges.Add((currentNode, newNode));

        if (newNode == 23) // fin 노드 번호가 23이라고 가정
        {
            SuccessGame();
            return;
        }


        UpdateChoices(newNode, currentNode);
    }

    public void TurnBackClick()
    {
        UpdateChoices(fromNode, currentNode);
    }

    void SuccessGame()
    {
        string msg;
        if (runCount <= 17) //0~17
        {
            //도전과제 달성
            //안 지침, 빠름
            msg = "후, 이 정도면 충분히 멀어졌겠지?";
           

            branch = branchRunRealFast;
        }
        else if (runCount <= 25) //18~25
        {
            //덜 지침, 빠름
            msg = "이 정도면... 충분히 멀어졌겠지?";
            branch = branchRunFast;
        }
        else if (runCount <= 50) //26~50
        {
            //지침, 빠름
            msg = "헉헉... 이 정도면... 충분히 멀어졌겠지?";
            branch = branchRunNormal;
        }
        else //51개 이상
        {
            //빙빙 돌았음. 매우 지침.
            msg = "허윽! 죽을 것 같아! 이, 이 정도면... 충분히 멀어졌겠지?";
            branch = branchRunSlow;
        }

        isGameEnd = true;
        nextButton.SetActive(true);
        SoundManager.instance.EndBGM();
        timerImg.gameObject.SetActive(false);
        dirManager.ChangeBackground(gameEndBackground, "");

        SetMessageText(msg + runCount);
        leftButton.gameObject.SetActive(false);
        middleButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        turnBackButton.gameObject.SetActive(false);
    }


    [Header("When Game End")] 
    public Sprite gameEndBackground;
    public GameObject nextButton;
    private bool isGameEnd = false; // keyboard용
    public Image nextButtonFill;

    private Coroutine holdCoroutine;
    private float holdTime = 2f;

// 호출: 버튼을 누르기 시작할 때
    public void EndGameHoldStart()
    {
        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);

        holdCoroutine = StartCoroutine(HoldToEndGame());
    }

// 호출: 버튼에서 손 뗄 때
    public void EndGameHoldEnd()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        nextButtonFill.fillAmount = 0f;
    }

    private IEnumerator HoldToEndGame()
    {
        float timer = 0f;
        nextButtonFill.fillAmount = 0f;

        while (timer < holdTime)
        {
            timer += Time.deltaTime;
            nextButtonFill.fillAmount = timer / holdTime;
            yield return null;
        }

        nextButtonFill.fillAmount = 1f;

        // 이곳에서 실제 다음 동작 (예: 씬 이동 등)을 호출할 수 있습니다.
        MoveToNextScene();
    }

    void MoveToNextScene()
    {
        //다음꺼 진행
        SoundManager.instance.StopAmbience();
        Debug.Log("다음꺼 진행");
        DialogueMaster.Instance.EndMinigame();
        DialogueMaster.Instance.MoveBranch(chapter, branch);
       // this.gameObject.SetActive(false);
    }
}