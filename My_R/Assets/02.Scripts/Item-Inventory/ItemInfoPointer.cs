using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPointer : MonoBehaviour
{
    RectTransform rect;
    Image img;
    TMP_Text nameText;
    public Vector2 delta=new Vector2(250,-200);

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        nameText = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void SetInfoPointer(Vector2 pos, string itemName) {
        Debug.Log(pos);
        rect.anchoredPosition = pos + delta;
        SetInfoPointer(itemName);
    }

    public void SetInfoPointer(Vector2 startPos, int width, Vector2 pos, string itemName) {
        rect.anchoredPosition = startPos + new Vector2(width*pos.x, -width*pos.y) + delta;
        SetInfoPointer(itemName);
    }

    void SetInfoPointer(string itemName) {
        nameText.text = itemName;
        img.enabled = true;
    }

    public void PointerOut() {
        img.enabled = false;
        nameText.text = "";
    }


}
