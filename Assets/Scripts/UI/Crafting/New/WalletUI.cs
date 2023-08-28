using TMPro;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _moneyText;

	private void OnEnable()
	{
		GameManager.Instance.inventory.Wallet.OnMoneyChanged += UpdateMoneyUI;
	}

	private void Start()
	{
		int value = GameManager.Instance.inventory.Wallet.GetMoneyAmount();
		_moneyText.text = $"$: {value}";
	}

	private void UpdateMoneyUI(int value)
	{
		_moneyText.text = $"$: {value}";
	}
}
