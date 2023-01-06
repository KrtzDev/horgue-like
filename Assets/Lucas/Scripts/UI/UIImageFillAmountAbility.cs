using UnityEngine;

public class UIImageFillAmountAbility : UIImageFillAmount
{
    private PlayerMovementMobility _PlayerMovementMoblity;

    public override void Awake()
    {
        _PlayerMovementMoblity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementMobility>();

        _maxValue = _PlayerMovementMoblity._currentMaxCD;
        _currentValue = _PlayerMovementMoblity._abilityCDTimer;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        _maxValue = _PlayerMovementMoblity._currentMaxCD;
        _currentValue = _PlayerMovementMoblity._abilityCDTimer;

        base.FixedUpdate();
    }
}
