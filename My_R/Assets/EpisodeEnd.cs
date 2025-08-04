using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeEnd : MonoBehaviour
{
    public static int nextChapter = 1;
    public static int nextEpisodeBranch = 1;

    public GameObject onDemoEnd;
    AudioSource audioSource;


    private void OnEnable()
    {
        onDemoEnd.SetActive(nextChapter == -1);
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void GoNext()
    {

        if (!nextChapter.Equals(SheetLoader.chapter))
        {
            SheetLoader.chapter = nextChapter;
            SheetLoader.instance.StartLoadSheet(); //다이얼로그 자동 시작됨
        }
        else
        {
//            DialogueManager.instance.dirManager.MiniCutDisable();
  //          DialogueManager.instance.ChangeCurrentBlock(nextEpisodeBranch);
            DialogueMaster.Instance.StartDialogue(nextChapter, nextEpisodeBranch);
        }
        this.gameObject.SetActive(false);
    }
}
