using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _updateSpeedPerSeconds = 0.15f;
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _healthComponent = this.GetComponentInParent<HealthComponent>();
        _healthComponent.OnHealthPercentChanged += HandleHealthChanged;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.down);
        transform.Rotate(0, 180, 0);
    }

    private void Update()
    {
        if (_healthComponent._currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleHealthChanged(float percent)
    {
		if (_healthComponent._isDead)
			return;

        StartCoroutine(ChangeToPercent(percent));

        if(_healthComponent._currentHealth < _healthComponent._maxHealth && !this.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator ChangeToPercent(float percent)
    {
        float preChangePct = _healthBar.fillAmount;
        float elapsed = 0f;

        while (elapsed < _updateSpeedPerSeconds)
        {
            elapsed += Time.deltaTime;
            _healthBar.fillAmount = Mathf.Lerp(preChangePct, percent, elapsed / _updateSpeedPerSeconds);
            yield return null;
        }

        _healthBar.fillAmount = percent;
    }
}
