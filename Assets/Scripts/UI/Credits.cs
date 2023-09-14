using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private bool _mainCredits;
    [SerializeField] private TMP_Text _firstName;
    [SerializeField] private TMP_Text _lastName;
    [SerializeField] private TMP_Text _role;


    [SerializeField] private GameObject _creditsPlaytest;
    [SerializeField] private Image _background;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private float _timeToFade;

    private void Awake()
    {
        if(_mainCredits)
            SetCreditsPlayTestFalse();
    }

    public void ChangeFirstName(string name)
    {
        _firstName.text = "";
        _firstName.text = name;
    }

    public void ChangeLastName(string name)
    {
        _lastName.text = "";
        _lastName.text = name;
    }

    public void ChangeRole(string role)
    {
        _role.text = "";
        _role.text = role;
    }

    public void StartPlayTestCredits()
    {
        _creditsPlaytest.SetActive(true);
    }

    private void StartFadeToBlack()
    {
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        _mainMenu.DeactivateCreditsButton();

        float elapsedTime = 0;

        while(elapsedTime < _timeToFade)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Color lerpedColor = Color.Lerp(_background.color, Color.black, (elapsedTime / _timeToFade));

            _background.color = lerpedColor;

            yield return null;
        }

        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        SetCreditsPlayTestFalse();
        _mainMenu.StartCredits();
        _background.color = new Color(0, 0, 0, 0);
    }

    public void SetCreditsPlayTestFalse()
    {
        _creditsPlaytest.SetActive(false);
    }
}
