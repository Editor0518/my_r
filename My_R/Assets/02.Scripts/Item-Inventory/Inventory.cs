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
            Debug.Log("������ ���� ����.");
            return;//���߿� �����ϸ� �����ϵ��� �����
        }
        Item item = FindItemInItemList(itemName);
        if (item != null) {
            GotItemPopup(item);
            itemsShow.Add(item); 
        }
        else Debug.LogWarning("�������� ã�� �� �����ϴ�!: " + itemName);

        SaveItems();
    }

    void GotItemPopup(Item item) {
        popImg.sprite = item.sprite;
        popName.text = item.name+UnderLetter.SetUnderLetterEnd(item.name, '��')+" ������ϴ�!";
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
            return;//���� �� ����
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
    {//��� ���� ���̺� ����.

    }

    public void LoadItems()
    { //���� ���̺꿡�� �ҷ���. �����.

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
