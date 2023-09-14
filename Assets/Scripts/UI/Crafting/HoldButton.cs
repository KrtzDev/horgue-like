using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldButton : UIButton
{
	public event Action OnButtonExecute;

	[SerializeField]
	private InputActionReference _holdAction;

	[SerializeField]
	private Image _holdBar;

	[SerializeField]
	private float _holdTime;

	private bool _canExecute = true;

	private void OnEnable()
	{
		_holdAction.action.Enable();

		_holdAction.action.started += ButtonPressed;
		_holdAction.action.canceled += ButtonReleased;

		OnButtonDown += StartHold;
		OnButtonUp += CancleHold;
	}

	private void ButtonReleased(InputAction.CallbackContext ctx) => CancleHold();

	private void ButtonPressed(InputAction.CallbackContext ctx) => StartHold();

	private void Awake()
	{
		_holdBar.fillAmount = 0;
	}

	private void OnDisable()
	{
		_holdAction.action.started -= ButtonPressed;
		_holdAction.action.canceled -= ButtonReleased;

		OnButtonDown -= StartHold;
		OnButtonUp -= CancleHold;
	}

	private void StartHold()
	{
		_holdBar.fillAmount = 0;
		StopAllCoroutines();
		StartCoroutine(FillHoldBar());
	}

	private IEnumerator FillHoldBar()
	{
		if (_canExecute)
		{
			while (_holdBar.fillAmount < 1)
			{
				_holdBar.fillAmount += Time.deltaTime / _holdTime;
				yield return null;
			}
			_holdBar.fillAmount = 1;

			_canExecute = false;
			OnButtonExecute?.Invoke();
			AudioManager.Instance.PlaySound("ButtonSelect");
			_holdBar.fillAmount = 0;
		}
	}

	private void CancleHold()
	{
		StopAllCoroutines();
		_holdBar.fillAmount = 0;
		_canExecute = true;
	}
}
