using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntryChoice
{
    public int fromNodeId;
    public List<int> choices; // 왼, 중, 오 순
}

[System.Serializable]
public class NodeChoiceMap
{
    public int nodeId;
    public List<EntryChoice> entryChoices;
}

[CreateAssetMenu(fileName = "MinigameRunChoiceData", menuName = "ScriptableObject/MinigameRunChoiceData", order = 0)]
public class MinigameRunChoiceData : ScriptableObject
{
    public List<NodeChoiceMap> allChoices;

    public List<int> GetChoices(int currentNode, int fromNode)
    {
        var node = allChoices.Find(n => n.nodeId == currentNode);
        if (node == null) return null;
        var entry = node.entryChoices.Find(e => e.fromNodeId == fromNode);
        return entry?.choices;
    }

}
