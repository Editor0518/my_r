using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string nameForFind;
    public string nameForShow;

    public Sprite sprite;
    [TextArea(1, 3)] public string content;

    //public List<string> category;
    public bool canUse = false;
    public bool isTrash = false;
    public bool isCollected = false;
    public string afterUsed;
}

[System.Serializable]
public class ItemUseDialogue
{
    public string name;
    [TextArea(1, 3)] public List<string> content;
}

[CreateAssetMenu(fileName = "ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] public List<Item> item;
    [SerializeField] public List<ItemUseDialogue> itemUse;

    public void CreateItem(string name, string content)
    {

    }

    public void ChangeIsCollected(string name)
    {
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].nameForShow.Replace(" ", "").Equals(name))
            {
                item[i].isCollected = true;
            }
        }
    }

    public void ChangeIsCollected(string[] nameArray)
    {
        for (int i = 0; i < nameArray.Length; i++)
        {
            ChangeIsCollected(nameArray[i]);
        }
    }
    public string FindItemName(string code)
    {
        Debug.Log("찾기 시작: " + code);
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].nameForFind.Equals(code))
            {
                Debug.Log("찾음: " + item[i].nameForShow);
                return item[i].nameForShow;
            }
        }
        return code;
    }

    public List<Item> GetItemsWithNames(List<string> names)
    {
        for (int i = 0; i < names.Count; i++)
            if (names[i].Contains(" ")) names[i] = names[i].Replace(" ", "");

        List<Item> temp = new();

        for (int i = 0; i < item.Count; i++)
        {
            for (int n = 0; n < names.Count; n++)
            {
                if (item[i].nameForShow.Replace(" ", "").Equals(names[n]))
                {
                    temp.Add(item[i]);
                    break;
                }
            }

        }

        return temp;
    }

    public ItemUseDialogue GetItemUsedWithNames(string itemName)
    {
        itemName = itemName.Replace(" ", "");
        for (int i = 0; i < itemUse.Count; i++)
        {
            if (itemUse[i].name.Replace(" ", "").Equals(itemName))
            {
                return itemUse[i];

            }
        }
        Debug.Log("찾는 아이템이 ItemUseDialogue에 등록되지 않음.");
        return null;
    }
    /*
    public List<Item> GetItemsInCategory(string category) {
        List<Item> temp = new();

        for (int i = 0; i < item.Count; i++) {
            if (ItemInCategory(item[i], category)) {
                temp.Add(item[i]);
            }
        }

        return temp;
    }

    bool ItemInCategory(Item item, string category) {
        for (int i = 0; i < item.category.Count; i++) {
            if (item.category[i].Equals(category)) {
                return true;
            }
        }
        return false;
    }
    */
}
