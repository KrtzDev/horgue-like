using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
	public event Action OnButtonSelect;
	public event Action OnButtonDeselect;
	public event Action OnButtonDown;
	public event Action OnButtonUp;
	public event Action OnButtonKeepFocus;

	[field: SerializeField]
	public Button Button { get; private set; }

	[field: SerializeField]
	public TMP_Text ButtonText { get; private set; }

	private ColorBlock _normalColors;
	private ColorBlock _selectedcolors;

	private void Awake()
	{
		_normalColors = Button.colors;

		_selectedcolors.normalColor = Button.colors.selectedColor;
		_selectedcolors.highlightedColor = Button.colors.selectedColor;
		_selectedcolors.pressedColor = Button.colors.selectedColor;
		_selectedcolors.selectedColor = Button.colors.selectedColor;
		_selectedcolors.disabledColor = Button.colors.selectedColor;
		_selectedcolors.colorMultiplier = Button.colors.colorMultiplier;
		_selectedcolors.fadeDuration = Button.colors.fadeDuration;
	}

	public void Enable() => gameObject.SetActive(true);

	public void Disable() => gameObject.SetActive(false);

	public void Select() => Button.Select();

	public void SelectVisualy()
	{
		Button.colors = _selectedcolors;
		OnButtonSelect?.Invoke();
	}

	public void Deselect() => OnButtonDeselect?.Invoke();



	public void Addlistener(UnityAction action) => Button.onClick.AddListener(action);

	public void RemoveListener(UnityAction action) => Button.onClick.RemoveListener(action);

	public virtual void OnSelect(BaseEventData eventData) => OnButtonSelect?.Invoke();

	public virtual void OnDeselect(BaseEventData eventData) => OnButtonDeselect?.Invoke();

	public void OnPointerDown(PointerEventData eventData) => OnButtonDown?.Invoke();

	public void OnPointerUp(PointerEventData eventData) => OnButtonUp?.Invoke();



	public void KeepFocus()
	{
		OnButtonKeepFocus?.Invoke();
		Button.colors = _selectedcolors;
	}

	public void ResetColors() => Button.colors = _normalColors;



#if UNITY_EDITOR
	private void OnValidate()
	{
		if (!Button) Button = GetComponent<Button>();
		if (!ButtonText) ButtonText = GetComponentInChildren<TMP_Text>();
	}

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
#endif
}
