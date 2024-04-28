using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class FixedRatio : MonoBehaviour
{
    private void Awake()
    {
        setupCamera();
    }

    /// <summary>
    /// 화면 종횡비를 고정한다
    /// 출처: blog.naver.com/fnzlz/221069086916
    /// </summary>
    private void setupCamera()
    {
        //가로 화면 비율
        float targetWidthAspect = 9.0f;

        //세로 화면 비율
        float targetHeightAspect = 16.0f;

        //메인 카메라
        Camera mainCamera = Camera.main;

        mainCamera.aspect = targetWidthAspect / targetHeightAspect;

        float widthRatio = (float)Screen.width / targetWidthAspect;
        float heightRatio = (float)Screen.height / targetHeightAspect;

        float heightadd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        float widthtadd = ((heightRatio / (widthRatio / 100)) - 100) / 200;

        // 16_10비율보다 가로가 짦다면(4_3 비율)
        // 16_10비율보다 세로가 짧다면(16_9 비율)
        // 시작 지점을 0으로 만들어준다
        if (heightRatio > widthRatio)
            widthtadd = 0.0f;
        else
            heightadd = 0.0f;


        mainCamera.rect = new Rect(
            mainCamera.rect.x + Abs(widthtadd),
            mainCamera.rect.y + Abs(heightadd),
            mainCamera.rect.width + (widthtadd * 2),
            mainCamera.rect.height + (heightadd * 2));
    }

}
