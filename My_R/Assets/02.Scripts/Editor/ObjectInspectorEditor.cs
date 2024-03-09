using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ObjectInspector))]
public class ObjectInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        // _objectType �ʵ带 �����ɴϴ�.
        var objectType = (ObjectType)serializedObject.FindProperty("_objectType").intValue;
        // _objectType �ʵ带 Inspector�� ���� �����ݴϴ�.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_objectType"));

        // Inspector���� ObjectType Enum�� �����ϰ� �Ǹ� �ش� Ÿ�Կ� �°� ��������� �ʵ带 �������ݴϴ�.
        switch (objectType)
        {
            case ObjectType.CAR:
                {
                    // CarInfo�� Ŭ�����̹Ƿ� '_carInfo.name' ���·� Ŭ���� ���ο� ������ �� �ֽ��ϴ�.
                    // ������Ƽ�� �����Ȱ� ���ÿ� Inspector�� ������� �ݴϴ�.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_carInfo.name"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_carInfolist"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_carInfo.speed"));
                }
                break;

            case ObjectType.MONSTER:
                {
                    // MonsterInfo�� Ŭ�����̹Ƿ� '_monsterInfo.name' ���·� Ŭ���� ���ο� ������ �� �ֽ��ϴ�.
                    // ������Ƽ�� �����Ȱ� ���ÿ� Inspector�� ������� �ݴϴ�.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.name"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.power"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.hp"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.mp"));
                }
                break;
        }

        // ����� ������Ƽ�� �������ݴϴ�.
        serializedObject.ApplyModifiedProperties();
    }
}