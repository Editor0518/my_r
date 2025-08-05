using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotePanel : MonoBehaviour
{
    private void OnEnable()
    {
       // DialogueManager.instance.canClickToNext = false;
        Invoke("ClosePanel", 14f);
    }

    void ClosePanel()
    {
        //DialogueManager.instance.ChangeCurrentBlock(1);
        gameObject.SetActive(false);
    }


}
