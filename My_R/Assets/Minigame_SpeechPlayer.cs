using System.Collections;
using UnityEngine;

public class Minigame_SpeechPlayer : MonoBehaviour
{
    Transform player;
    Vector3 lastPosition;
    float idleTime = 0f;
    public float idleThreshold = 0.3f;
    public float rotationAmount = 5f;
    public float rotationSpeed = 1500f;
    bool isIdle = false;
    float rotationDirection = 1f;
    public Minigame_SpeechAttackManager sam;
    public Minigame_Crowd mgCrowd;
    public BoxCollider2D playerCol;
    public SpriteRenderer itemSpr;

    private void Start()
    {
        player = transform;
        lastPosition = player.position;
        isIdle = true;
    }

    void Update()
    {
        if (!Minigame_SpeechManager.isPlaying) return;
        // 마우스 위치를 월드 좌표로 변환 (플레이어의 z 위치 사용)
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.WorldToScreenPoint(player.position).z));

        // 가장 가까운 crowdHolder 위치 찾기
        Vector3 nearestPos = FindNearestHolderPos(point);

        // 플레이어를 해당 위치로 이동
        player.position = Vector3.MoveTowards(player.position, nearestPos, 0.2f);
    }

    Vector3 FindNearestHolderPos(Vector3 point)
    {
        float minDistance = float.MaxValue;
        Vector3 nearestPos = player.position;  // 기본값: 현재 위치
        int nearestIndex = -1;

        for (int i = 0; i < sam.tiles.Count; i++)
        {
            float distance = Vector3.Distance(point, sam.tiles[i].tileTrans.position);

            if (distance < minDistance)  // 가장 가까운 위치 찾기
            {
                minDistance = distance;
                nearestPos = sam.tiles[i].tileTrans.position;
                nearestIndex = i;
            }
        }

        // 가장 가까운 인덱스를 업데이트
        if (nearestIndex != -1)
        {
            mgCrowd.currentPlayerIndex = nearestIndex;
        }

        return nearestPos;
    }

    public void ChangePlayerCollider(bool isTrue)
    {
        playerCol.enabled = isTrue;
    }

    public void ShowItemUsing(Sprite spr, float time)
    {
        itemSpr.sprite = spr;
        itemSpr.enabled = true;

        StartCoroutine(IEShowItemUsing(time));
    }
    IEnumerator IEShowItemUsing(float time)
    {
        yield return new WaitForSeconds(time);
        itemSpr.enabled = false;
    }
}
