using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
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
		if (_reward.weaponPartReward.isSlotted) 
			return;

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			foreach (WeaponUI weaponUI in UIManager.Instance.CraftingMenu.weaponUIs)
			{
				weaponUI.ShowPotentilaUpdatedWeaponStats(_reward.weaponPartReward);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		DestroyToolTip();

		_currentToolTipUI = Instantiate(_toolTip_prefab, transform.root);
		_currentToolTipUI.Initialize(_reward.weaponPartReward);

		_currentToolTipUI.transform.position = eventData.position + Vector2.right * 50 * transform.root.GetComponent<Canvas>().scaleFactor;
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		if (_currentToolTipUI)
		{
			_currentToolTipUI.transform.position += (Vector3)eventData.delta;

			RectTransform currentToolTipUiRect = _currentToolTipUI.GetComponent<RectTransform>();

			Vector3 pos = currentToolTipUiRect.localPosition;

			Vector3 minPosition = transform.root.GetComponent<RectTransform>().rect.min - currentToolTipUiRect.rect.min;
			Vector3 maxPosition = transform.root.GetComponent<RectTransform>().rect.max - currentToolTipUiRect.rect.max;

			pos.x = Mathf.Clamp(currentToolTipUiRect.localPosition.x, minPosition.x, maxPosition.x);
			pos.y = Mathf.Clamp(currentToolTipUiRect.localPosition.y, minPosition.y, maxPosition.y);

			currentToolTipUiRect.localPosition = pos;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		DestroyToolTip();

		if (_reward.weaponPartReward.isSlotted)
			return;

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			foreach (WeaponUI weaponUI in UIManager.Instance.CraftingMenu.weaponUIs)
			{
				weaponUI.ShowWeaponStats(weaponUI._weapon.CalculateWeaponStats(weaponUI._weapon));
			}
		}
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
			DestroyToolTip();

			List<RaycastResult> hits = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, hits);
			foreach (var hit in hits)
			{
				if (hit.gameObject.TryGetComponent(out WeaponPartSlot weaponPartSlot))
				{
					if (weaponPartSlot._owningWeaponUI.SetNewWeaponPart(_reward.weaponPartReward, weaponPartSlot))
					{
						Destroy(gameObject);
					}
					else
					{
						transform.SetParent(_parent);
						_rectTransform.localPosition = _defaultPos;
					}
				}
				else
				{
					transform.SetParent(_parent);
					_rectTransform.localPosition = _defaultPos;
				}
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
