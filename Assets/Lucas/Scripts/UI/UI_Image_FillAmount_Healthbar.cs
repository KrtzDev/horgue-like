using UnityEngine;

public class UI_Image_FillAmount_Healthbar : UI_Image_FillAmount
{
    private HealthComponent _HealthComponent;

    public override void Awake()
    {
        _HealthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
        _maxValue = _HealthComponent.maxHealth;
        _currentValue = _HealthComponent.currentHealth;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        // maxHealth Update falls man mehr Health dazu bekommt
        _currentValue = _HealthComponent.currentHealth;

        base.FixedUpdate();
    }
}
