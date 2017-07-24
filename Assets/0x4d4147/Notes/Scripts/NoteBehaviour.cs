using UnityEngine;

[AddComponentMenu("0x4d4147/NoteBehaviour")]
public class NoteBehaviour : MonoBehaviour
{
    [Space(10)]
    public NoteType type;

	[TextArea(1, 5), Space(10)]
	public string note;
}

public enum NoteType
{
    Memo,
    ToDo,
    Bug
}