using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHoverTextColorChanger : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Color _textColorOnHover;

	private TMP_Text _buttonText;
	private Color _defaultColor;

	private bool _isHovered;

	private void Awake()
	{
		_buttonText = GetComponentInChildren<TMP_Text>();	
		_defaultColor = _buttonText.color;
	}

	private void Update()
	{
		if (EventSystem.current?.currentSelectedGameObject == gameObject || _isHovered) 
		{
			_buttonText.color = _textColorOnHover;
		}
		else
		{
			_buttonText.color = _defaultColor;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_buttonText.color = _textColorOnHover;
		_isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_buttonText.color = _defaultColor;
		eventData.selectedObject = null;
		_isHovered = false;
	}
}