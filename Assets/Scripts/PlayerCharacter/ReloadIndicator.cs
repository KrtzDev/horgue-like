using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour
{
    [SerializeField] private Image _reloadCircleWeapon1;
    [SerializeField] private Image _reloadCircleWeapon2;
    [SerializeField] private float _fadeOutTime;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.down);
        transform.Rotate(0, 180, 0);
    }

    public void UseReloadIndicator(Sprite reloadCircle, float duration)
    {
        Image currentReloadCircle;

        if (!_reloadCircleWeapon1.enabled)
        {
            _reloadCircleWeapon1.gameObject.SetActive(true);
            _reloadCircleWeapon1.enabled = true;
            _reloadCircleWeapon1.sprite = reloadCircle;
            _reloadCircleWeapon1.fillAmount = 0;
            _reloadCircleWeapon1.color = new(_reloadCircleWeapon1.color.r, _reloadCircleWeapon1.color.g, _reloadCircleWeapon1.color.b, 1);
            currentReloadCircle = _reloadCircleWeapon1;
        }
        else
        {
            _reloadCircleWeapon2.gameObject.SetActive(true);
            _reloadCircleWeapon2.enabled = true;
            _reloadCircleWeapon2.sprite = reloadCircle;
            _reloadCircleWeapon2.fillAmount = 0;
            _reloadCircleWeapon2.color = new(_reloadCircleWeapon2.color.r, _reloadCircleWeapon2.color.g, _reloadCircleWeapon2.color.b, 1);
            currentReloadCircle = _reloadCircleWeapon2;
        }

        StartCoroutine(FillAmount(currentReloadCircle, duration));
    }

    private IEnumerator FillAmount(Image reloadCircle, float duration)
    {
        float elapsed = 0f;


        while (elapsed < duration)
        {
            reloadCircle.fillAmount = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeOut(reloadCircle));
    }

    private IEnumerator FadeOut(Image reloadCircle)
    {
        float timer = 0f;
        Color startColor = reloadCircle.color;
        Color targetColor = new(reloadCircle.color.r, reloadCircle.color.g, reloadCircle.color.b, 0);

        while (timer < _fadeOutTime)
        {
            reloadCircle.color = Color.Lerp(startColor, targetColor, timer / _fadeOutTime);
            timer += Time.deltaTime;
            yield return null;
        }

        reloadCircle.enabled = false;
        reloadCircle.gameObject.SetActive(false);
    }
}
