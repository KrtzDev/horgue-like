using UnityEngine;
using UnityEngine.UI;

public class UIImageFillAmountHealthbar : UIImageFillAmount
{
    private HealthComponent _HealthComponent;
    [SerializeField] private Text _healthText;

    public override void Awake()
    {
        _HealthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
        _maxValue = _HealthComponent._maxHealth;
        _currentValue = _HealthComponent._currentHealth;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        // maxHealth Update falls man mehr Health dazu bekommt
        _currentValue = _HealthComponent._currentHealth;
        _healthText.text = _currentValue + " / " + _maxValue;

        base.FixedUpdate();
    }
}
