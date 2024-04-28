using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnjolrasSetting : MonoBehaviour
{
    public string myName="æ”¡π∂ÛΩ∫";
    bool hasUnderletter = false;


    private void Start()
    {
        HasUnderLetter();
    }

    public void HasUnderLetter() {
        UnderLetter.HasUnderLetter(myName);
    }


}
