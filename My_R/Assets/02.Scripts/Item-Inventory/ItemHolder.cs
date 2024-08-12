using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public ItemInfoPointer infoPointer;

    [Header("INFO")]
    [SerializeField] string itemName;
    [SerializeField] string itemContent;
    [SerializeField] Sprite itemSpr;
    Image itemImg;
    RectTransform rect;
    Vector3 scaleUp = new Vector3(1.1f, 1.1f,1f);
    public bool isCollected=false;

    private void Start()
    {
        Reload();
    }

    public void SetItem(string itemName, string itemContent, Sprite itemSpr) {
        this.itemName = itemName;
        this.itemContent = itemContent;
        this.itemSpr = itemSpr;
        Reload();
    }

    public void SetIsCollected(bool isCollected) {
        this.isCollected = isCollected;
        Reload();
    }

    public void Reload() {
        rect = GetComponent<RectTransform>();

        itemImg = GetComponent<Image>();
        itemImg.sprite = itemSpr;

        itemImg.color = isCollected ? Color.white : Color.black*0.75f;
        itemImg.enabled = (itemName != "");
    }

    public string GetItemName() {
        return itemName;
    }

    public string GetItemContent()
    {
        return itemName;
    }

    public string[] GetItemInfo()
    {
        string[] str = { itemName, itemContent };
        return str;
    }

    public Sprite GetItemSpr()
    {
        return itemSpr;
    }


    public void PointerOnMe() {
        if (!itemImg.enabled) return;
        infoPointer.SetInfoPointer(rect.anchoredPosition, isCollected?itemName:"???");
        rect.localScale = scaleUp;
    }

    public void PointerNotOnMe() {
        infoPointer.PointerOut();
        rect.localScale = Vector3.one;
    }
}
