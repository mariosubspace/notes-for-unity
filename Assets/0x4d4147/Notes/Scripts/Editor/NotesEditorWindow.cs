using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class NotesEditorWindow : EditorWindow
{
    Dictionary<NoteType, List<NoteBehaviour>> notes;
    NoteType[] noteTypes;

	Vector2 scrollPosition;

	GUIStyle memoNoteStyle;
    GUIStyle todoNoteStyle;
    GUIStyle bugNoteStyle;

    bool isMemoNoteBoxOpen = true;
    bool isTodoNoteBoxOpen = true;
    bool isBugNoteBoxOpen = true;

	[MenuItem("Window/Gather Notes")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		NotesEditorWindow window = (NotesEditorWindow)EditorWindow.GetWindow<NotesEditorWindow>("Gather Notes"); 
		window.Show();
	}

	void OnEnable()
	{
        memoNoteStyle = ConstructNoteStyle(Color.white);
        todoNoteStyle = ConstructNoteStyle(Color.white);
        bugNoteStyle = ConstructNoteStyle(Color.white);

        ConstructNoteDictionary();
    }

    GUIStyle ConstructNoteStyle(Color textColor)
    {
        GUIStyle style;
        style = new GUIStyle();
        style.wordWrap = true;
        style.padding = new RectOffset(10, 10, 5, 5);
        style.richText = true;
        style.normal.textColor = textColor;
        return style;
    }

    GUIStyle ConstructFoldoutStyle(Color textColor)
    {
        GUIStyle style;
        style = new GUIStyle(EditorStyles.foldout);
        style.normal.textColor = textColor;
        return style;
    }

    void ConstructNoteDictionary()
    {
        notes = new Dictionary<NoteType, List<NoteBehaviour>>();
        noteTypes = (NoteType[])Enum.GetValues(typeof(NoteType));
        for (int i = 0; i < noteTypes.Length; ++i)
        {
            notes.Add(noteTypes[i], new List<NoteBehaviour>());
        }
    }

    void OnGUI()
	{
		GUILayout.Space(18);

		if (GUILayout.Button("Refresh Notes"))
		{
			GatherNotes();
		}

		GUILayout.Space(10);

		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
		for (int i = 0; i < noteTypes.Length; ++i)
		{
            GUILayout.Space(10);
            DrawNoteBox(noteTypes[i]);
		}

		EditorGUILayout.EndScrollView();
	}

    void DrawNoteBox(NoteType noteType)
    {
        switch (noteType)
        {
            case NoteType.Memo:
                {
                    DrawNoteBox_Generic(noteType, ref isMemoNoteBoxOpen, "Memos", memoNoteStyle, Color.black);
                    break;
                }
            case NoteType.ToDo:
                {
                    DrawNoteBox_Generic(noteType, ref isTodoNoteBoxOpen, "To Dos", todoNoteStyle, Color.blue);
                    break;
                }
            case NoteType.Bug:
                {
                    DrawNoteBox_Generic(noteType, ref isBugNoteBoxOpen, "Bugs", bugNoteStyle, Color.red);
                    break;
                }
        }
    }
    
    void DrawNoteBox_Generic(NoteType noteType, ref bool isFoldoutOpen, string foldoutLabel, GUIStyle noteStyle, Color backgroundColor)
    {
        var notes = this.notes[noteType];

        if (notes.Count == 0) return;

        if (isFoldoutOpen = EditorGUILayout.Foldout(isFoldoutOpen, foldoutLabel))
        {
            GUI.backgroundColor = backgroundColor;
            for (int i = 0; i < notes.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (GUILayout.Button(notes[i].note, noteStyle))
                {
                    Selection.activeObject = notes[i].gameObject;
                    EditorGUIUtility.PingObject(notes[i].gameObject);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUI.backgroundColor = Color.white;
        }
    }

    void ClearNotes()
    {
        foreach (var noteList in notes.Values)
        {
            noteList.Clear();
        }
    }

	void GatherNotes()
	{
        ClearNotes();

		GameObject[] gobs = FindObjectsOfType<GameObject>();
		foreach (var gob in gobs)
		{
			NoteBehaviour[] nbs = gob.GetComponents<NoteBehaviour>();
			if (nbs != null)
			{
				foreach (var nb in nbs)
				{
                    notes[nb.type].Add(nb);
				}
			}
		}
	}
}
