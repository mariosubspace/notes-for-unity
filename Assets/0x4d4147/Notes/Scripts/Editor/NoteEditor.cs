using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoteBehaviour))]
public class NotesEditor : Editor
{
    private SerializedProperty type;
    private SerializedProperty note;

    private void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        note = serializedObject.FindProperty("note");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(type);

        SetGUIColorForType((NoteType)type.enumValueIndex);

        EditorGUILayout.PropertyField(note);

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    void SetGUIColorForType(NoteType type)
    {
        switch (type)
        {
            case NoteType.Memo:
                {
                    GUI.backgroundColor = Color.black;
                    break;
                }
            case NoteType.ToDo:
                {
                    GUI.backgroundColor = Color.blue;
                    break;
                }
            case NoteType.Bug:
                {
                    GUI.backgroundColor = Color.red;
                    break;
                }
        }
    }
}
