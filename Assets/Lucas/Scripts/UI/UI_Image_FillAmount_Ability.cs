using UnityEngine;

public class UI_Image_FillAmount_Ability : UI_Image_FillAmount
{
    private Player_Movement_Mobility _PlayerMovementMoblity;

    public override void Awake()
    {
        _PlayerMovementMoblity = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement_Mobility>();

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
