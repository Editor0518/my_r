using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorChange : MonoBehaviour
{
    Image img;
    public Color colorA;
    public Color colorB;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void ChangeColorToA() {
        img.color = colorA;
    }

    public void ChangeColorToB()
    {
        img.color = colorB;
    }
}
