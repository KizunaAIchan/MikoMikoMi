using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomText), true)]
[CanEditMultipleObjects]
public class CustomTextEditor : TextEditor
{
    private SerializedProperty _isLanguageText;
    private SerializedProperty LanguageId;

    protected override void OnEnable()
    {
        base.OnEnable();
        _isLanguageText = serializedObject.FindProperty("isLanguageText");
        LanguageId = serializedObject.FindProperty("LanguageId");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_isLanguageText);
        EditorGUILayout.PropertyField(LanguageId);
        serializedObject.ApplyModifiedProperties();
    }
}
