using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Slot
{
    [HideInInspector] public string itemName;
    [HideInInspector] public string itemContent;
    public Image itemImg;
    public Button clickBtn;
}

public class Inventory : MonoBehaviour
{
    [HideInInspector] public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("For Inv UI")]
    public Slot[] slots = new Slot[8];

    [Header("For Inv Info UI")]
    public Image infoImg;
    public TMP_Text infoName;
    public TMP_Text infoContent;

    [Header("For Item Gain Popup UI")]
    public Animator popAnim;
    public Image popImg;
    public TMP_Text popName;

    [Header("For File")]
    public ItemList itemList;
    public List<Item> itemsShow;


    public void AddItem(string itemName)
    {
        if (itemsShow.Count >= slots.Length)
        {
            Debug.Log("저장할 공간 없음.");
            return;//나중에 부족하면 삭제하도록 만들기
        }
        Item item = FindItemInItemList(itemName);
        if (item != null) {
            GotItemPopup(item);
            itemsShow.Add(item); 
        }
        else Debug.LogWarning("아이템을 찾을 수 없습니다!: " + itemName);

        SaveItems();
    }

    void GotItemPopup(Item item) {
        popImg.sprite = item.sprite;
        popName.text = item.name+UnderLetter.SetUnderLetterEnd(item.name, '을')+" 얻었습니다!";
        //popAnim
    }

    Item FindItemInItemList(string itemName)
    {
        for (int i = 0; i < itemList.item.Count; i++)
        {
            if (itemList.item[i].name.Equals(itemName))
            {
                return itemList.item[i];
            }
        }
        return null;
    }

    public void RemoveItem(string itemName)
    {
        if (itemsShow.Count == 0)
        {
            return;//지울 게 없음
        }
        for (int i = 0; i < itemsShow.Count; i++)
        {
            if (itemsShow[i].name.Equals(itemName))
            {
                itemsShow.RemoveAt(i);
                break;
            }
        }

        SaveItems();
    }

    public void SaveItems()
    {//기록 로컬 세이브 저장.

    }

    public void LoadItems()
    { //로컬 세이브에서 불러옴. 덮어쓰기.

    }

    private void OnEnable()
    {
        SetInv();
    }

    public void SetInv()
    {
        LoadItems();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].itemImg.sprite = itemsShow[i].sprite;
            slots[i].itemName = itemsShow[i].name;
            slots[i].itemContent = itemsShow[i].content;

        }
    }

    public void SetInvInfo(int index)
    {
        infoImg.sprite = slots[index].itemImg.sprite;
        infoName.text = slots[index].itemName;
        infoContent.text = slots[index].itemContent;
    }

}
