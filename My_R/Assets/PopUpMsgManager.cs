using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMsgManager : MonoBehaviour
{
    [System.Serializable]
    public class PopUpMsg
    {
        public string msg;
        public Sprite spr;
        public float time;
        public bool isItem = false;
        public AudioClip audioClip;
        public PopUpMsg(string msg, Sprite spr, float time, bool isItem, AudioClip audioClip)
        {
            this.msg = msg;
            this.spr = spr;
            this.time = time;
            this.isItem = isItem;
            this.audioClip = audioClip;
        }
    }

    public class SprHolder
    {
        public string name;
        public Sprite spr;

        public SprHolder(string name, Sprite spr)
        {
            this.name = name;
            this.spr = spr;
        }
    }

    bool isPlaying = false;


    [Header("�˾� â")]
    public GameObject popUpWhole;
    public Animator popUpMsgAnim;
    RectTransform popUpMsgRect;
    public Image popUpMsgImg;
    public TMP_Text popUpMsgText;

    public List<PopUpMsg> popUpMsgs = new List<PopUpMsg>();
    public List<SprHolder> sprites = new List<SprHolder>();

    private void Start()
    {
        popUpMsgRect = popUpMsgAnim.GetComponent<RectTransform>();

        /*        //test
                AddPopUpMsgOnly("���Ⱑ ABC�� ���� ���Ƹ� �½��ϱ�?", "", 3, false);
                AddPopUpMsgOnly("��?", "", 2, false);
                AddPopUpMsgOnly("���Ⱑ ABC�� ���� ���Ƹ� �½��ϱ�?", "", 3, false);
                AddPopUpMsgOnly("��...����, ������?", "", 3, false);
                AddPopUpMsgOnly("ABC�� ���� ���Դϴ�.", "", 2, false);
                AddPopUpMsgOnly("��...", "", 2, false);
                AddPopUpMsgOnly("��, �װ�.���̺񾾰� �ƴ϶� �ƺ���ŵ��.", "", 3, false);
                AddPopUpMsgOnly("��������.", "", 2, false);
                AddPopUpMsgOnly("�׷��� �½��ϱ�?", "", 3, false);
                AddPopUpMsg("��, �� �¾ƿ�.", "", 2, false);*/
    }

    public void ClearAllMsg()
    {
        popUpMsgs.Clear();
        if (popUpMsgs.Count > 0) popUpMsgs = new List<PopUpMsg>();
        popUpWhole.SetActive(false);
        Debug.Log("offffff");
    }
    public void AddPopUpMsg(PopUpMsg popUpMsg)
    {
        AddPopUpMsgOnly(popUpMsg);
        PlayPopUpMsg();
    }

    public void AddPopUpMsgOnly(PopUpMsg popUpMsg)
    {
        popUpMsgs.Add(popUpMsg);
    }

    public void AddPopUpMsg(string msg, string sprName, float time, bool isItem, AudioClip audioClip)
    {
        AddPopUpMsgOnly(msg, sprName, time, isItem, audioClip);
        popUpWhole.SetActive(true);
        PlayPopUpMsg();

    }

    public void AddPopUpMsgOnly(string msg, string sprName, float time, bool isItem, AudioClip audioClip)
    {
        Sprite spr = FindSprite(sprName);
        popUpMsgs.Add(new PopUpMsg(msg, spr, time, isItem, audioClip));

    }

    public void PlayPopUpMsg()
    {
        if (popUpMsgRect == null) popUpMsgRect = popUpMsgAnim.GetComponent<RectTransform>();
        if (isPlaying) return;
        isPlaying = true;
        popUpMsgImg.gameObject.SetActive(true);
        StopCoroutine(ShowPopUpMsg());
        StartCoroutine(ShowPopUpMsg());

    }

    IEnumerator ShowPopUpMsg()
    {
        float animDur = 0.5f;
        while (popUpMsgs.Count > 0)
        {
            PopUpMsg popUpMsg = popUpMsgs[0];
            //Debug.Log(popUpMsg.msg);


            if (popUpMsg.msg == "")
            {
                yield return new WaitForSeconds(popUpMsg.time);
            }
            else
            {
                popUpMsgText.text = popUpMsg.msg;
                popUpMsgRect.sizeDelta = new Vector2(popUpMsgText.preferredWidth + 85, popUpMsgRect.sizeDelta.y);
                popUpMsgImg.sprite = popUpMsg.spr;
                //popUpMsgAnim.SetTrigger("on");
                popUpMsgAnim.SetBool("isOn", true);
                yield return new WaitForSeconds(popUpMsg.time + animDur);
                popUpMsgAnim.SetBool("isOn", false);
            }

            if (popUpMsgs.Count > 0) popUpMsgs.RemoveAt(0);

            //opUpMsgAnim.SetTrigger("off");
            // yield return new WaitForSeconds(animDur + 0.1f);
        }
        isPlaying = false;
    }


    public Sprite FindSprite(string name)
    {
        foreach (SprHolder spr in sprites)
        {
            if (spr.name == name) return spr.spr;
        }
        return null;
    }
}
