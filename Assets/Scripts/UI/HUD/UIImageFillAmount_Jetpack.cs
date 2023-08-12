using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIImageFillAmount_Jetpack : UIImageFillAmount
{
    public TextMeshProUGUI _jetpackFuelAmount;
    private PlayerAbilities _playerAbilities;

    public override void Awake()
    {
        _playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        _maxValue = _playerAbilities._maxJetpackFuel;
        _currentValue = _playerAbilities.jetpackFuel;

        base.Awake();
    }

    public override void OnGUI()
    {
        _maxValue = _playerAbilities._maxJetpackFuel;
        _currentValue = _playerAbilities.jetpackFuel;
        _jetpackFuelAmount.text = (int)_currentValue + " %";

        base.OnGUI();
    }
}
