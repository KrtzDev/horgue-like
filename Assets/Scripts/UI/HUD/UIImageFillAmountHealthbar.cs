using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIImageFillAmountHealthbar : UIImageFillAmount
{
    private HealthComponent _HealthComponent;
    [SerializeField] private TextMeshProUGUI _healthText;

    public override void Awake()
    {
        _HealthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
        _maxValue = _HealthComponent.maxHealth;
        _currentValue = _HealthComponent.currentHealth;

        base.Awake();
    }

    public override void OnGUI()
    {
        // maxHealth Update falls man mehr Health dazu bekommt
        _currentValue = _HealthComponent.currentHealth;
        _healthText.text = _currentValue + " / " + _maxValue;

        base.OnGUI();
    }
}
