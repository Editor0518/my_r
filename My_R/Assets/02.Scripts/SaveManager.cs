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
        
    }

    public void ChangeHeart(float add) {//ȣ����=Heart, dft=0�ε� ó���� 100���� ������.
        PlayerPrefs.SetFloat("Heart", (PlayerPrefs.GetFloat("Heart") + add));
    }

    public void ResetAll() {
        PlayerPrefs.DeleteAll();
    }

}