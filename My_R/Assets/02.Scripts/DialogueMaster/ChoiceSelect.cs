using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : ButtonEffect
{
    public DialogueUIManager dialogueUI;
    public TMP_Text choiceText;
    public string choice_after_cmd;
    public string log;
    public string move;
    [Space]
    public Image icon;
    public Sprite[] iconType;

    private void Start()
    {
        if (dialogueUI == null) dialogueUI = GameObject.FindWithTag("GameUI").GetComponent<DialogueUIManager>();
    }
    public void SetChoice(string choiceType, string sentence, string choice_after_cmd, string move, string log)
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
            case "THINK":
                choiceTypeInt = 2;
                break;
        }

        icon.sprite = iconType[choiceTypeInt];
       // sentence = dialogueUI.ReplaceEnjolrasName(sentence);
        if (sentence.Contains("["))
        {
            sentence = sentence.Replace("[ ", "");
            sentence = sentence.Replace(" ]", "");
        }
        choiceText.text = sentence;
        // this.clip = clip;
        this.choice_after_cmd = choice_after_cmd;
        this.move = move;
        this.log = log;
        this.GetComponent<Button>().interactable = false;
        this.GetComponent<Button>().interactable = true;
    }

    public void PlaySound()
    {
        OnClickSound();
    }


    public Sprite GetChoiceType()
    {
        return  icon.sprite;
    }

    public string GetLog()
    {
        return log;
    }
}
