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
            new NodeChoiceMap { nodeId = 0, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 1, choices = new List<int>() } } },
            new NodeChoiceMap { nodeId = 1, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 2, choices = new List<int> { 0, 21, 15 } } } },
            new NodeChoiceMap { nodeId = 2, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 3, choices = new List<int> { 1, 16, 14 } } } },
            new NodeChoiceMap { nodeId = 3, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 2, choices = new List<int> { 16, 4, 12 } },
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 12, 16, 2 } },
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 2, 4, 16 } },
                    new EntryChoice { fromNodeId = 16, choices = new List<int> { 2, 4, 12 } },
                }
            },
            new NodeChoiceMap { nodeId = 4, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { 5, 6, 12 } },
                    new EntryChoice { fromNodeId = 5, choices = new List<int> { 6, 12, 3 } },
                    new EntryChoice { fromNodeId = 6, choices = new List<int> { 12, 3, 5 } },
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 3, 5, 6 } },
                }
            },
            new NodeChoiceMap { nodeId = 5, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 8, 6, 3 } },
                    new EntryChoice { fromNodeId = 6, choices = new List<int> { 4, 8, 9 } },
                    new EntryChoice { fromNodeId = 8, choices = new List<int> { 6, 4, 9 } },
                    new EntryChoice { fromNodeId = 9, choices = new List<int> { 4, 6, 8 } },
                }
            },
            new NodeChoiceMap { nodeId = 6, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 5, choices = new List<int> { 7, 4, 8 } },
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 5, 7 } },
                    new EntryChoice { fromNodeId = 7, choices = new List<int> { 4, 5 } },
                }
            },
            new NodeChoiceMap { nodeId = 7, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 6, choices = new List<int>() } } },
            new NodeChoiceMap { nodeId = 8, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 5, choices = new List<int> { 9, 22 } },
                    new EntryChoice { fromNodeId = 9, choices = new List<int> { 22, 5 } },
                    new EntryChoice { fromNodeId = 22, choices = new List<int> { 5, 9 } },
                }
            },
            new NodeChoiceMap { nodeId = 9, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 8, choices = new List<int> { 10, 22, 5 } },
                    new EntryChoice { fromNodeId = 22, choices = new List<int> { 8, 10 } },
                    new EntryChoice { fromNodeId = 10, choices = new List<int> { 22, 5, 8 } },
                }
            },
            new NodeChoiceMap { nodeId = 10, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 9, choices = new List<int> { 11, 22, 23 } },
                    new EntryChoice { fromNodeId = 22, choices = new List<int> { 23, 11, 9 } },
                    new EntryChoice { fromNodeId = 23, choices = new List<int> { 9, 22, 11 } },
                    new EntryChoice { fromNodeId = 11, choices = new List<int> { 23, 9, 22 } },
                }
            },
            new NodeChoiceMap { nodeId = 11, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 10, choices = new List<int> { 12, 20 } },
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 20, 10 } },
                    new EntryChoice { fromNodeId = 20, choices = new List<int> { 10, 12 } },
                }
            },
            new NodeChoiceMap { nodeId = 12, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { 13, 11, 4 } },
                    new EntryChoice { fromNodeId = 4, choices = new List<int> { 11, 3, 13 } },
                    new EntryChoice { fromNodeId = 11, choices = new List<int> { 3, 13, 4 } },
                    new EntryChoice { fromNodeId = 13, choices = new List<int> { 4, 3, 11 } },
                }
            },
            new NodeChoiceMap { nodeId = 13, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 12, choices = new List<int> { 14, 19, 3 } },
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { 19, 14, 12 } },
                    new EntryChoice { fromNodeId = 14, choices = new List<int> { 12, 3, 19 } },
                    new EntryChoice { fromNodeId = 19, choices = new List<int> { 3, 12, 14 } },
                }
            },
            new NodeChoiceMap { nodeId = 14, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 13, choices = new List<int> { 18, 16, 21 } },
                    new EntryChoice { fromNodeId = 18, choices = new List<int> { 21, 13, 16 } },
                    new EntryChoice { fromNodeId = 16, choices = new List<int> { 13, 21, 18 } },
                    new EntryChoice { fromNodeId = 21, choices = new List<int> { 16, 18, 13 } },
                }
            },
            new NodeChoiceMap { nodeId = 15, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 21, choices = new List<int>() } } },
            new NodeChoiceMap { nodeId = 16, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 14, choices = new List<int> { 21, 2, 3 } },
                    new EntryChoice { fromNodeId = 2, choices = new List<int> { 14, 21, 3 } },
                    new EntryChoice { fromNodeId = 3, choices = new List<int> { 2, 14, 21 } },
                    new EntryChoice { fromNodeId = 21, choices = new List<int> { 3, 2, 14 } },
                }
            },
            new NodeChoiceMap { nodeId = 17, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 20, choices = new List<int>() } } },
            new NodeChoiceMap { nodeId = 18, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 14, choices = new List<int>() } } },
            new NodeChoiceMap { nodeId = 19, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 13, choices = new List<int> { 20, 17, 14 } },
                    new EntryChoice { fromNodeId = 20, choices = new List<int> { 17, 13 } },
                    new EntryChoice { fromNodeId = 17, choices = new List<int> { 13, 20 } },
                }
            },
            new NodeChoiceMap { nodeId = 20, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 11, choices = new List<int> { 19, 17, 10 } },
                    new EntryChoice { fromNodeId = 19, choices = new List<int> { 11, 17 } },
                    new EntryChoice { fromNodeId = 10, choices = new List<int> { 11, 19, 17 } },
                    new EntryChoice { fromNodeId = 17, choices = new List<int> { 10, 11, 19 } },
                }
            },
            new NodeChoiceMap { nodeId = 21, entryChoices = new List<EntryChoice>
                {
                    new EntryChoice { fromNodeId = 1, choices = new List<int> { 15, 16, 14 } },
                    new EntryChoice { fromNodeId = 14, choices = new List<int> { 1, 15, 16 } },
                    new EntryChoice { fromNodeId = 15, choices = new List<int> { 16, 1, 14 } },
                    new EntryChoice { fromNodeId = 16, choices = new List<int> { 14, 1, 15 } },
                }
            },
            new NodeChoiceMap { nodeId = 22, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 9, choices = new List<int>() } } },
            new NodeChoiceMap { nodeId = 23, entryChoices = new List<EntryChoice> { new EntryChoice { fromNodeId = 10, choices = new List<int>() } } },
        };

        AssetDatabase.CreateAsset(data, "Assets/ChoiceData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;

        Debug.Log("ChoiceData.asset 생성 완료!");
    }
}
