using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Heart : MonoBehaviour
{
    public Animator wholeAnim;
    public Image fill;
    public TMP_Text text;

    private void Start()
    {
        
    }

    public void HeartUp(float add)
    {
        StartCoroutine(HeartFillChange(add));
    }

    IEnumerator HeartFillChange(float add)
    {
        wholeAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        WaitForSeconds wait = new(0.05f);
        float current = SaveManager.instance.GetHeart();//0 to 100
        float addHeart = add+ current;//0 to 100+
        Debug.Log(add);

        bool ifAddBig = addHeart >= current;
        while (ifAddBig ? current < addHeart : current > addHeart)
        {

            current += 1f;//*0.75f;
            fill.fillAmount = current * 0.01f*0.75f;

            text.text = ((int)(current))+"%";
            if (((int)(current))>=100) text.text = "MAX";

            yield return wait;
        }
        SaveManager.instance.ChangeHeart(add);
        yield return null;
    }

    void ChangeHeart()
    {

    }

}
