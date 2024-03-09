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

        // character �ʵ带 �����ɴϴ�.
        var if_end = (if_end)serializedObject.FindProperty("ifEnd").intValue;
        // character �ʵ带 Inspector�� ���� �����ݴϴ�.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ifEnd"));

        // Inspector���� ObjectType Enum�� �����ϰ� �Ǹ� �ش� Ÿ�Կ� �°� ��������� �ʵ带 �������ݴϴ�.
        switch (if_end)
        {
            case if_end.CHOICE:
                {
                    // CarInfo�� Ŭ�����̹Ƿ� '_carInfo.name' ���·� Ŭ���� ���ο� ������ �� �ֽ��ϴ�.
                    // ������Ƽ�� �����Ȱ� ���ÿ� Inspector�� ������� �ݴϴ�.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choice"));

                }
                break;

            case if_end.HEART:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("heartAdd"));

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

        // ����� ������Ƽ�� �������ݴϴ�.
        serializedObject.ApplyModifiedProperties();
    }
}
