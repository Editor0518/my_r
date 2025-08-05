using UnityEngine;
using UnityEngine.UI;

public class ScrollZoomMinigame : MonoBehaviour
{
    
    public float scrollValue = 0.5f;
    public float targetValue = 0.75f; // 초점이 맞는 위치
    public float blurMultiplier = 2f;
    
    public SpriteRenderer spriteRenderer;

    public Image fill;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sharedMaterial.SetFloat("_BlurDistance", 0);
    }
    
    void Update()
    {
        // 마우스 스크롤 반영
        float scrollDelta = Input.mouseScrollDelta.y * 0.002f;
        scrollValue = Mathf.Clamp(scrollValue + scrollDelta, 0.47f, 0.53f);  // 0~1 범위로 제한
        Debug.Log($"Scroll Value: {scrollValue}");
        fill.fillAmount = scrollValue;

        // 중심 초점값과 최대 blur가 적용되는 거리
        float targetValue = 0.5f;
        float blurMaxDistance = 0.04f;  // 최대 흐림일 때의 거리 = 0.04

        // 중심값(0.5)로부터 얼마나 떨어졌는지
        float distance = Mathf.Abs(scrollValue - targetValue);

        // BlurDistance는 선명할수록 0, 흐릴수록 최대 0.04가 되게 설정
        float blurDistance = Mathf.Clamp01(distance / blurMaxDistance) * blurMaxDistance;

        // SpriteRenderer에 BlurDistance 전달
        if (spriteRenderer != null)
        {
            spriteRenderer.sharedMaterial.SetFloat("_BlurDistance", blurDistance);
        }
    }





}
