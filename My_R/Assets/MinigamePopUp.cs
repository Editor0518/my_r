using System.Collections.Generic;
using UnityEngine;
using static PopUpMsgManager;

public class MinigamePopUp : MonoBehaviour
{

    public PopUpMsgManager popManager;
    public List<PopUpMsg> msgs;

    void OnEnable()
    {
        float totalTime = 0.0f;
        for (int i = 0; i < msgs.Count - 1; i++)
        {
            popManager.AddPopUpMsgOnly(msgs[i]);
            totalTime += msgs[i].time;
        }
        popManager.AddPopUpMsg(msgs[msgs.Count - 1]);
        totalTime += msgs[msgs.Count - 1].time;
        Debug.Log("ÆË¾÷ ÃÑ Àç»ý ½Ã°£" + totalTime);

    }

    public void ClearAllMsg()
    {
        popManager.ClearAllMsg();
    }

}
