using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameHolder", menuName = "ScriptableObjects/MinigameHolder", order = 3)]
public class MinigameHolder : ScriptableObject
{
    public static MinigameHolder instance;

    void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public struct DMinigame
    {
        public string name;
        public GameObject minigame;
    }
    public List<DMinigame> minigameList;

    public GameObject FindMinigame(string name)
    {
        return minigameList.Find(x => x.name == name).minigame;
    }
}
