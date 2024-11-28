using UnityEditor;
using UnityEngine;


//[CustomPropertyDrawer(typeof(ItemBlock))]
public class BlockInspectorDrawer : PropertyDrawer
{
    private SerializedProperty itemName;
    private SerializedProperty newBlock;

    //인스펙터에 어떻게 그릴지
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //fill our properties
        itemName = property.FindPropertyRelative("itemName");
        newBlock = property.FindPropertyRelative("newBlock");



        //drawing instructions here
        Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        if (property.isExpanded)
        {
            //draw our properties
            DrawItemNameProperty(position);
            DrawNewBlockProperty(position);
        }

        EditorGUI.EndProperty();
        //base.OnGUI(position, property, label);

    }

    //request more vertical spacing, return it
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLines = 1;

        //increase out height if we expand arrow
        if (property.isExpanded)
        {
            totalLines += 2;
        }

        return EditorGUIUtility.singleLineHeight * totalLines;
    }

    private void DrawItemNameProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, itemName, new GUIContent("Item Name"));
    }

    private void DrawNewBlockProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 2f;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, newBlock, new GUIContent("New Block"));
    }


}
