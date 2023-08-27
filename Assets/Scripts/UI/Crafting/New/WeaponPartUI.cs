using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponPartUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public Image WeaponPartImage => _effectImage;

	[SerializeField]
	private Image _rewardImage;
	[SerializeField]
	private Image _effectImage;

	[SerializeField]
	private ToolTipUI _toolTip_prefab;

	private ToolTipUI _currentToolTipUI;

	private RectTransform _parent;
	private RectTransform _rectTransform;
	private Vector2 _defaultPos;

	public WeaponUI weaponUI;
	public WeaponPart weaponPart;
	public Transform statsContainer;
	public bool isSlotted;

	public void Initialize(WeaponPart weaponPart)
	{
		this.weaponPart = weaponPart;
		_rewardImage.sprite = weaponPart.WeaponPartUISprite;
		_rectTransform = GetComponent<RectTransform>();
		_parent = transform.parent.GetComponent<RectTransform>();
		_defaultPos = _rectTransform.localPosition;
	}

	public void Select()
	{
		_currentToolTipUI = Instantiate(_toolTip_prefab, statsContainer);
		_currentToolTipUI.Initialize(weaponPart);

		weaponUI.ShowPotentialUpdatedWeaponStats(weaponPart);
	}

	public void Deselect()
	{
		weaponUI.ShowWeaponStats(weaponUI.weapon.CalculateWeaponStats(weaponUI.weapon));
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isSlotted)
			return;

		weaponUI.ShowPotentialUpdatedWeaponStats(weaponPart);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(!isSlotted) 
			return;

		DestroyToolTip();

		Select();
	}


	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isSlotted)
			return;

		DestroyToolTip();

		weaponUI.ShowWeaponStats(weaponUI.weapon.CalculateWeaponStats(weaponUI.weapon));
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		DestroyToolTip();
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position += (Vector3)eventData.delta;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DestroyToolTip();

		List<RaycastResult> hits = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, hits);
		foreach (var hit in hits)
		{
			if (hit.gameObject.TryGetComponent(out WeaponUI weaponUI))
			{
				if (weaponUI.SetNewWeaponPart(this))
				{
					Destroy(gameObject);
					GameManager.Instance.inventory.RemoveFromInventory(weaponPart);
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

	public void DestroyToolTip()
	{
		if (_currentToolTipUI)
		{
			Destroy(_currentToolTipUI.gameObject);
			_currentToolTipUI = null;
		}
	}
}
