using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private void Awake()
    {
        SaveManager.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //디버깅용
        ResetAll();
    }

    public void ChangeHeart(float add) {//호감도=Heart, dft=0인데 처음에 100으로 시작함.
        PlayerPrefs.SetFloat("Heart", (PlayerPrefs.GetFloat("Heart") + add));
    }

    public float GetHeart() {
        return PlayerPrefs.GetFloat("Heart", 0.0f);
    }

    public void ResetAll() {
        PlayerPrefs.DeleteAll();
    }

    public void SetGender(bool isMale)
    {
        PlayerPrefs.SetInt("Gender", isMale?1:0);
    }

}
