using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class UIButton : MonoBehaviour, ISelectHandler
{
	public event Action OnButtonSelect;

	[field: SerializeField]
	public Image Image { get; private set; }

	[field: SerializeField]
	public Button Button { get; private set; }

	[field: SerializeField]
	public TMP_Text ButtonText { get; private set; }


	public void Select() => Button.Select();

	public void Addlistener(UnityAction action) => Button.onClick.AddListener(action);

	public void RemoveListener(UnityAction action) => Button.onClick.RemoveListener(action);

	public void OnSelect(BaseEventData eventData) => OnButtonSelect.Invoke();



#if UNITY_EDITOR
	private void OnValidate()
	{
		if (!Image) Image = GetComponent<Image>();
		if (!Button) Button = GetComponent<Button>();
		if (!ButtonText) ButtonText = GetComponentInChildren<TMP_Text>();
	}
#endif
}
