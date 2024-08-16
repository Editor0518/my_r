using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : ButtonEffect
{
    public DialogueManager dialogueManager;
    public TMP_Text choiceText;
    public AudioClip clip;


    private void Start()
    {
        if (dialogueManager == null) dialogueManager = GameObject.FindWithTag("DialogueManager").GetComponent<DialogueManager>();
    }
    public void SetChoice(string sentence, AudioClip clip)
    {
        choiceText.text = dialogueManager.ReplaceEnjolrasName(sentence);
        this.clip = clip;
        this.GetComponent<Button>().interactable = false;
        this.GetComponent<Button>().interactable = true;
    }

    public void PlaySound()
    {
        if (clip != null)
            SoundManager.instance.PlaySound(clip, "");
        else OnClickSound();
    }

}
