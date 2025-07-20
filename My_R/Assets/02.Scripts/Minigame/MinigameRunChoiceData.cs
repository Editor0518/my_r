using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntryChoice
{
    public int fromNodeId;
    public List<int> choices; // 왼, 중, 오 순
}

[System.Serializable]
public class Edge
{
    public int from;
    public int to;
    public int weight; // 선 위의 점 개수
}

[System.Serializable]
public class NodeChoiceMap
{
    public int nodeId;
    public Sprite background;
    public List<EntryChoice> entryChoices;
}

[CreateAssetMenu(fileName = "MinigameRunChoiceData", menuName = "ScriptableObject/MinigameRunChoiceData", order = 0)]
public class MinigameRunChoiceData : ScriptableObject
{
    public List<NodeChoiceMap> allChoices;
    // 이걸 새로 추가
    public List<Edge> edgeWeights;
    public List<int> GetChoices(int currentNode, int fromNode)
    {
        var node = allChoices.Find(n => n.nodeId == currentNode);
        if (node == null) return null;
        var entry = node.entryChoices.Find(e => e.fromNodeId == fromNode);
        return entry?.choices;
    }
    
    public int GetEdgeWeight(int from, int to)
    {
        var edge = edgeWeights.Find(e => (e.from == from && e.to == to) || (e.from == to && e.to == from));
        return edge != null ? edge.weight : 1; // 없으면 기본값 1
    }

    public Sprite GetBackground(int index)
    {
        return allChoices[index].background;
    }

}
