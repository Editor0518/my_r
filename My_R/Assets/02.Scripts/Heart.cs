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
        float current = fill.fillAmount;
        add = (add * 0.01f) + current;
        bool ifAddBig = add >= current;
        while (ifAddBig ? current < add : current > add)
        {

            current += 0.01f;//*0.75f;
            fill.fillAmount = current * 0.75f;

            text.text = ((int)(current * 100))+"%";
            if (((int)(current * 100))>=100) text.text = "MAX";

            yield return wait;
        }
        yield return null;
    }

    void ChangeHeart()
    {

    }

}
