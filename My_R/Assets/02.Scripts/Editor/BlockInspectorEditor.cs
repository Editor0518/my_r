using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(StoryBlock))]
public class BlockInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        // character 필드를 가져옵니다.
        var if_end = (if_end)serializedObject.FindProperty("ifEnd").intValue;
        // character 필드를 Inspector에 노출 시켜줍니다.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ifEnd"));

        // Inspector에서 ObjectType Enum을 변경하게 되면 해당 타입에 맞게 노출시켜줄 필드를 정의해줍니다.
        switch (if_end)
        {
            case if_end.CHOICE:
                {
                    // CarInfo는 클래스이므로 '_carInfo.name' 형태로 클래스 내부에 접근할 수 있습니다.
                    // 프로퍼티를 가져옴과 동시에 Inspector에 노출시켜 줍니다.
                    EditorGUILayout.LabelField("*** Choice A ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.choiceName"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.moveTo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.clip"));
                    EditorGUILayout.LabelField("*** Choice B ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.choiceName"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.moveTo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.clip"));
                    EditorGUILayout.LabelField("*** Choice C ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.choiceName"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.moveTo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.clip"));
                    //EditorGUILayout.PropertyField(serializedObject.FindProperty("choice"));

                }
                break;

            case if_end.HEART:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("heartAdd"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("newBlock"));

                }
                break;
            case if_end.NEW:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("newBlock"));

                }
                break;
            case if_end.MINIGAME:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("minigameObj"));

                }
                break;
            default:
                {

                }
                break;
        }

        // 변경된 프로퍼티를 저장해줍니다.
        serializedObject.ApplyModifiedProperties();
    }
}
