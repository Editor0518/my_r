using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Minigame_SpeechAttackManager : MonoBehaviour
{
    /// <summary>
    /// 어택 클래스
    ///    공격 패턴 제작, 공격 랜덤 소환, 아이템(스킬) 소환

    /// </summary>


    public Minigame_SpeechManager speechManager;
    public Minigame_Crowd mCrowd;
    public Minigame_SpeechPlayer player;

    public List<Minigame_Tile> tiles;
    public int sizeOfTile;

    [Header("Item")]
    public SpriteRenderer item;
    public TMP_Text crtInvItemText;
    public SkillType crtSkillType;
    public Sprite[] skillTypeSpr = new Sprite[5];


    public enum SkillType
    {
        None,
        Persuasion, //설득의 외침
        Calm, //침착한 태도
        Cheer, //청중의 환호
        Resolve, //결의의 순간
        Message, //메시지의 울림
        Firm //단호한 발언
    }


    void Start()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].attack.gameObject.SetActive(false);
            tiles[i].attackPrev.SetActive(false);
        }
        StartCoroutine(Attack());
        StartCoroutine(Item());
    }

    #region 스킬 아이템 관련
    /// <summary>
    ///| **설득의 외침** | 짧은 시간 동안 군중 게이지 상승 속도 증가 | 연설자가 강조하는 순간을 표현 |
    ///| **침착한 태도** | 3초간 모든 공격 속도 감소 | 침착한 태도가 연설을 더 효과적으로 만든다는 의미 |
    ///|   단호한 발언   | 맨앞 적대 군중 즉시 제거 | 강한 주장으로 방해 요소 차단
    ///| **청중의 환호** | 군중 게이지가 즉시 일정량 상승 | 청중이 열광적으로 반응하는 장면 |
    ///| **결의의 순간** | 잠시 무적 상태가 됨 | 연설자가 자신감을 얻고 방해에 흔들리지 않는 느낌 |
    ///|  메시지의 울림  | 일정 시간 동안 모든 청중의 게이지가 서서히 차오름 | 연설의 감동이 퍼져 청중이 흔들리지 않음 |
    /// </summary>
    /// <returns></returns>

    IEnumerator Item()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(18f, 15f));
            int x = UnityEngine.Random.Range(0, sizeOfTile);
            int y = UnityEngine.Random.Range(0, sizeOfTile);

            int index = sizeOfTile * y + x;

            crtSkillType = (SkillType)UnityEngine.Random.Range(1, 6);//원래 7까지인데, 6으로 해서 적대군중 관련 안뜨게하기.
            item.sprite = skillTypeSpr[(int)crtSkillType];

            item.transform.position = tiles[index].tileTrans.position;
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(6f);
            item.gameObject.SetActive(false);
        }
    }

    public void UseSkillItem()
    {
        switch (crtSkillType)
        {
            case SkillType.Persuasion:
                crtInvItemText.text = "설득의 외침";
                StartCoroutine(SkillPersuasion());
                break;

            case SkillType.Calm:
                crtInvItemText.text = "침착한 태도";
                StartCoroutine(SkillCalm());
                break;

            case SkillType.Firm:
                crtInvItemText.text = "단호한 발언";
                SkillFirm();
                break;

            case SkillType.Cheer:
                crtInvItemText.text = "청중의 환호";
                SkillCheer();
                break;

            case SkillType.Resolve:
                crtInvItemText.text = "결의의 순간";
                StartCoroutine(SkillResolve());
                break;

            case SkillType.Message:
                crtInvItemText.text = "메시지의 울림";
                SkillMessage();
                break;
        }

    }

    IEnumerator SkillPersuasion()
    {
        //일정 시간 군중 게이지 상승 속도 증가
        mCrowd.crowdBoost = 2f;

        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        yield return new WaitForSeconds(5f);
        mCrowd.crowdBoost = 1f;
        crtInvItemText.text = "";
    }

    IEnumerator SkillCalm()
    {//침착한 태도, 5초간 모든 공격 속도 감소
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        yield return new WaitForSeconds(5f);
        crtInvItemText.text = "";
    }

    void SkillFirm()
    {//단호한 발언, 맨앞 적대 군중 즉시 제거
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 1);
        mCrowd.RemoveHostileInLine();
        crtInvItemText.text = "";
    }
    void SkillMessage()
    {//메시지의 울림, 의심 군중을 지지 군중으로
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 1);
        mCrowd.ChangeDoubt2Support();
        crtInvItemText.text = "";
    }

    IEnumerator SkillCheer()
    {//청중의 환호, 게이지 즉시 상승 --> 불꽃 게이지 직접 상승으로 수정
        float time = 0;
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        while (time < 5f)
        {
            /*for (int i = 0; i < mCrowd.crowdHolders.Count; i++)
            {
                mCrowd.CrowdGaugeChange(i, 0.05f, true);
            }*/
            speechManager.GaugeChange(6);
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        crtInvItemText.text = "";
    }

    IEnumerator SkillResolve()
    {//결의의 순간(무적)
        speechManager.isResolved = true;
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        yield return new WaitForSeconds(5f);
        speechManager.isResolved = false;
        crtInvItemText.text = "";
    }



    #endregion


    #region 공격 관련

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.2f * 1.5f, 2.2f * 1.5f));
            int x = UnityEngine.Random.Range(0, sizeOfTile);
            int y = UnityEngine.Random.Range(0, sizeOfTile);

            int index = sizeOfTile * y + x;

            CreateAttackPattern2(index, UnityEngine.Random.Range(0, 2));
        }
    }


    void CreateAttackPattern2(int startIndex, int isDirX)
    {
        //0 1 2
        //3 4 5
        //6 7 8 

        startIndex = (int)((float)startIndex / sizeOfTile);
        //isDirX 0:x축, 1:y축
        StartCoroutine(IECreateAttackPattern2(startIndex, isDirX));
    }

    IEnumerator IECreateAttackPattern2(int startIndex, int isDirX)
    {
        CreateAttack(startIndex);
        yield return new WaitForSeconds(0.3f);
        CreateAttack(startIndex + (isDirX.Equals(0) ? 1 : sizeOfTile));
    }

    void CreateAttack(int index)
    {
        StartCoroutine(IECreateAttack(index));
    }
    IEnumerator IECreateAttack(int index)
    {
        tiles[index].attackPrev.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tiles[index].attack.gameObject.SetActive(true);
        tiles[index].attackPrev.SetActive(false);
        yield return new WaitForSeconds(1f);
        tiles[index].attack.gameObject.SetActive(false);
    }
    #endregion
}
