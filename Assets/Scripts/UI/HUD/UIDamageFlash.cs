using Cinemachine;
using System.Collections;
// using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIDamageFlash : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Color _backGroundColor;
    [SerializeField] private Image _textureImage;

    [SerializeField] private Vector2 _posOffset;

    Coroutine _currentFlashRoutine = null;

    [SerializeField] private CinemachineBasicMultiChannelPerlin _cam;
    [SerializeField] private float _screenShakeIntensity = 1f;

    private Gamepad _pad;
    [SerializeField] private float _lowFrequencyPulse = 0.25f;
    [SerializeField] private float _highFrequencyPulse = 1f;

    private void Start()
    {
        _cam = FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
    }

    public void DamageFlash(float secondsForOneFlash, float maxAlphaBackground, float maxAlphaTexture)
    {
        _backgroundImage.color = _backGroundColor;

        // maxAlpha = 0 to 1
        maxAlphaBackground = Mathf.Clamp(maxAlphaBackground, 0, 1);

        if(_currentFlashRoutine != null)
            StopCoroutine(_currentFlashRoutine);       
        _currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash, maxAlphaBackground, maxAlphaTexture));
    }

    private IEnumerator Flash(float secondsForOneFlash, float maxAlphaBackground, float maxAlphaTexture)
    {
        float xOffset = Random.Range(-_posOffset.x, _posOffset.x);
        float yOffset = Random.Range(-_posOffset.y, _posOffset.y);
        _textureImage.rectTransform.localPosition = new Vector3(xOffset, yOffset, 0);

        // Flash In
        float flashInDuration = secondsForOneFlash / 2;
        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
            _cam.m_AmplitudeGain = _screenShakeIntensity;

            RumblePulse(_lowFrequencyPulse, _highFrequencyPulse);

            Color colorThisFrame = _backgroundImage.color;
            colorThisFrame.a = Mathf.Lerp(0, maxAlphaBackground, t / flashInDuration);
            _backgroundImage.color = colorThisFrame;

            colorThisFrame = _textureImage.color;
            colorThisFrame.a = Mathf.Lerp(0, maxAlphaTexture, t / flashInDuration);
            _textureImage.color = colorThisFrame;
            yield return null;
        }

        // Flash Out
        float flashOutDuration = secondsForOneFlash / 2;
        for (float t = 0; t <= flashOutDuration; t += Time.deltaTime)
        {
            _cam.m_AmplitudeGain = 0;

            StopRumble();

            Color colorThisFrame = _backgroundImage.color;
            colorThisFrame.a = Mathf.Lerp(maxAlphaBackground, 0, t / flashOutDuration);
            _backgroundImage.color = colorThisFrame;

            colorThisFrame = _textureImage.color;
            colorThisFrame.a = Mathf.Lerp(maxAlphaTexture, 0, t / flashOutDuration);
            _textureImage.color = colorThisFrame;
            yield return null;
        }

        // Alpha = 0 to avoid Screen Errors
        _backgroundImage.color = new Color(0, 0, 0, 0);
        _textureImage.color = new Color(0, 0, 0, 0);
    }

    private void RumblePulse(float lowFrequency, float highFrequency)
    {
        _pad = Gamepad.current;

        if (_pad != null)
        {
            _pad.SetMotorSpeeds(lowFrequency, highFrequency);
        }
    }

    private void StopRumble()
    {
        _pad = Gamepad.current;

        if (_pad != null)
        {
            _pad.SetMotorSpeeds(0f, 0f);
        }
    } 
}
