using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : UIButton, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private Image _abilityBackground;
    [SerializeField] private GameObject _abilityNameText;
    [SerializeField] private GameObject _selectionMarker;

    private Ability _ability;

    private RectTransform _rectTransform;

    [HideInInspector] public Vector3 _startScale;

    private bool _abilitySelected = false;

    public void Initialize(Ability ability)
    {
        _ability = ability;
        _abilityNameText.GetComponent<TextMeshProUGUI>().text = _ability.name;
        _abilityIcon.sprite = ability._icon;
        _rectTransform = GetComponent<RectTransform>();
        _startScale = _rectTransform.localScale;

        Addlistener(AbilitySelected);
    }

    private void Start()
    {
        _abilityNameText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponentInChildren<Button>().colors.normalColor;
    }

    IEnumerator MoveAbilityOnSelection(bool startingAnimation)
    {
		yield return new WaitForEndOfFrame();
		Vector3 endScale;

		float elapsedTime = 0f;

		if (startingAnimation)
		{
			_selectionMarker.SetActive(true);
			_abilityNameText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponentInChildren<Button>().colors.selectedColor;
		}
		else
		{
			_selectionMarker.SetActive(false);
			_abilityNameText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponentInChildren<Button>().colors.normalColor;
		}

		while (elapsedTime < ChooseAbility.instance._moveSelectionTime)
		{
			elapsedTime += Time.unscaledDeltaTime;

			if (startingAnimation)
			{
				endScale = _startScale * ChooseAbility.instance._scaleAmount;
			}
			else
			{
				endScale = _startScale;
			}

			Vector3 lerpedScale = Vector3.Lerp(_rectTransform.localScale, endScale, (elapsedTime / ChooseAbility.instance._moveSelectionTime));

			_rectTransform.localScale = lerpedScale;

			yield return null;
		}
	}

    IEnumerator MoveAbilityOnActivation()
    {
        ChooseAbility.instance.AbilityParent.GetComponent<LayoutGroup>().enabled = false;

        _selectionMarker.SetActive(false);

        for (int i = 0; i < ChooseAbility.instance.AbilityParent.childCount; i++)
        {
            if (ChooseAbility.instance.AbilityParent.GetChild(i).gameObject != gameObject)
            {
                ChooseAbility.instance.AbilityParent.GetChild(i).gameObject.SetActive(false);
            }
        }

        yield return new WaitForSecondsRealtime(ChooseAbility.instance._moveActivationWaitTime);

        float elapsedTime = 0f;    

        while (elapsedTime < ChooseAbility.instance._moveActivationTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector3 lerpedPos = Vector3.Lerp(_rectTransform.position, ChooseAbility.instance.AbilityParent.position, (elapsedTime / ChooseAbility.instance._moveActivationTime));
            Vector3 lerpedScale = Vector3.Lerp(_rectTransform.localScale, _startScale, (elapsedTime / ChooseAbility.instance._moveActivationTime));

            _rectTransform.position = lerpedPos;
            _rectTransform.localScale = lerpedScale;

            yield return null;
        }

        StartCoroutine(MoveAbilityToUI());
    }

    IEnumerator MoveAbilityToUI()
    {
        yield return new WaitForSecondsRealtime(ChooseAbility.instance._moveToUI_WaitTime);

        _abilityNameText.SetActive(false);
        _abilityBackground.enabled = false;

        float elapsedTime = 0f;

        ChooseAbility.instance._abilityCoolDownToReplace = FindObjectOfType<AbilityCooldownToReplace>().gameObject;

        StartCoroutine(StartLevel(ChooseAbility.instance._countDown, ChooseAbility.instance._countdownText.rectTransform.localScale));

        while (elapsedTime < ChooseAbility.instance._moveToUI_Time)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector3 lerpedPos = Vector3.Lerp(_abilityIcon.transform.position, ChooseAbility.instance._abilityCoolDownToReplace.transform.position, (elapsedTime / ChooseAbility.instance._moveToUI_Time));
            Vector3 lerpedScale = Vector3.Lerp(_abilityIcon.transform.localScale, ChooseAbility.instance._abilityCoolDownToReplace.transform.localScale, (elapsedTime / ChooseAbility.instance._moveToUI_Time));

            _abilityIcon.transform.position = lerpedPos;
            _abilityIcon.transform.localScale = lerpedScale;

            yield return null;
        }

    }

    IEnumerator StartLevel(int countdown, Vector3 textStartScale)
    {
        Vector3 endScale = ChooseAbility.instance._textScale;
        float elapsedTime = 0f;

        ChooseAbility.instance._titleText.text = "Level Begins in";
        ChooseAbility.instance._countdownText.rectTransform.localScale = textStartScale;

        if (countdown == ChooseAbility.instance._countDown)
        {
            StartCoroutine(FadeBackground());
        }

        if (countdown <= 0)
        {
            if (!GameManager.Instance._gameIsPaused)
            {
                Time.timeScale = 1;
            }

            ChooseAbility.instance._countdownText.text =  "GO!!!!";

            AudioManager.Instance.PlaySound("LevelCountDownGO");

            GameManager.Instance._currentAbility = _ability;
            
            ChooseAbility.instance._abilityCoolDownToReplace.GetComponent<Image>().sprite = _ability._icon;

            while (elapsedTime < 0.5f)
            {
                elapsedTime += Time.unscaledDeltaTime;

                Vector3 lerpedScale = Vector3.Lerp(ChooseAbility.instance._countdownText.rectTransform.localScale, endScale, (elapsedTime / 1));
                ChooseAbility.instance._countdownText.rectTransform.localScale = lerpedScale;

                yield return null;
            }

            ChooseAbility.instance.gameObject.SetActive(false);
        }
        else
        {
            ChooseAbility.instance._countdownText.text = "" + countdown;

            AudioManager.Instance.PlaySound("LevelCountDown");

            while (elapsedTime < countdown + 1)
            {
                elapsedTime += Time.unscaledDeltaTime;

                if (elapsedTime >= 1)
                {
                    StartCoroutine(StartLevel(countdown - 1, textStartScale));
                    yield break;
                }

                Vector3 lerpedScale = Vector3.Lerp(ChooseAbility.instance._countdownText.rectTransform.localScale, endScale, (elapsedTime / 1));
                ChooseAbility.instance._countdownText.rectTransform.localScale = lerpedScale;

                yield return null;
            }
        }
    }

    private IEnumerator FadeBackground()
    {
        Color color = ChooseAbility.instance._background.GetComponent<Image>().color;
        float alpha = color.a;

        float elapsedTime = 0f;

        while (elapsedTime < ChooseAbility.instance._countDown)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, 0, elapsedTime / ChooseAbility.instance._countDown));
            ChooseAbility.instance._background.GetComponent<Image>().color = newColor;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Select the card
        if(!_abilitySelected)
            eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deselect the card
        if (!_abilitySelected)
            eventData.selectedObject = null;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (!_abilitySelected)
        {
            StartCoroutine(MoveAbilityOnSelection(true));

            ChooseAbility.instance.LastSelected = gameObject;

            for (int i = 0; i < ChooseAbility.instance._drawnAbilities.Count; i++)
            {
                if(ChooseAbility.instance._drawnAbilities[i] == gameObject)
                {
                    ChooseAbility.instance.LastSelectedIndex = i;
                    return;
                }
            }
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (!_abilitySelected)
            StartCoroutine(MoveAbilityOnSelection(false));
    }

    public void AbilitySelected()
    {
        if(!GameManager.Instance._gameIsPaused)
        {
            _abilitySelected = true; 
            EnableAbilityUsage(_ability);

            // change Text
            ChooseAbility.instance._titleText.text = "Ability was selected";
            ChooseAbility.instance._explanationText.text = "";
            ChooseAbility.instance._submitText.text = "";

            StartCoroutine(MoveAbilityOnActivation());
        }
    }

    public void EnableAbilityUsage(Ability ability)
    {
        PlayerAbilities playerAbilities = FindObjectOfType<PlayerAbilities>();

        switch (ability._name)
        {
            case "Dash":
                playerAbilities.CanUseDashAbility = true;
                playerAbilities.CanUseForceSphereAbility = false;
                playerAbilities.CanUseJetpackAbility = false;
                playerAbilities.CanUseEarthquakeAbility = false;
                playerAbilities.CanUseStealthAbility = false;
                break;
            case "Force Sphere":
                playerAbilities.CanUseDashAbility = false;
                playerAbilities.CanUseForceSphereAbility = true;
                playerAbilities.CanUseJetpackAbility = false;
                playerAbilities.CanUseEarthquakeAbility = false;
                playerAbilities.CanUseStealthAbility = false;
                break;
            case "Jetpack":
                playerAbilities.CanUseDashAbility = false;
                playerAbilities.CanUseForceSphereAbility = false;
                playerAbilities.CanUseJetpackAbility = true;
                playerAbilities.CanUseEarthquakeAbility = false;
                playerAbilities.CanUseStealthAbility = false;

                UIManager.Instance.JetPackUI.SetActive(true);
                break;
            case "Earthquake":
                playerAbilities.CanUseDashAbility = false;
                playerAbilities.CanUseForceSphereAbility = false;
                playerAbilities.CanUseJetpackAbility = false;
                playerAbilities.CanUseEarthquakeAbility = true;
                playerAbilities.CanUseStealthAbility = false;
                break;
            case "Decoy":
                playerAbilities.CanUseDashAbility = false;
                playerAbilities.CanUseForceSphereAbility = false;
                playerAbilities.CanUseJetpackAbility = false;
                playerAbilities.CanUseEarthquakeAbility = false;
                playerAbilities.CanUseStealthAbility = true;
                break;
        }

        playerAbilities.ActivateVisuals();
    }
}
