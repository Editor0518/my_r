using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Minigame_SpeechAttackManager : MonoBehaviour
{
    /// <summary>
    /// ���� Ŭ����
    ///    ���� ���� ����, ���� ���� ��ȯ, ������(��ų) ��ȯ

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
        Persuasion, //������ ��ħ
        Calm, //ħ���� �µ�
        Cheer, //û���� ȯȣ
        Resolve, //������ ����
        Message, //�޽����� �︲
        Firm //��ȣ�� �߾�
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

    #region ��ų ������ ����
    /// <summary>
    ///| **������ ��ħ** | ª�� �ð� ���� ���� ������ ��� �ӵ� ���� | �����ڰ� �����ϴ� ������ ǥ�� |
    ///| **ħ���� �µ�** | 3�ʰ� ��� ���� �ӵ� ���� | ħ���� �µ��� ������ �� ȿ�������� ����ٴ� �ǹ� |
    ///|   ��ȣ�� �߾�   | �Ǿ� ���� ���� ��� ���� | ���� �������� ���� ��� ����
    ///| **û���� ȯȣ** | ���� �������� ��� ������ ��� | û���� ���������� �����ϴ� ��� |
    ///| **������ ����** | ��� ���� ���°� �� | �����ڰ� �ڽŰ��� ��� ���ؿ� ��鸮�� �ʴ� ���� |
    ///|  �޽����� �︲  | ���� �ð� ���� ��� û���� �������� ������ ������ | ������ ������ ���� û���� ��鸮�� ���� |
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

            crtSkillType = (SkillType)UnityEngine.Random.Range(1, 6);//���� 7�����ε�, 6���� �ؼ� ���뱺�� ���� �ȶ߰��ϱ�.
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
                crtInvItemText.text = "������ ��ħ";
                StartCoroutine(SkillPersuasion());
                break;

            case SkillType.Calm:
                crtInvItemText.text = "ħ���� �µ�";
                StartCoroutine(SkillCalm());
                break;

            case SkillType.Firm:
                crtInvItemText.text = "��ȣ�� �߾�";
                SkillFirm();
                break;

            case SkillType.Cheer:
                crtInvItemText.text = "û���� ȯȣ";
                SkillCheer();
                break;

            case SkillType.Resolve:
                crtInvItemText.text = "������ ����";
                StartCoroutine(SkillResolve());
                break;

            case SkillType.Message:
                crtInvItemText.text = "�޽����� �︲";
                SkillMessage();
                break;
        }

    }

    IEnumerator SkillPersuasion()
    {
        //���� �ð� ���� ������ ��� �ӵ� ����
        mCrowd.crowdBoost = 2f;

        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        yield return new WaitForSeconds(5f);
        mCrowd.crowdBoost = 1f;
        crtInvItemText.text = "";
    }

    IEnumerator SkillCalm()
    {//ħ���� �µ�, 5�ʰ� ��� ���� �ӵ� ����
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        yield return new WaitForSeconds(5f);
        crtInvItemText.text = "";
    }

    void SkillFirm()
    {//��ȣ�� �߾�, �Ǿ� ���� ���� ��� ����
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 1);
        mCrowd.RemoveHostileInLine();
        crtInvItemText.text = "";
    }
    void SkillMessage()
    {//�޽����� �︲, �ǽ� ������ ���� ��������
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 1);
        mCrowd.ChangeDoubt2Support();
        crtInvItemText.text = "";
    }

    IEnumerator SkillCheer()
    {//û���� ȯȣ, ������ ��� ��� --> �Ҳ� ������ ���� ������� ����
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
    {//������ ����(����)
        speechManager.isResolved = true;
        player.ShowItemUsing(skillTypeSpr[(int)crtSkillType], 5);
        yield return new WaitForSeconds(5f);
        speechManager.isResolved = false;
        crtInvItemText.text = "";
    }



    #endregion


    #region ���� ����

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
        //isDirX 0:x��, 1:y��
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
