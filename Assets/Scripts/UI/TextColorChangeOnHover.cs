using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextColorChangeOnHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Color _textColorOnHover;

	private TMP_Text _text;
	private Color _defaultColor;

	private bool _isHovered;

	private void Awake()
	{
		_text = GetComponentInChildren<TMP_Text>();	
		_defaultColor = _text.color;
	}

	private void Update()
	{
		if (EventSystem.current?.currentSelectedGameObject == gameObject || _isHovered) 
		{
			_text.color = _textColorOnHover;
		}
		else
		{
			_text.color = _defaultColor;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_text.color = _textColorOnHover;
		_isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_text.color = _defaultColor;
		eventData.selectedObject = null;
		_isHovered = false;
	}
}
