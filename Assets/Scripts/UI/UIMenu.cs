using UnityEngine;

public class UIMenu : MonoBehaviour
{
	public CanvasGroup CanvasGroup => _canvasGroup;

	[SerializeField]
	public CanvasGroup _canvasGroup;

	public virtual void SetFocusedMenu()
	{

	}
}
