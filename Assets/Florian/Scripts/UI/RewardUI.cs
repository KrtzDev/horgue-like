using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[SerializeField]
	private Image _rewardImage;

	[SerializeField]
	private ToolTipUI _toolTip_prefab;

	private Reward _reward;
	private ToolTipUI _currentToolTipUI;

	private RectTransform _parent;
	private RectTransform _rectTransform;
	private Vector2 _defaultPos;

	public void Initialize(Reward reward)
	{
		_reward = reward;
		_rewardImage.sprite = reward.weaponPartReward.WeaponPartUISprite;
		_rectTransform = GetComponent<RectTransform>();
		_parent = transform.parent.GetComponent<RectTransform>();
		_defaultPos = _rectTransform.localPosition;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_currentToolTipUI = Instantiate(_toolTip_prefab, transform.root);
		_currentToolTipUI.Initialize(_reward.weaponPartReward);

		_currentToolTipUI.transform.position = eventData.position + Vector2.right * 50 * transform.root.GetComponent<Canvas>().scaleFactor;
	}
	public void OnPointerMove(PointerEventData eventData)
	{
		if (_currentToolTipUI)
		{
			_currentToolTipUI.transform.position += (Vector3)eventData.delta;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		DestroyToolTip();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			DestroyToolTip();
			transform.SetParent(transform.root);
			transform.SetAsLastSibling();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			transform.position += (Vector3)eventData.delta;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			//if ()
			//{

			//}
			//else
			{
				transform.SetParent(_parent);
				_rectTransform.localPosition = _defaultPos;
			}
		}
	}

	private void DestroyToolTip()
	{
		if (_currentToolTipUI)
		{
			Destroy(_currentToolTipUI.gameObject);
			_currentToolTipUI = null;
		}
	}
}
