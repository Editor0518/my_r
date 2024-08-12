using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Speech : MonoBehaviour
{
    [Header("player")]
    public Rigidbody2D rigid;
    public TMP_Text text;

    void Start()
    {
        
    }
    //-4 ~ -1
    //1080
    //~360, 720, 1080

    void Update()
    {
        float y=Input.mousePosition.y;
        text.text = y+"";
        //rigid.AddForce(new Vector2(0, Direction(y)*2));
        if (y > 1080) y = 1080;
        if (y <= 0) y = 0.1f;
        rigid.transform.localPosition = new Vector3(rigid.transform.localPosition.x, -1 * (1080 / y), rigid.transform.localPosition.z);
        if(rigid.transform.localPosition.y<-4) rigid.transform.localPosition = new Vector3(rigid.transform.localPosition.x, -4, rigid.transform.localPosition.z);
    }
    //-1.3   -2.6   -3.9
    int Direction(float dir) {
        if (dir > 1080/2) return 1;//720+200
        else if (dir <1080/2) return -1;// 360-200
        else return 0;

    }
}
