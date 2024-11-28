using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad instance;

    public List<SaveSlot> saveSlots = new List<SaveSlot>();

    public List<TMP_Text> nextTxts = new List<TMP_Text>();

    public int page = 0;
    public bool isSaveMode = true;

    public void SaveOrLoadDataInSlot(int index)
    {
        Debug.Log("SaveOrLoadDataInSlot " + index);
        if (isSaveMode)
        {
            saveSlots[index / 2].Save(index, page);

        }
        else
        {
            saveSlots[index / 2].Load(index, page);

        }
    }

    public void ChangePage(int page)
    {
        this.page = page;
    }


    private void Awake()
    {
        SaveAndLoad.instance = this;
    }
}
