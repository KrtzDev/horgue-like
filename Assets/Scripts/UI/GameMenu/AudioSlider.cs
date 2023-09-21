using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private TextMeshProUGUI _valueText;
	private Slider _slider;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
	}

	public void OnSliderChange()
	{
		_valueText.text = _slider.value.ToString();

		if (_slider.value == 0)
		{
			AudioManager.Instance.SetAudioVolume(-1);
		}
		else
		{
			AudioManager.Instance.SetAudioVolume(_slider.value * 4 / 100);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameObject currentselection = EventSystem.current.currentSelectedGameObject;

		if (currentselection != null && currentselection != this.gameObject)
			EventSystem.current.SetSelectedGameObject(null);

		_slider.Select();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StartCoroutine(SelectAfterFrame());
	}

	private IEnumerator SelectAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		_slider.Select();
	}
}
