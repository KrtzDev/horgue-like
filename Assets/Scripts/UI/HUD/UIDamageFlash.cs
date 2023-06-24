using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageFlash : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _color;

    Coroutine _currentFlashRoutine = null;

    public void DamageFlash(float secondsForOneFlash, float maxAlpha)
    {
        _image.color = _color;

        // maxAlpha = 0 to 1
        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if(_currentFlashRoutine != null)
            StopCoroutine(_currentFlashRoutine);       
        _currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash, maxAlpha));
    }

    private IEnumerator Flash(float secondsForOneFlash, float maxAlpha)
    {
        // Flash In
        float flashInDuration = secondsForOneFlash / 2;
        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(0, maxAlpha, t / flashInDuration);
            _image.color = colorThisFrame;
            yield return null;
        }

        // Flash Out
        float flashOutDuration = secondsForOneFlash / 2;
        for (float t = 0; t <= flashOutDuration; t += Time.deltaTime)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(maxAlpha, 0, t / flashOutDuration);
            _image.color = colorThisFrame;
            yield return null;
        }

        // Alpha = 0 to avoid Screen Errors
        _image.color = new Color(0, 0, 0, 0);

    }

}
