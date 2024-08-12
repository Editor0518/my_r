using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public ItemInfoPointer infoPointer;
    [HideInInspector] public string itemName;
    [HideInInspector] public string itemContent;
    [HideInInspector] public bool itemCanUse;
    [HideInInspector] public bool itemIsTrash;
    public Image itemImg;
    public Image isSelectedImg;
    public Button clickBtn;
    public Image giftOn;
    public StoryBlock giftBlock;
    public Vector2 pos;
    Vector2 startPos=new Vector2(116, -115);
    RectTransform rect;
    Vector3 scaleUp = new Vector3(1.1f, 1.1f, 1f);

    private void Start()
    {
        rect = this.itemImg.GetComponent<RectTransform>();
    }

    public void PointerOnMe()
    {
        if (!clickBtn.interactable) return;
        if(rect==null) rect = this.itemImg.GetComponent<RectTransform>();
        infoPointer.SetInfoPointer(startPos, 150, pos, itemName);
        rect.localScale = scaleUp;
    }

    public void PointerNotOnMe()
    {
        infoPointer.PointerOut();
        rect.localScale = Vector3.one;
    }

    public void GiftModeOn(StoryBlock nextBlock) {
        giftBlock = nextBlock;
        giftOn.enabled = true;
    }

    public void GiftModeOff()
    {
        giftOn.enabled = false;
        giftBlock = null;
    }

}
