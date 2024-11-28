using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : ButtonEffect
{
    public DialogueManager dialogueManager;
    public TMP_Text choiceText;
    public string choice_after_cmd;
    public string move;
    [Space]
    public Image icon;
    public Sprite[] iconType;

    private void Start()
    {
        if (dialogueManager == null) dialogueManager = GameObject.FindWithTag("DialogueManager").GetComponent<DialogueManager>();
    }
    public void SetChoice(string choiceType, string sentence, string choice_after_cmd, string move)
    {

        int choiceTypeInt = 0;
        switch (choiceType)
        {
            case "SAY":
                choiceTypeInt = 0;
                break;
            case "ACT":
                choiceTypeInt = 1;
                break;
        }

        icon.sprite = iconType[choiceTypeInt];
        choiceText.text = dialogueManager.ReplaceEnjolrasName(sentence);
        // this.clip = clip;
        this.choice_after_cmd = choice_after_cmd;
        this.move = move;
        this.GetComponent<Button>().interactable = false;
        this.GetComponent<Button>().interactable = true;
    }

    public void PlaySound()
    {
        OnClickSound();
    }

}
