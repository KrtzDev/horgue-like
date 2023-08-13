using UnityEngine;

[System.Serializable]
public class Wallet
{
	[SerializeField]
	private int _money;

	public void Store(int value)
	{
		_money += value;
	}

	public void TryPay(int value)
	{
		if(_money - value >= 0)
			Pay(value);
	}

	private void Pay(int value)
	{
		_money -= value;
	}
}