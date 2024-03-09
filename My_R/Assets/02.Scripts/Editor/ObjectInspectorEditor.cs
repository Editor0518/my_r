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

        // _objectType 필드를 가져옵니다.
        var objectType = (ObjectType)serializedObject.FindProperty("_objectType").intValue;
        // _objectType 필드를 Inspector에 노출 시켜줍니다.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_objectType"));

        // Inspector에서 ObjectType Enum을 변경하게 되면 해당 타입에 맞게 노출시켜줄 필드를 정의해줍니다.
        switch (objectType)
        {
            case ObjectType.CAR:
                {
                    // CarInfo는 클래스이므로 '_carInfo.name' 형태로 클래스 내부에 접근할 수 있습니다.
                    // 프로퍼티를 가져옴과 동시에 Inspector에 노출시켜 줍니다.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_carInfo.name"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_carInfolist"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_carInfo.speed"));
                }
                break;

            case ObjectType.MONSTER:
                {
                    // MonsterInfo는 클래스이므로 '_monsterInfo.name' 형태로 클래스 내부에 접근할 수 있습니다.
                    // 프로퍼티를 가져옴과 동시에 Inspector에 노출시켜 줍니다.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.name"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.power"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.hp"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_monsterInfo.mp"));
                }
                break;
        }

        // 변경된 프로퍼티를 저장해줍니다.
        serializedObject.ApplyModifiedProperties();
    }
}