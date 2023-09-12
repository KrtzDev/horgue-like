using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
	public Image BgImage => _bgImage;

	[SerializeField]
	private Image _bgImage;

	[SerializeField]
	private TMP_Text _moneyText;

	private void OnEnable()
	{
		GameManager.Instance.inventory.Wallet.OnMoneyChanged += UpdateMoneyUI;
	}

	private void Start()
	{
		int value = GameManager.Instance.inventory.Wallet.GetMoneyAmount();
		_moneyText.text = $"$ {value}";
	}

	private void UpdateMoneyUI(int value)
	{
		_moneyText.text = $"$ {value}";
	}
}
