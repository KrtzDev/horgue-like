using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    HealthComponent hp;

    [SerializeField]
    private Image currentHealth;
    [SerializeField]
    private float updateSpeedSeconds = 0.15f;

    private void Awake()
    {
        hp = this.GetComponentInParent<HealthComponent>();
        hp.OnHealthPctChanged += HandleHealthChanged;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));

        if(hp.currentHealth < hp.maxHealth && !this.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = currentHealth.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            currentHealth.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        currentHealth.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.down);
        transform.Rotate(0, 180, 0);
    }
}
