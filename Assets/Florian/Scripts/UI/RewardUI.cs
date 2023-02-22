using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
	[SerializeField]
	private Image _rewardImage;

	[SerializeField]
	private ToolTipUI _toolTip_prefab;

	private Reward _reward;
	private ToolTipUI _currentToolTipUI;

	public void Initialize(Reward reward)
	{
		_reward = reward;
		_rewardImage.sprite = reward.weaponPartReward.WeaponPartUISprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_currentToolTipUI = Instantiate(_toolTip_prefab, UIManager.Instance.Endscreen.transform);
		_currentToolTipUI.Initialize(_reward.weaponPartReward);
	}
	public void OnPointerMove(PointerEventData eventData)
	{
		//_currentToolTipUI.transform.position = eventData.position;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Destroy(_currentToolTipUI.gameObject);
	}
}
