using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour
{
    Animator animator;
    RectTransform character;

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<RectTransform>();
    }

    public void Move(RectTransform endPoint)
    {
        StopAllCoroutines(); // 이동 중복 방지
        StartCoroutine(MoveToPosition(endPoint));
    }

    IEnumerator MoveToPosition(RectTransform endPoint)
    {
        Vector3 startPos = character.anchoredPosition;
        Vector3 targetPos = endPoint.anchoredPosition + new Vector2(0, 100f); // 보정

        // 방향 판단 (왼쪽이면 flip)
        bool isGoingLeft = targetPos.x < startPos.x;
        Transform sprite = character.GetChild(0); // 자식 오브젝트 (스프라이트)

        Vector3 scale = sprite.localScale;
        scale.x = Mathf.Abs(scale.x) * (isGoingLeft ? -1 : 1);
        sprite.localScale = scale;

        animator.SetBool("isWalking", true); // 걷는 애니메이션 (있다면)

        float distance = Vector2.Distance(startPos, targetPos);
        float baseDistance = 650f;         // 기준 거리 (무난한 거리)
        float baseDuration = 1.5f;         // 기준 시간
        float duration = Mathf.Max(0.2f, (distance / baseDistance) * baseDuration);  // 거리 비례 시간

        float elapsed = 0f;
        while (elapsed < duration)
        {
            character.anchoredPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        character.anchoredPosition = targetPos;
        animator.SetBool("isWalking", false); // 걷기 종료
    }


    public void ResetScale()
    {
        Transform sprite = character.GetChild(0); // 자식 오브젝트 (스프라이트)
        Vector3 scale = sprite.localScale;
        //다시 앞에 보기
        scale.x = Mathf.Abs(scale.x);
        sprite.localScale = scale;
    }

    public void EnjolrasJump()
    {
        animator.SetTrigger("jump");
    }
}