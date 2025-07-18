using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public GameObject prologue;
    public TMP_Text showThinkText;

    private void OnEnable()
    {
        DialogueManager.instance.canClickToNext = false;
    }


    public List<Vector2> chapAndBranch;

    public void MoveBranch(int index)
    {
        StartCoroutine(IEMoveBranch(index));
    }

    IEnumerator IEMoveBranch(int index)
    {
        int chapter = (int)chapAndBranch[index].x;
        int branch = (int)chapAndBranch[index].y;

        if (chapter == 0)
        {
            prologue.SetActive(true);
        }
        else if (DialogueManager.instance.crtChapter == chapter)
        {
            DialogueManager.instance.ChangeCurrentBlock(branch);
            prologue.SetActive(false);
        }
        else
        {
            prologue.SetActive(false);
            SheetLoader.chapter = chapter;
            SheetLoader.isLoading = true;
            SheetLoader.instance.StartLoadSheet();
            yield return new WaitUntil(() => SheetLoader.isLoading == false);
            SoundManager.instance.StopBGM();
            DialogueManager.instance.EndMinigame();
            DialogueManager.instance.dirManager.screenEffectImg.gameObject.SetActive(false);
            DialogueManager.instance.dirManager.MiniCutDisable();
            DialogueManager.instance.ChangeCurrentBlock(branch);
        }
        this.gameObject.SetActive(false);
    }

    public void CloseTab()
    {
        DialogueManager.instance.canClickToNext = true;
        this.gameObject.SetActive(false);
    }

    public void DestroyChild()
    {
        DialogueManager.instance.DestroyChildInMinigameParent();
    }

    public void ChangeThinking()
    {
        DialogueManager.instance.isShowThinking = !DialogueManager.instance.isShowThinking;
        showThinkText.text = DialogueManager.instance.isShowThinking ? "�Ӹ��� ���� �����(1ȸ��)" : "�Ӹ��� ���� �ѱ�(2ȸ��)";
    }

    public void ResetAllPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }

}
