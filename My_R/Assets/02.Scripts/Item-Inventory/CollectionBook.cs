using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionBook : MonoBehaviour
{
    DialogueManager dialogueManager;
    public GameObject collectionWhole;
    public ItemList itemList;
    public Transform itemSpace;
    public List<ItemHolder> items;

    public TMP_Text collectedText;
    public int collectedCount = 0;


    public void OnEnableCollection()
    {
        if (dialogueManager == null) dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        SetCollectionItem();
        collectionWhole.SetActive(true);
        dialogueManager.isPause = true;
    }

    public void OnDisableCollection()
    {
        if (dialogueManager == null) dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        collectionWhole.SetActive(false);
        dialogueManager.isPause = false;
    }

    private void Start()
    {
        SetCollectionItem();
    }

    public void SetCollectionItem()
    {
        if (items.Count == 0) return;
        if (items.Count < itemList.item.Count)
        {
            for (int i = 0; i < itemSpace.childCount; i++)
                items.Add(itemSpace.GetChild(i).GetComponent<ItemHolder>());
        }
        for (int i = 0; i < itemList.item.Count; i++)
        {
            items[i].SetItem(itemList.item[i].name, itemList.item[i].content, itemList.item[i].sprite);
            items[i].SetIsCollected(itemList.item[i].isCollected);
        }
        float result = (float)collectedCount / (float)itemList.item.Count;
        float percent = (float)Mathf.Floor(result * 1000f) / 1000f;
        // Debug.Log(collectedCount+" / "+ itemList.item.Count +"  = "+ result + " ,  "+percent);
        collectedText.text = $"수집 진행도 {collectedCount}/{itemList.item.Count} ({percent * 100}%)";
    }

    public void LoadCollection()
    {

    }

    public void Refresh()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Reload();
        }
    }
}
