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


	private void OnEnable()
	{
		_holdAction.action.Enable();

		_holdAction.action.started += (_) => StartHold();
		_holdAction.action.canceled += (_) => CancleHold();

		OnButtonDown += StartHold;
		OnButtonUp += CancleHold;
	}

	private void Awake()
	{
		_holdBar.fillAmount = 0;
	}

	private void OnDisable()
	{
		_holdAction.action.Disable();

		_holdAction.action.started -= (_) => StartHold();
		_holdAction.action.canceled -= (_) => CancleHold();

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
		while (_holdBar.fillAmount < 1)
		{
			_holdBar.fillAmount += Time.deltaTime / _holdTime;
			yield return null;
		}
		_holdBar.fillAmount = 1;

		OnButtonExecute?.Invoke();
	}

	private void CancleHold() 
	{
		StopAllCoroutines();
		_holdBar.fillAmount = 0;
	}
}
