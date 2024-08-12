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
    /// ȭ�� ��Ⱦ�� �����Ѵ�
    /// ��ó: blog.naver.com/fnzlz/221069086916
    /// </summary>
    private void setupCamera()
    {
        //���� ȭ�� ����
        float targetWidthAspect = 9.0f;

        //���� ȭ�� ����
        float targetHeightAspect = 16.0f;

        //���� ī�޶�
        Camera mainCamera = Camera.main;

        mainCamera.aspect = targetWidthAspect / targetHeightAspect;

        float widthRatio = (float)Screen.width / targetWidthAspect;
        float heightRatio = (float)Screen.height / targetHeightAspect;

        float heightadd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        float widthtadd = ((heightRatio / (widthRatio / 100)) - 100) / 200;

        // 16_10�������� ���ΰ� �F�ٸ�(4_3 ����)
        // 16_10�������� ���ΰ� ª�ٸ�(16_9 ����)
        // ���� ������ 0���� ������ش�
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
