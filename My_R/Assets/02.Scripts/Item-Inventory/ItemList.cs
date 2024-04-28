using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public string name;
    public Sprite sprite;
    [TextArea(1,3)]public string content;
}

[CreateAssetMenu(fileName ="ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] public List<Item> item;

    public void CreateItem(string name, string content) { 
        
    }

}
