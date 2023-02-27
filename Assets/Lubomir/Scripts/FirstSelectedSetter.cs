using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectedSetter : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		EventSystem.current.firstSelectedGameObject = gameObject;
		EventSystem.current.SetSelectedGameObject(gameObject);
	}
}
