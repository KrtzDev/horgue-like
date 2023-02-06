using UnityEngine;
using UnityEngine.UI;

public class UIImageFillAmountHealthbar : UIImageFillAmount
{
    private HealthComponent _HealthComponent;
    [SerializeField]
    private Text _healthText;

    public override void Awake()
    {
        _HealthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
        _maxValue = _HealthComponent.MaxHealth;
        _currentValue = _HealthComponent.CurrentHealth;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        // maxHealth Update falls man mehr Health dazu bekommt
        _currentValue = _HealthComponent.CurrentHealth;
        _healthText.text = _currentValue + " / " + _maxValue;

        base.FixedUpdate();
    }
}
