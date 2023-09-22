
using UnityEngine.EventSystems;
using UnityEngine;

public class DefaultButton : UIButton, IPointerExitHandler, IPointerEnterHandler
{

	public void OnPointerExit(PointerEventData eventData)
	{
		this.Select();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameObject currentselection = EventSystem.current.currentSelectedGameObject;

		if (currentselection != null && currentselection != this.gameObject)
			EventSystem.current.SetSelectedGameObject(null);

		this.Select();
	}
}
