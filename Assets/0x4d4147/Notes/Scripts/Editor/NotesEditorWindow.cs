using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

public class NotesEditorWindow : EditorWindow
{
	NoteBehaviour[] notes;

	Vector2 scrollPosition;
	GUIStyle noteStyle;

	[MenuItem("Window/Gather Notes")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		NotesEditorWindow window = (NotesEditorWindow)EditorWindow.GetWindow<NotesEditorWindow>("Gather Notes"); 
		window.Show();
	}

	void OnEnable()
	{
		noteStyle = new GUIStyle();
		noteStyle.wordWrap = true;
		noteStyle.padding = new RectOffset(10, 10, 5, 5);
		noteStyle.richText = true;
		noteStyle.normal.textColor = Color.white;
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

		if (notes != null)
		{
			for (int i = 0; i < notes.Length; ++i)
			{
				EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
				if (GUILayout.Button(notes[i].note, noteStyle)) 
				{
					Selection.activeObject = notes[i].gameObject;
					EditorGUIUtility.PingObject(notes[i].gameObject);
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		EditorGUILayout.EndScrollView();
	}

	void GatherNotes()
	{
		List<NoteBehaviour> notes = new List<NoteBehaviour>();
		GameObject[] gobs = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (var gob in gobs)
		{
			NoteBehaviour[] nbs = gob.GetComponents<NoteBehaviour>();
			if (nbs != null)
			{
				foreach (var nb in nbs)
				{
					notes.Add(nb);
				}
			}
		}
		this.notes = notes.ToArray();
	}
}
