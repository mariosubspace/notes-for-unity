using UnityEngine;

[AddComponentMenu("0x4d4147/NoteBehaviour")]
public class NoteBehaviour : MonoBehaviour
{
	[TextArea(1, 5)]
	public string note;
}