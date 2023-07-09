using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectedSetter : MonoBehaviour
{
	private void Start()
	{
		if (EventSystem.current)
		{
			EventSystem.current.firstSelectedGameObject = gameObject;
			EventSystem.current.SetSelectedGameObject(gameObject);
		}
	}
}
