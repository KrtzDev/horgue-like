using UnityEngine;
using UnityEngine.UI;
public class UI_Image_FillAmount : MonoBehaviour
{
    protected Image _ImageToFill;
    protected float _currentValue;
    protected float _maxValue;

    public virtual void Awake()
    {
        _ImageToFill = this.GetComponent<Image>();
    }

    public virtual void FixedUpdate()
    {
        _ImageToFill.fillAmount = _currentValue / _maxValue;
    }
}
