using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    public Image box_left;
    public Image box_right;
    public Image box_top;
    public Image box_bottom;


    private void Start()
    {
        StartCoroutine(MovePos(100, 0, 0, 0));
    }

    IEnumerator MovePos(float left, float right, float top, float bottom) {
        
        float crt_left=box_left.rectTransform.anchoredPosition.x;
        float crt_right = box_right.rectTransform.anchoredPosition.x;
        float crt_top = box_top.rectTransform.anchoredPosition.y;
        float crt_bottom = box_bottom.rectTransform.anchoredPosition.y;

        left += crt_left;
        right += crt_right;
        top += crt_top;
        bottom += crt_bottom;

        Debug.Log(crt_left);
        while (!(crt_left.Equals(left)&& crt_right.Equals(right)&& crt_top.Equals(top) && crt_bottom.Equals(bottom))) {
            box_left.rectTransform.anchoredPosition = new Vector2(box_left.rectTransform.anchoredPosition.x+1, box_left.rectTransform.anchoredPosition.y);
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    

}
