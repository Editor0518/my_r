using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColorChange : MonoBehaviour
{
    public Color colorA;
    public bool boldA = false;
    public Color colorB;
    public bool boldB = false;

    public TMP_Text text;

    public void ChangeColorToA()
    {
        text.color = colorA;
        text.fontStyle = boldA ? FontStyles.Bold : FontStyles.Normal;
    }

    public void ChangeColorToB()
    {
        text.color = colorB;
        text.fontStyle = boldB ? FontStyles.Bold : FontStyles.Normal;
    }
}
