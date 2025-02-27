using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentWarning : MonoBehaviour
{
    public TMP_Text warnText;
    public TMP_Text detailText;
    public RectTransform detailButton;
    [TextArea] public string warningShort;
    [TextArea] public string warningLong;

    bool isShort = true;

    private void Start()
    {
        warnText.text = warningShort;
        DialogueManager.instance.canClickToNext = false;
    }

    private void OnEnable()
    {
        if (DialogueManager.instance != null)
            DialogueManager.instance.canClickToNext = false;
    }

    public void ToggleWarning()
    {
        if (isShort)
        {
            warnText.text = warningLong;
            detailText.text = "간략하게 보기";
            detailButton.anchoredPosition = new Vector2(detailButton.anchoredPosition.x, -32);
            isShort = false;
        }
        else
        {
            warnText.text = warningShort;
            detailText.text = "자세히 보기 [스포일러 포함]";
            detailButton.anchoredPosition = new Vector2(detailButton.anchoredPosition.x, 35);
            isShort = true;
        }
    }

}
