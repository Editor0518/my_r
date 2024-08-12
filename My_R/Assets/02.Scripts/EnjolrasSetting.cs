using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnjolrasSetting : MonoBehaviour
{
    public Button applyButton;


    [Header("NAME")]
    public TMP_InputField inputField;
    public TMP_Text infoText;

    public string myName = "¾ÓÁ¹¶ó½º";

    [Header("LOOKS")]
    public GameObject[] looks=new GameObject[2];
    public int gender = -1;
    public bool finishedName = false;
    public GameObject resetGenderButton;

    private void Start()
    {
        ChangeMyName();
    }

    public void SetGender(bool isMale)
    {
        gender = isMale ? 1 : 0;
        looks[gender].SetActive(false);
        UpdateApplyButton();
        resetGenderButton.SetActive(true);
    }

    public void ResetGender()
    {
        gender = -1;
        UpdateApplyButton();
        looks[0].SetActive(true);
        looks[1].SetActive(true);
        resetGenderButton.SetActive(false);
    }

    public void ChangeMyName()
    {
        string input = inputField.text.Replace(" ", "");
        if (input.Equals("")) { 
            input = "¾ÓÁ¹¶ó½º";
            finishedName = true;
        }
        string[] dontUse = { "ÄáºêÆä¸£", "±×¶ûÅ×¸£", "Äí¸£Æä¶ô", "¶ó¸¶¸£Å©", "Á¹¸®", "¾ÆÆú·Î", "¹Ù¿À·¼" };
        for (int i = 0; i < dontUse.Length; i++)
        {
            if (input.Equals(dontUse[i]))
            {
                infoText.text = "»ç¿ëÇÒ ¼ö ¾ø´Â ÀÌ¸§ÀÔ´Ï´Ù!";
                finishedName = false;
                UpdateApplyButton();
                return;
            }
        }
        myName = input;
        infoText.text = myName + " / 22¼¼ (´ëÇÐ»ý)";
        finishedName = true;
        UpdateApplyButton();
    }

    void UpdateApplyButton()
    {
        if (myName.Equals("¾ÓÁ¹¶ó½º")) finishedName = true;
        applyButton.interactable = finishedName && gender > 0;
    }

    public void ApplyInfo()
    {
        SaveManager.instance.ResetAll();
        SaveManager.instance.SetGender(gender);
        SaveManager.instance.SetMyName(myName);
        GameSceneManager.instance.chapter = 0;
        GameSceneManager.instance.LoadScene(1);
    }

}
