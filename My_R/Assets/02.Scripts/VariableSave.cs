using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableSave : MonoBehaviour
{
    public void SetGender(bool isMale) {
        SaveManager.instance.SetGender(isMale);
    }

    public void AddCollectedItem(string itemName) {
        SaveManager.instance.AddCollectedItem(itemName);
    }
}
