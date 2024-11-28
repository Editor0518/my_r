/*
[CustomEditor(typeof(StoryBlock)), CanEditMultipleObjects]
public class BlockInspectorEditor : Editor
{
    //SerializedProperty property;
    // property;
    //StoryBlock enemyScript;
    void OnEnable()
    {
        //var property = serializedObject.FindProperty("itemBlock");
        //enemyScript = target as StoryBlock;
    }


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
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.choiceName"));

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.moveTo"));
                    // EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.choiceCmdOnWhen"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.choiceCmdAfter"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceA.clip"));
                    EditorGUILayout.LabelField("*** Choice B ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.choiceName"));

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.moveTo"));
                    // EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.choiceCmdOnWhen"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.choiceCmdAfter"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceB.clip"));
                    EditorGUILayout.LabelField("*** Choice C ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.choiceName"));

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.moveTo"));
                    //  EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.choiceCmdOnWhen"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.choiceCmdAfter"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceC.clip"));
                    //EditorGUILayout.PropertyField(serializedObject.FindProperty("choice"));
                    EditorGUILayout.LabelField("*** Choice D ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceD.type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceD.choiceName"));

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceD.moveTo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceD.choiceCmdOnWhen"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceD.choiceCmdAfter"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceD.clip"));

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
            case if_end.GIFT:
                {
                    //                    EditorGUILayout.LabelField("*** Item Blocks ***", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock"));
                }
                break;
            case if_end.MOVECMD:
                {

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock"));
                    EditorGUILayout.LabelField("작성 가이드 Item Name : SavedVarName==value", EditorStyles.label);
                }
                break;
            default:
                {

                }
                break;
        }


        // CarInfo는 클래스이므로 '_carInfo.name' 형태로 클래스 내부에 접근할 수 있습니다.
        // 프로퍼티를 가져옴과 동시에 Inspector에 노출시켜 줍니다.

*/
/*for (int i = 0; i < serializedObject.FindProperty("itemBlock").arraySize; i++)
{
    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock"), includeChildren: true);
    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock.itemName"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock.newBlock"));
}*/


//DrawList(serializedObject.FindProperty("itemBlock"), "원소 이름");
/*
// 변경된 프로퍼티를 저장해줍니다.
serializedObject.ApplyModifiedProperties();

if (GUI.changed)
{
    EditorUtility.SetDirty(target);
}
}

public void DrawList(SerializedProperty _listProperty, string _labalName)
{
if (_listProperty.isExpanded = EditorGUILayout.Foldout(_listProperty.isExpanded, _listProperty.name))
{
    EditorGUILayout.PropertyField(_listProperty.FindPropertyRelative("Array.size"));
    int Count = _listProperty.arraySize;
    for (int i = 0; i < Count; ++i)
    {
        //EditorGUILayout.PropertyField(_listProperty.GetArrayElementAtIndex(i), new GUIContent(_labalName + i));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock"), includeChildren: true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock.itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemBlock.newBlock"));
    }
}
}

}
*/