using UnityEngine;

public class UIImageFillAmountAbility : UIImageFillAmount
{
    private PlayerMovementMobility _playerMovementMoblity;

    public override void Awake()
    {
        _playerMovementMoblity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementMobility>();

        base.Awake();
    }

    public override void FixedUpdate()
    {
        _maxValue = _playerMovementMoblity._currentMaxCD;
        _currentValue = _playerMovementMoblity._abilityCDTimer;

        base.FixedUpdate();
    }
}
