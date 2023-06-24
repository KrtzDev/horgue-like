using UnityEngine;
using UnityEngine.UI;
public abstract class UIImageFillAmount : MonoBehaviour
{
    [SerializeField] protected Image _imageToFill;
    protected float _currentValue;
    protected float _maxValue;

    public virtual void Awake()
    {
        if (_imageToFill ==  null)
        {
            _imageToFill = this.GetComponent<Image>();
        }
    }

    public virtual void FixedUpdate()
    {
        _imageToFill.fillAmount = _currentValue / _maxValue;
    }
}
