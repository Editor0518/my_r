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

        public Slot(Image slotImg, Image stickerImg, TMP_Text slotText, TMP_Text popText)
        {
            this.slotImg = slotImg;
            this.stickerImg = stickerImg;
            this.slotText = slotText;
            this.popText = popText;
        }
    }

    public int myOrder = 0;//몇번째 슬롯인지
    public int page = 0;//몇번째 페이지인지
    public List<Slot> slots;

    public void Save(int index)
    {
        SaveManager.instance.SaveData((page * 8) + myOrder + index);
    }

    public void ChangePage(int page)
    {
        this.page = page;
    }

    public void Load()
    {
        string[] data = { SaveManager.instance.LoadData((page * 8) + myOrder), SaveManager.instance.LoadData((page * 8) + myOrder + 1), };
    }

    #region 저장/불러오기 가이드 텍스트
    public void PopUpWannaSaveOrOverwrite(int index)
    {
        slots[index].popText.text = slots[index].slotImg.sprite == null ? "클릭하여 이곳에<br>저장하기" : "클릭하여 이곳에<br>덮어쓰기";
        slots[index].popText.transform.parent.gameObject.SetActive(true);
    }

    public void PopUpOff(int index)
    {
        slots[index].popText.transform.parent.gameObject.SetActive(false);
    }
    #endregion

}
