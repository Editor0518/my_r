using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipePanel : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;

    public RectTransform swipe;
    float swipeYDown = -1625f;
    float swipeYUp = 1475f;
    public TMP_Text text;

/// <summary>
/// 휴대폰 위 스와이프 기능. 빠르게 움직이면 다 내려오고, 
/// 느리게 화면의 절반 or 조금을 아래로 끌어내리면 내려옴. 다른 경우에는 내려오다가 다시 위로 올라가 제자리.
/// </summary>

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            text.text = "moving " + Input.GetTouch(0).position.y;
            swipe.anchoredPosition = new Vector2(swipe.anchoredPosition.x, Input.GetTouch(0).position.y-2950/2);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endPos = Input.GetTouch(0).position;
            if (endPos.y < startPos.y)
            {
                text.text = "down";
                swipe.anchoredPosition = new Vector2(swipe.anchoredPosition.x, swipeYDown);
            }
            else { 
                text.text = ("up");
                swipe.anchoredPosition = new Vector2(swipe.anchoredPosition.x, swipeYUp);
            }
        }
    }
}
