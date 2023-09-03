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
        _maxValue = _playerAbilities.MaxJetpackFuel;
        _currentValue = _playerAbilities.JetpackFuel;

        base.Awake();
    }

    public override void OnGUI()
    {
        _maxValue = _playerAbilities.MaxJetpackFuel;
        _currentValue = _playerAbilities.JetpackFuel;
        _jetpackFuelAmount.text = (int)_currentValue + " %";

        base.OnGUI();
    }
}
