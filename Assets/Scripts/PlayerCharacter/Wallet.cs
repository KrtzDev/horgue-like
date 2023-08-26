using System;
using UnityEngine;

[Serializable]
public class Wallet
{
	public event Action<int> OnMoneyChanged;

	[SerializeField]
	private int _money;

	public int GetMoneyAmount()
	{
		return _money;
	}

	public void Store(int value)
	{
		_money += value;
		OnMoneyChanged?.Invoke(_money);
	}

	public void TryPay(int value)
	{
		if (_money - value >= 0)
			Pay(value);
	}

	private void Pay(int value)
	{
		_money -= value;
		OnMoneyChanged?.Invoke(_money);
	}
}