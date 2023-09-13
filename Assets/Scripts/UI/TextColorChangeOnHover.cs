using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TextColorChangeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Color _textColorOnHover;

	private TMP_Text[] _texts;
	private Color[] _defaultColors;

	private bool _isHovered;

	private void Awake()
	{
		_texts = GetComponentsInChildren<TMP_Text>();
		_defaultColors = new Color[_texts.Length];

		for(int i = 0; i < _texts.Length; i++)
        {
			_defaultColors[i] = _texts[i].color;
		}
	}

	private void Update()
	{
		if (EventSystem.current?.currentSelectedGameObject == gameObject || _isHovered) 
		{
			for (int i = 0; i < _texts.Length; i++)
			{
				_texts[i].color = _textColorOnHover;
			}
		}
		else
		{
			for (int i = 0; i < _texts.Length; i++)
			{
				_texts[i].color = _defaultColors[i];
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		for (int i = 0; i < _texts.Length; i++)
		{
			_texts[i].color = _textColorOnHover;
		}
		_isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		for (int i = 0; i < _texts.Length; i++)
		{
			_texts[i].color = _defaultColors[i];
		}

		eventData.selectedObject = null;
		_isHovered = false;
	}
}
