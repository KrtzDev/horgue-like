using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCostUI : MonoBehaviour
{
	public Image BgImage => _bgImage;
	public TMP_Text CostText => _costText;

	[SerializeField]
	private Image _bgImage;

	[SerializeField]
	private TMP_Text _costText;

	private void OnEnable()
	{
		// Update for when shop is loaded, new shop is loaded
		GameManager.Instance.inventory.Wallet.OnMoneyChanged += UpdateMoneyUI;
	}

	private void Start()
	{
		int value = GameManager.Instance.inventory.Wallet.GetMoneyAmount();
		_costText.text = $"$: {value}";
	}

	private void UpdateMoneyUI(int value)
	{
		_costText.text = $"$: {value}";
	}
}
