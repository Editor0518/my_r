using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChoiceDataGenerator
{
    [MenuItem("Tools/Generate ChoiceData Asset")]
    public static void Generate()
    {
        var data = ScriptableObject.CreateInstance<MinigameRunChoiceData>();
        data.allChoices = new List<NodeChoiceMap>
        {
            new NodeChoiceMap
            {
                nodeId = 0, 
                entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 1, choices = new List<int>() {-1, 1} } //0->1
                }
            },
            new NodeChoiceMap {
                nodeId = 1,
                entryChoices = new List<EntryChoice> {
                    new EntryChoice { fromNodeId = 2, choices = new List<int> { 0, -1, 21 } },
                    new EntryChoice { fromNodeId = 0, choices = new List<int> { 2, 21 } } ,
                    new EntryChoice { fromNodeId = 21, choices = new List<int> { -1,-1, 2 } }
                }
            },
            new NodeChoiceMap { 
                nodeId = 2, 
                entryChoices = new List<EntryChoice>{
                new EntryChoice { fromNodeId = 1, choices = new List<int> { -1, 3, 16 } },
                new EntryChoice { fromNodeId = 3, choices = new List<int> { -1, 16, 1 } },
                new EntryChoice { fromNodeId = 16, choices = new List<int> { 1, -1, 3 } }
            } },
            new NodeChoiceMap { nodeId = 3, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 2, choices = new List<int> { 4, 12, 16 } },
                   // new EntryChoice { fromNodeId = 4, choices = new List<int> {} }, //길치 방지 길막
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 12, 16, 2 } },
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 16, 2, 4 } },
                    new EntryChoice { fromNodeId = 16, choices = new List<int> { -1, 4, 12 } }
                }
            },
            new NodeChoiceMap { nodeId = 4, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { 6, 5 } },
                    new EntryChoice { fromNodeId = 5, choices = new List<int> { 3, -1, 6 } },
                    new EntryChoice { fromNodeId = 6, choices = new List<int> { 3, -1, 5 } }
                }
            },
            new NodeChoiceMap { nodeId = 5, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 6, 8 } },
                    new EntryChoice { fromNodeId = 6, choices = new List<int> { 8, -1, 4 } },
                    new EntryChoice { fromNodeId = 8, choices = new List<int> { -1, 4, 6 } }
                }
            },
            new NodeChoiceMap { nodeId = 6, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 5, choices = new List<int> { 4, 7 } },
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 7, 5 } },
                    new EntryChoice { fromNodeId = 7, choices = new List<int> { 5, 4 } }
                }
            },
            new NodeChoiceMap
            {
                nodeId = 7, 
                entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 6, choices = new List<int>(){-1, -1, 8} },
                    new EntryChoice { fromNodeId = 8, choices = new List<int>(){6} }
                }
            },
            new NodeChoiceMap { nodeId = 8, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 5, choices = new List<int> { 7, 9 } },
                    new EntryChoice { fromNodeId = 9, choices = new List<int> { -1, 5, 7 } },
                    new EntryChoice { fromNodeId = 7, choices = new List<int> { 9, -1, 5 } }

                }
            },
            new NodeChoiceMap { nodeId = 9, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 8, choices = new List<int> { 22, 10 } },
                    new EntryChoice { fromNodeId = 22, choices = new List<int> { 10, -1, 8 } },
                    new EntryChoice { fromNodeId = 10, choices = new List<int> { -1, 8, 22 } }
                }
            },
            new NodeChoiceMap { nodeId = 10, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 9, choices = new List<int> { 23 } },
                    new EntryChoice { fromNodeId = 11, choices = new List<int> { -1, 23 } }
                }
            },
            new NodeChoiceMap { nodeId = 11, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 10, choices = new List<int> {}},//길치 방지 길막
                    //new EntryChoice { fromNodeId = 10, choices = new List<int> { -1, 20, 12 } },
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 10, -1, 20 } },
                    new EntryChoice { fromNodeId = 20, choices = new List<int> { 12, 10 } }
                }
            },
            new NodeChoiceMap { nodeId = 12, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { -1, 11, 13 } },
                    new EntryChoice { fromNodeId = 11, choices = new List<int> { 13, 3 } },
                    new EntryChoice { fromNodeId = 13, choices = new List<int> { 3, 11 } }
                }
            },
            new NodeChoiceMap { nodeId = 13, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 19, 14 } },
                    new EntryChoice { fromNodeId = 14, choices = new List<int> { -1, 12, 19 } },
                    new EntryChoice { fromNodeId = 19, choices = new List<int> { 14, -1, 12 } }
                }
            },
            new NodeChoiceMap { nodeId = 14, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 13, choices = new List<int> { 18, -1, 16 } },
                    new EntryChoice { fromNodeId = 18, choices = new List<int> { -1, 16, 13 } },
                    new EntryChoice { fromNodeId = 16, choices = new List<int> { 13, 18 } }
                }
            },
            new NodeChoiceMap { nodeId = 15, entryChoices = new List<EntryChoice>
            {
                new EntryChoice { fromNodeId = 21, choices = new List<int>() }//막다른 길
            } },
            new NodeChoiceMap { nodeId = 16, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 2, choices = new List<int> { 3, 14, 21 } },
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { 14, 21, 2 } },
                    new EntryChoice { fromNodeId = 14, choices = new List<int> { 21, 2, 3 } },
                    new EntryChoice { fromNodeId = 21, choices = new List<int> { 2, 3, 14 } }
                }
            },
            new NodeChoiceMap { nodeId = 17, entryChoices = new List<EntryChoice>
            {
                new EntryChoice { fromNodeId = 20, choices = new List<int>() { 19 }},
                new EntryChoice { fromNodeId = 19, choices = new List<int>() { 20 }}
            } },
            new NodeChoiceMap { nodeId = 18, entryChoices = new List<EntryChoice>
            {
                new EntryChoice { fromNodeId = 14, choices = new List<int>() } //막다른 길 18번
            } },
            new NodeChoiceMap { nodeId = 19, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 13, choices = new List<int> { -1, 20, 17 } },
                    new EntryChoice { fromNodeId = 20, choices = new List<int> { 17, 13 } },
                    new EntryChoice { fromNodeId = 17, choices = new List<int> { 13, -1, 20 } },
                }
            },
            new NodeChoiceMap { nodeId = 20, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 11, choices = new List<int> { -1, 17, 19 } },
                    new EntryChoice { fromNodeId = 19, choices = new List<int> { 11, -1, 17 } },
                    new EntryChoice { fromNodeId = 17, choices = new List<int> { 19, 11 } }
                }
            },
            new NodeChoiceMap { nodeId = 21, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 1, choices = new List<int> { 16, 15 } },
                    new EntryChoice { fromNodeId = 15, choices = new List<int> { -1, 1, 16 } },
                    new EntryChoice { fromNodeId = 16, choices = new List<int> { 15, -1, 1 } }
                }
            },
            new NodeChoiceMap
            {
                nodeId = 22, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 9, choices = new List<int>() } //막다른 길
                }
            },
            new NodeChoiceMap { nodeId = 23, entryChoices = new List<EntryChoice>
            {
                new EntryChoice { fromNodeId = 10, choices = new List<int>() {-1, -100} } //도착!
            } },
        };

        data.edgeWeights = new List<Edge>
        {
            new Edge { from = 0, to = 1, weight = 1 },
            
            new Edge { from = 1, to = 2, weight = 1 },
            new Edge { from = 1, to = 21, weight = 1 },
            
            new Edge { from = 2, to = 3, weight = 1 },
            new Edge { from = 2, to = 1, weight = 1 },
            new Edge { from = 2, to = 16, weight = 1 },
            
            new Edge { from = 3, to = 2, weight = 1 },
            new Edge { from = 3, to = 4, weight = 4 },
            new Edge { from = 3, to = 12, weight = 4 },
            new Edge { from = 3, to = 16, weight = 3 },
            
            new Edge { from = 4, to = 5, weight = 4 },
            new Edge { from = 4, to = 6, weight = 4 },
            new Edge { from = 4, to = 3, weight = 4 },
            
            new Edge { from = 5, to = 8, weight = 1 },
            new Edge { from = 5, to = 6, weight = 1 },
            new Edge { from = 5, to = 4, weight = 4 },
            
            new Edge { from = 6, to = 7, weight = 1 },
            new Edge { from = 6, to = 4, weight = 4 },
            new Edge { from = 6, to = 5, weight = 1 },
            
            new Edge { from = 7, to = 8, weight = 4 },
            new Edge { from = 7, to = 6, weight = 1 },
            
            new Edge { from = 8, to = 9, weight = 1 },
            new Edge { from = 8, to = 7, weight = 4 },
            new Edge { from = 8, to = 5, weight = 1 },
            
            new Edge { from = 9, to = 10, weight = 1 },
            new Edge { from = 9, to = 22, weight = 1 },
            new Edge { from = 9, to = 8, weight = 1 },
            
            new Edge { from = 10, to = 11, weight = 1 },
            new Edge { from = 10, to = 9, weight = 1 },
            new Edge { from = 10, to = 23, weight = 6 },
            
            new Edge { from = 11, to = 12, weight = 6 },
            new Edge { from = 11, to = 10, weight = 1 },
            new Edge { from = 11, to = 20, weight = 1 },
            
            new Edge { from = 12, to = 3, weight = 4 },
            new Edge { from = 12, to = 13, weight = 1 },
            new Edge { from = 12, to = 11, weight = 6 },
            
            new Edge { from = 13, to = 12, weight = 1 },
            new Edge { from = 13, to = 14, weight = 1 },
            new Edge { from = 13, to = 19, weight = 4 },
            
            new Edge { from = 14, to = 13, weight = 1 },
            new Edge { from = 14, to = 16, weight = 1 },
            new Edge { from = 14, to = 18, weight = 1 },
            
            new Edge { from = 15, to = 21, weight = 1 },
            
            new Edge { from = 16, to = 14, weight = 1 },
            new Edge { from = 16, to = 21, weight = 1 },
            new Edge { from = 16, to = 3, weight = 3 },
            new Edge { from = 16, to = 2, weight = 1 },
            
            new Edge { from = 17, to = 19, weight = 4 },
            new Edge { from = 17, to = 20, weight = 4 },
            
            new Edge { from = 18, to = 14, weight = 1 },
            
            new Edge { from = 19, to = 20, weight = 1 },
            new Edge { from = 19, to = 17, weight = 4 },
            new Edge { from = 19, to = 13, weight = 4 },
            
            new Edge { from = 20, to = 19, weight = 1 },
            new Edge { from = 20, to = 11, weight = 1 },
            new Edge { from = 20, to = 17, weight = 4 },
            
            new Edge { from = 21, to = 1, weight = 1 },
            new Edge { from = 21, to = 16, weight = 1 },
            new Edge { from = 21, to = 15, weight = 1 },
            
            new Edge { from = 22, to = 9, weight = 1 },
            
            new Edge { from = 23, to = 10, weight = 6 }
        };

        AssetDatabase.CreateAsset(data, "Assets/ChoiceData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;

        Debug.Log("ChoiceData.asset 생성 완료!");
    }
}
