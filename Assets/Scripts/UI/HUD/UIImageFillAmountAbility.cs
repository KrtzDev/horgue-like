using UnityEngine;

public class UIImageFillAmountAbility : UIImageFillAmount
{
    private PlayerAbilities _playerMovementMoblity;

    public override void Awake()
    {
        _playerMovementMoblity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();

        base.Awake();
    }

    public override void FixedUpdate()
    {
        _maxValue = _playerMovementMoblity._currentMaxCD;
        _currentValue = _playerMovementMoblity._abilityCDTimer;

        base.FixedUpdate();
    }
}
