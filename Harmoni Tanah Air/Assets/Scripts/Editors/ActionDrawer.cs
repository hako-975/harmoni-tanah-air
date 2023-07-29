using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StoryScene.Sentence.Action))]
public class ActionDrawer : PropertyDrawer
{
    private const int LineHeight = 20;
    private const int ExtraRows = 2; // Number of extra rows for the ActionType enum

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty actionType = property.FindPropertyRelative("actionType");
            Rect typeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
            actionType.enumValueIndex = EditorGUI.Popup(typeRect, "Action Type", actionType.enumValueIndex, actionType.enumDisplayNames);

            // Draw Speaker and Sprite fields
            SerializedProperty speaker = property.FindPropertyRelative("speaker");
            SerializedProperty sprite = property.FindPropertyRelative("sprite");
            position.y += LineHeight; // Move down to add space at the top
            DrawFieldWithLabel(ref position, speaker);
            DrawFieldWithLabel(ref position, sprite);

            switch ((StoryScene.Sentence.Action.ActionType)actionType.enumValueIndex)
            {
                case StoryScene.Sentence.Action.ActionType.NONE:
                case StoryScene.Sentence.Action.ActionType.APPEAR:
                    DrawCoordsField(ref position, property);
                    break;
                case StoryScene.Sentence.Action.ActionType.MOVE:
                    DrawMoveFields(ref position, property);
                    break;
                case StoryScene.Sentence.Action.ActionType.DISAPPEAR:
                    DrawDurationField(ref position, property);
                    break;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    private void DrawFieldWithLabel(ref Rect position, SerializedProperty property)
    {
        Rect fieldRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(fieldRect, property);

        position.y += LineHeight;
    }

    private void DrawCoordsField(ref Rect position, SerializedProperty property)
    {
        SerializedProperty coords = property.FindPropertyRelative("coords");
        DrawFieldWithLabel(ref position, coords);
    }

    private void DrawMoveFields(ref Rect position, SerializedProperty property)
    {
        SerializedProperty startCoords = property.FindPropertyRelative("startCoords");
        SerializedProperty endCoords = property.FindPropertyRelative("endCoords");
        SerializedProperty speed = property.FindPropertyRelative("speed");

        DrawFieldWithLabel(ref position, startCoords);
        DrawFieldWithLabel(ref position, endCoords);
        DrawFieldWithLabel(ref position, speed);
    }

    private void DrawDurationField(ref Rect position, SerializedProperty property)
    {
        SerializedProperty duration = property.FindPropertyRelative("duration");
        DrawFieldWithLabel(ref position, duration);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        SerializedProperty actionType = property.FindPropertyRelative("actionType");
        StoryScene.Sentence.Action.ActionType selectedType = (StoryScene.Sentence.Action.ActionType)GetActionType(property);

        int fieldCount = 2; // Foldout and ActionType enum

        switch (selectedType)
        {
            case StoryScene.Sentence.Action.ActionType.NONE:
            case StoryScene.Sentence.Action.ActionType.APPEAR:
                fieldCount += 1; // Coords field
                break;
            case StoryScene.Sentence.Action.ActionType.MOVE:
                fieldCount += 3; // StartCoords, EndCoords, Speed fields
                break;
            case StoryScene.Sentence.Action.ActionType.DISAPPEAR:
                fieldCount += 1; // Duration field
                break;
        }

        // Add extra rows for ActionType enum
        fieldCount += ExtraRows;

        return fieldCount * LineHeight;
    }

    private int GetActionType(SerializedProperty property)
    {
        SerializedProperty actionType = property.FindPropertyRelative("actionType");
        return actionType.enumValueIndex;
    }
}
