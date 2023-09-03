using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void OnSliderChange()
    {
        _valueText.text = _slider.value.ToString();

        if(_slider.value == 0)
        {
            AudioManager.Instance.SetAudioVolume(-1);
        }
        else
        {
            AudioManager.Instance.SetAudioVolume(_slider.value * 4 / 100);
        }
    }
}
