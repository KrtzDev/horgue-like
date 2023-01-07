using UnityEngine;

public class UIImageFillAmountAbility : UIImageFillAmount
{
    private PlayerMovementMobility _PlayerMovementMoblity;

    public override void Awake()
    {
        _PlayerMovementMoblity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementMobility>();

        _maxValue = _PlayerMovementMoblity.CurrentMaxCD;
        _currentValue = _PlayerMovementMoblity.AbilityCDTimer;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        _maxValue = _PlayerMovementMoblity.CurrentMaxCD;
        _currentValue = _PlayerMovementMoblity.AbilityCDTimer;

        base.FixedUpdate();
    }
}
