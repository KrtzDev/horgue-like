using System;
using UnityEngine;

[Serializable]
public class Wallet
{
	public event Action<int> OnMoneyChanged;

	[SerializeField]
	private int _money;

	public void Reset()
	{
		_money = 0;
	}

	public int GetMoneyAmount()
	{
		return _money;
	}

	public void Store(int value)
	{
		_money += value;
		OnMoneyChanged?.Invoke(_money);
	}

	public bool TryPay(int value)
	{
		if (_money - value < 0)
			return false;

		Pay(value);
		return true;

	}

	private void Pay(int value)
	{
		_money -= value;
		OnMoneyChanged?.Invoke(_money);
	}
}