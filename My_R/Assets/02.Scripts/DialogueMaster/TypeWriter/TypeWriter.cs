using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;

// TypeWriter.cs
public class TypeWriter : MonoBehaviour
{
    [Header("Thinking Animator")]
    public Animator thinkingAnim;
    public TMP_Text thinkingText;

    [Header("Sentence")]
    public bool isOn = true;
    public int name_ch;
    public string sentence;
    public string current;

    [Header("Type Setting")]
    public GameObject dialogue;
    public TMP_Text content;
    public DialogueMaster dialogueMaster;

    public float autoDelay = 1.0f;
    public bool isTyping = false;

    [Header("Auto Click")]
    public RectTransform nextImg;
    public Image autoImg;
    private RectTransform autoRect;
    public float delay = 1.0f;

    private float currentDelay = 0.07f;

    public void ThinkingOn(string thinkingContent)
    {
        thinkingText.text = thinkingContent;
        thinkingAnim.gameObject.SetActive(false);
        thinkingAnim.gameObject.SetActive(true);
    }

    public void ThinkingOff()
    {
        thinkingText.enabled = false;
    }

    public void StartTyping(string sentence)
    {
        nextImg.gameObject.SetActive(false);
        if (autoRect == null) autoRect = autoImg.GetComponent<RectTransform>();
        autoRect.gameObject.SetActive(false);
        StopAllCoroutines();

        this.sentence = PreprocessTags(sentence);
        current = "";
        
        currentDelay = 0.07f;//<normal>
        StartCoroutine(Typing());
    }

    public void StopTyping()
    {
        StopAllCoroutines();
        current = sentence.Replace("<slow>","").Replace("<fast>","").Replace("<normal>","");
        content.text = current;
        isTyping = false;
        DialogueMaster.Instance.RunAfterCMD();
    }

    IEnumerator Typing()
    {
        isTyping = true;
        current = "";
        StringBuilder builder = new StringBuilder();

        int i = 0;
        while (i < sentence.Length)
        {
            if (!dialogue.activeInHierarchy)
            {
                yield return null;
                continue;
            }

            if (sentence[i] == '<')
            {
                int closeIndex = sentence.IndexOf('>', i);
                if (closeIndex != -1)
                {
                    string tag = sentence.Substring(i, closeIndex - i + 1);

                    switch (tag)
                    {
                        case "<slow>": currentDelay = 0.15f; break;
                        case "<fast>": currentDelay = 0.03f; break;
                        case "<normal>": currentDelay = 0.07f; break;
                        case "<shake>": /* optional effect */ break;
                        case "<big>": case "<small>": case "</big>": case "</small>":
                        case "<b>": case "</b>": case "<i>": case "</i>":
                        case "<color=yellow>": 
                        case "<color=red>":
                            case "<color=#b9de7d>":
                        case "</color>":
                            builder.Append(tag);
                            current = builder.ToString();
                            content.text = current;
                            break;
                        default:
                            builder.Append(tag);
                            current = builder.ToString();
                            content.text = current;
                            break;
                    }
                    i = closeIndex + 1;
                    continue;
                }
            }

            builder.Append(sentence[i]);
            current = builder.ToString();
            content.text = current;
            i++;
            yield return new WaitForSeconds(currentDelay);
        }

        isTyping = false;
        nextImg.anchoredPosition = new Vector2(content.preferredWidth % 920 / 2 + 30, -content.preferredHeight + 98);
        autoRect.anchoredPosition = nextImg.anchoredPosition;
        if (!DialogueMaster.isAuto) nextImg.gameObject.SetActive(true);
        else autoRect.gameObject.SetActive(true);

        if (DialogueMaster.isAuto)
        {
            StartCoroutine(AutoDelay());
        }
        DialogueMaster.Instance.RunAfterCMD();
    }

    private string PreprocessTags(string input)
    {
        input = Regex.Replace(input, "<yb>(.*?)</yb>", "<color=yellow><b>$1</b></color>");
        input = Regex.Replace(input, "<y>(.*?)</y>", "<color=yellow>$1</color>");
        input = Regex.Replace(input, "<red>(.*?)</red>", "<color=red>$1</color>");
        input = Regex.Replace(input, "<g>(.*?)</g>", "<color=#b9de7d>$1</color>"); //185, 222, 125, 255 GRANTAIRE
        input = Regex.Replace(input, "<big>(.*?)</big>", "<size=120%>$1</size>");
        input = Regex.Replace(input, "<small>(.*?)</small>", "<size=70%>$1</size>");
        return input;
    }

    public void ChangeIsAuto()
    {
        DialogueMaster.isAuto = !DialogueMaster.isAuto;
        DialogueMaster.canClickToNext = !DialogueMaster.isAuto;
    }

    public void ShowAutoDelayIcon(bool isShow)
    {
        autoImg.color = isShow ? Color.white : Color.clear;
    }

    IEnumerator AutoDelay()
    {
        autoImg.enabled = true;
        float delta = 0.05f;
        WaitForSeconds wait = new WaitForSeconds(delta);
        float time = 0.0f;
        while (time < delay)
        {
            time += delta;
            autoImg.fillAmount = time / delay;
            yield return wait;
        }

        if (!dialogueMaster.dialogueUI.isNoNext)
            dialogueMaster.ContinueDialogue();

        autoImg.enabled = false;
    }

    private void Start()
    {
        autoRect = autoImg.GetComponent<RectTransform>();
    }
}
