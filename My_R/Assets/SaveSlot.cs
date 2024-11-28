using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Image slotImg;
        public Image stickerImg;
        public TMP_Text slotText;
        public TMP_Text popText;
        public bool isSaved = false;

        public Slot(Image slotImg, Image stickerImg, TMP_Text slotText, TMP_Text popText, bool isSaved)
        {
            this.slotImg = slotImg;
            this.stickerImg = stickerImg;
            this.slotText = slotText;
            this.popText = popText;
            this.isSaved = isSaved;
        }
    }

    public Sprite tempSpr;

    public int myOrder = 0;//���° ��������
    public List<Slot> slots;

    public void Save(int index, int page)
    {
        int slotNum = (page * 8) + index;//0 + 2*2 + 1
        Debug.Log("Save " + slotNum);
        string[] str = SaveManager.instance.SaveData(slotNum);
        slots[index % 2].slotText.text = str[0] + " " + str[1];
        slots[index % 2].slotImg.sprite = tempSpr;
        slots[index % 2].isSaved = true;
    }



    public void Load(int index, int page)
    {
        int slotNum = (page * 8) + index;
        Debug.Log("Load " + slotNum);
        SaveManager.instance.LoadData(slotNum);

    }

    #region ����/�ҷ����� ���̵� �ؽ�Ʈ
    void PopUpSetSaveOrOverwrite(int index)
    {
        slots[index].popText.text = slots[index].slotImg.sprite == null ? "Ŭ���Ͽ� �̰���<br>�����ϱ�" : "Ŭ���Ͽ� �̰���<br>�����";

    }

    void PopUpSetLoad(int index)
    {
        if (slots[index].slotImg.sprite != null)
        {
            slots[index].popText.text = "Ŭ���Ͽ� �ҷ�����";

        }
        else slots[index].popText.transform.parent.gameObject.SetActive(false);
    }

    public void PopUpOn(int index)
    {
        slots[index].popText.transform.parent.gameObject.SetActive(true);
        if (SaveAndLoad.instance.isSaveMode) PopUpSetSaveOrOverwrite(index);
        else PopUpSetLoad(index);
    }

    public void PopUpOff(int index)
    {
        slots[index].popText.transform.parent.gameObject.SetActive(false);
    }
    #endregion

}
