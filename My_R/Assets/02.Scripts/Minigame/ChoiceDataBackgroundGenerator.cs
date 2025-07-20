using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDataBackgroundGenerator : MonoBehaviour
{
    public MinigameRunChoiceData choiceData;
    public List<Sprite> backgrounds;
    public bool generateOnStart = true;
    
    private void Start()
    {
        if(generateOnStart)
             SetBackgrounds();
    }

    void SetBackgrounds()
    {
        if (choiceData.allChoices.Count != backgrounds.Count)
        {
            Debug.LogWarning($"ChoiceData 수와 배경 수가 맞지 않아 실행하지 않았습니다!{choiceData.allChoices.Count},{backgrounds.Count}");
            return;
        }
        for (int i = 0; i < choiceData.allChoices.Count; i++)
        {
            choiceData.allChoices[i].background = backgrounds[i];
        }
    }
}
