using UnityEngine;
using UnityEngine.UI;

public class ToolTipUI : MonoBehaviour
{
	[SerializeField]
	private Image _image;
	[SerializeField]
	private RectTransform _statParent;
	[SerializeField]
	private StatUI _statUI_prefab;

	public void Initialize(WeaponPart weaponPartData)
	{
		_image.sprite = weaponPartData.WeaponPartUISprite;
		if (weaponPartData is Magazine)
		{
			Magazine mag = weaponPartData as Magazine;
			Debug.Log("Ping");
			StatUI currentStat = Instantiate(_statUI_prefab,_statParent);
			currentStat.Initialize("Attack Speed: ",mag.attackSpeed.ToString());
		}
	}
}