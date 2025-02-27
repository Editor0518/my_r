using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Item))]
public class ItemInspectorDrawer : PropertyDrawer
{
    private SerializedProperty nameForFind;
    private SerializedProperty nameForShow;
    private SerializedProperty sprite;
    private SerializedProperty content;
    private SerializedProperty isCollected;
    private SerializedProperty canUse;
    private SerializedProperty isTrash;
    private SerializedProperty afterUsed;


    //인스펙터에 어떻게 그릴지
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //fill our properties
        nameForFind = property.FindPropertyRelative("nameForFind");
        nameForShow = property.FindPropertyRelative("nameForShow");
        sprite = property.FindPropertyRelative("sprite");
        content = property.FindPropertyRelative("content");
        isCollected = property.FindPropertyRelative("isCollected");
        canUse = property.FindPropertyRelative("canUse");
        isTrash = property.FindPropertyRelative("isTrash");
        afterUsed = property.FindPropertyRelative("afterUsed");


        //drawing instructions here
        Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        if (property.isExpanded)
        {
            //draw our properties
            DrawNameFindProperty(position);
            DrawNameProperty(position);
            DrawSpriteProperty(position);
            DrawContentProperty(position);

            DrawCanUseProperty(position);

            DrawIsTrashProperty(position);
            if (canUse.boolValue) DrawAfterUsedProperty(position);
            DrawIsCollectedProperty(position);
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
            totalLines += 8;
            totalLines += 2;
        }


        return EditorGUIUtility.singleLineHeight * totalLines;
    }

    private void DrawNameProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 2;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, nameForShow, new GUIContent("NameForShow"));
    }

    private void DrawNameFindProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, nameForFind, new GUIContent("NameForFind"));
    }


    private void DrawSpriteProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 3;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, sprite, new GUIContent("Sprite"));
    }

    private void DrawContentProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 4;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight * 4;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, content, new GUIContent("Content"));
    }


    private void DrawCanUseProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 8;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, canUse, new GUIContent("Can Use"));
    }

    private void DrawAfterUsedProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 9;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, afterUsed, new GUIContent("After Used"));
    }


    private void DrawIsTrashProperty(Rect position)
    {
        float xPos = position.min.x + position.size.x / 2;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * 8;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, isTrash, new GUIContent("Is Trash"));
    }
    private void DrawIsCollectedProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight * (canUse.boolValue ? 10 : 9);
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, isCollected, new GUIContent("Is Collected"));
    }

}
