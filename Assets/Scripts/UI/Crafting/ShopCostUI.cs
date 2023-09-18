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
}
