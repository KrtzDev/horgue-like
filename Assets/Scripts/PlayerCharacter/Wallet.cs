using System;
using UnityEngine;

[Serializable]
public class Wallet
{
	public event Action<int> OnMoneyChanged;

	[SerializeField]
	private float _money;

	public void Reset()
	{
		_money = 0;
	}

	public int GetMoneyAmount()
	{
		return Mathf.RoundToInt(_money-0.5f);
	}

	public void Store(int value)
	{
		_money += value;
		OnMoneyChanged?.Invoke((int)_money);
	}

	public bool TryPay(int value)
	{
		if (_money - value <= 0)
			return false;

		Pay(value);
		return true;
	}

	private void Pay(int value)
	{
		_money -= value;
		OnMoneyChanged?.Invoke((int)_money);
	}
}