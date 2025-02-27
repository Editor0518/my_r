using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("For Inv UI")]
    public GameObject inventoryWhole;
    public Transform slotSpace;
    public List<Slot> slots = new();

    [Header("For Inv Info UI")]
    public GameObject infoWhole;
    public Image infoImg;
    public TMP_Text infoName;
    public TMP_Text infoContent;
    public GameObject useButton;
    public GameObject trashButton;
    public GameObject giftButton;
    //ItemBlock giftBlock;

    [Header("For Item Gain Popup UI")]
    public Animator popAnim;
    public Image popImg;
    public TMP_Text popName;

    [Header("For File")]
    public ItemList itemList;
    public List<Item> itemsShow;

    [Header("Item Use")]
    public DialogueManager smallDialogueManager;
    public StoryBlock storyBlock;

    private void Start()
    {
        SetSlots();
        EndGiftMode();
    }

    public string FindItemName(string code)
    {
        // Debug.Log("찾기 시작: " + code);
        for (int i = 0; i < itemList.item.Count; i++)
        {
            if (itemList.item[i].nameForFind.Equals(code))
            {
                // Debug.Log("찾음: " + itemList.item[i].nameForShow);
                return itemList.item[i].nameForShow;
            }
        }
        // Debug.Log("못찾음: " + code);
        return code;
    }


    public void AddItem(string itemName)
    {
        if (itemsShow.Count >= slots.Count)
        {
            Debug.Log("저장할 공간 없음.");
            return;//나중에 부족하면 삭제하도록 만들기
        }
        itemList.ChangeIsCollected(itemName);
        Item item = FindItemInItemList(itemName);
        if (item != null)
        {
            GotItemPopup(item);
            itemsShow.Insert(0, item);


        }
        else Debug.LogWarning("아이템을 찾을 수 없습니다!: " + itemName);

        SaveItems();
    }

    void GotItemPopup(Item item)
    {
        popImg.sprite = item.sprite;
        popName.text = item.nameForShow + UnderLetter.SetUnderLetterEnd(item.nameForShow, '을') + " 얻었습니다!";
        popImg.gameObject.SetActive(true);
        popAnim.SetTrigger("ItemPopupOn");
    }

    Item FindItemInItemList(string itemName)
    {
        itemName = itemName.Replace(" ", "");
        for (int i = 0; i < itemList.item.Count; i++)
        {
            if (itemList.item[i].nameForShow.Replace(" ", "").Equals(itemName))
            {
                return itemList.item[i];
            }
            else if (itemList.item[i].nameForFind.Equals(itemName))
                return itemList.item[i];
        }
        return null;
    }

    public void RemoveItem(string itemName)
    {
        if (itemsShow.Count == 0)
        {
            return;//지울 게 없음
        }
        itemName = itemName.Replace(" ", "");
        for (int i = 0; i < itemsShow.Count; i++)
        {
            if (itemsShow[i].nameForShow.Replace(" ", "").Equals(itemName))
            {
                itemsShow.RemoveAt(i);
                break;
            }
        }

        SaveItems();
    }

    public void SaveItems()
    {//기록 로컬 세이브 저장.
        //SetInv();
    }

    public void LoadItems()
    { //로컬 세이브에서 불러옴. 덮어쓰기.

    }


    void SetSlots()
    {
        for (int i = slots.Count; i < slotSpace.childCount; i++)
        {
            /*Slot newSlot = new();
            newSlot.clickBtn = slotSpace.GetChild(i).GetComponent<Button>();
            newSlot.itemImg = slotSpace.GetChild(i).GetChild(0).GetComponent<Image>();*/
            slots.Add(slotSpace.GetChild(i).GetComponent<Slot>());
        }
    }

    public void SetInv()
    {
        LoadItems();
        GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().isPause = true;

        for (int i = 0; i < slots.Count; i++)
        {
            if (itemsShow.Count <= i)
            {
                slots[i].itemImg.sprite = null;
                slots[i].clickBtn.interactable = false;
                continue;
            }
            slots[i].itemImg.sprite = itemsShow[i].sprite;
            slots[i].itemName = itemsShow[i].nameForShow;
            slots[i].itemContent = itemsShow[i].content;
            slots[i].itemCanUse = itemsShow[i].canUse;
            slots[i].itemIsTrash = itemsShow[i].isTrash;
            slots[i].clickBtn.interactable = true;
        }
        slots[pastIndex].isSelectedImg.enabled = false;
        infoWhole.SetActive(false);
        inventoryWhole.SetActive(true);

    }

    public void CloseInv()
    {
        GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().isPause = false;
        inventoryWhole.SetActive(false);

    }
    int pastIndex = 0;
    public void SetInvInfo(int index)
    {
        slots[pastIndex].isSelectedImg.enabled = false;
        slots[index].isSelectedImg.enabled = true;
        pastIndex = index;
        infoWhole.SetActive(true);
        infoImg.sprite = slots[index].itemImg.sprite;
        infoName.text = slots[index].itemName;
        infoContent.text = slots[index].itemContent;
        useButton.SetActive(slots[index].itemCanUse);
        trashButton.SetActive(slots[index].itemIsTrash);
        giftButton.SetActive(slots[index].giftOn.enabled);
        // giftBlock = new ItemBlock(slots[index].itemName, slots[index].giftBlock);
    }

    private void Update()
    {
        if (!inventoryWhole.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.Escape)) CloseInv();
    }

    public void ThrowAwayItem()
    {
        RemoveItem(infoName.text);
        SetInv();
    }

    public void UseItem()
    {
        ItemUseDialogue itemUseDialogue = itemList.GetItemUsedWithNames(infoName.text);


        storyBlock.block.Clear();
        for (int i = 0; i < itemUseDialogue.content.Count; i++)
        {
            //storyBlock.block.Add(new Block(itemUseDialogue.content[i]));

        }
        Debug.Log("이거 쓰지마1!!!");
        smallDialogueManager.ChangeCurrentBlock(0);
        smallDialogueManager.ChangeDialogue();
        StartCoroutine("UseItemCheck");
        RemoveItem(infoName.text);

    }

    IEnumerator UseItemCheck()
    {//UseItem
        WaitForSeconds wait = new WaitForSeconds(0.7f);

        while (true)
        {
            if (smallDialogueManager.isNoNext && smallDialogueManager.crtBranch == 0)
            {
                Debug.Log("이거 쓰지마!!!!!");
                //end
                break;
            }
            yield return wait;
        }
        EndUseItem();
        yield return null;
    }

    void EndUseItem()
    {
        storyBlock.block.Clear();
        SetInv();
    }

    /*
    public void StartGiftEvent(List<ItemBlock> itemBlocks)
    {
        SetInv();

        for (int s = 0; s < slots.Count; s++)
        {
            for (int i = 0; i < itemBlocks.Count; i++)
            {
                Debug.Log(slots[s].itemName);
                if (slots[s].itemName.Replace(" ", "").Equals(itemBlocks[i].itemName.Replace(" ", "")))
                {
                    Debug.Log("찾음" + slots[s].itemName);
                    slots[s].GiftModeOn(itemBlocks[i].newBlock);
                    break;
                }
            }
        }

    }
    */
    void EndGiftMode()
    {
        for (int s = 0; s < slots.Count; s++)
        {
            slots[s].GiftModeOff();
        }
        // giftBlock = null;
        CloseInv();
    }

    //gift 해서 선택된 상태일시.
    public void SelectBlock()
    {
        // RemoveItem(giftBlock.itemName);
        // GameObject.FindGameObjectWithTag
        //    ("DialogueManager").GetComponent<DialogueManager>().ChangeCurrentBlock(giftBlock.newBlock);
        EndGiftMode();

    }

}
