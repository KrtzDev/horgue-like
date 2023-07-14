using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private GameObject _abilityNameText;
    [SerializeField] private GameObject _selectionMarker;

    private Ability _ability;

    private RectTransform _parent;
    private RectTransform _rectTransform;

    [HideInInspector] public Vector3 _startPos;
    [HideInInspector] public Vector3 _startScale;

    private bool _abilitySelected = false;

    public void Initialize(Ability ability)
    {
        _ability = ability;
        _abilityNameText.GetComponent<TextMeshProUGUI>().text = _ability.name;
        _abilityIcon.sprite = ability._icon;
        _rectTransform = GetComponent<RectTransform>();
        _parent = transform.parent.GetComponent<RectTransform>();
        _startPos = _rectTransform.localPosition;
    }

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
        _abilityNameText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponentInChildren<Button>().colors.normalColor;
    }

    IEnumerator MoveAbilityOnSelection(bool startingAnimation)
    {
        Vector3 endPosition;
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
                endPosition = _startPos + new Vector3(0f, ChooseAbility.instance._verticalMoveAmount, 0f);
                endScale = _startScale * ChooseAbility.instance._scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / ChooseAbility.instance._moveSelectionTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / ChooseAbility.instance._moveSelectionTime));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;    
        }
    }

    IEnumerator MoveAbilityOnActivation()
    {
        _selectionMarker.SetActive(false);

        yield return new WaitForSecondsRealtime(ChooseAbility.instance._moveActivationWaitTime);

        Vector3 averageAbilityPosition = Vector3.zero;

        float elapsedTime = 0f;

        for (int i = 0; i < ChooseAbility.instance._drawnAbilities.Count; i++)
        {
            averageAbilityPosition += ChooseAbility.instance._drawnAbilities[i]._startPos;
        }

        averageAbilityPosition /= ChooseAbility.instance._drawnAbilities.Count;
        averageAbilityPosition += new Vector3(0f, ChooseAbility.instance._verticalMoveAmount, 0f);

        /*
        if(AbilitySelectionManager.instance.Abilities.Length % 2 != 0)
        {
            int middleNumber = Mathf.RoundToInt((AbilitySelectionManager.instance.Abilities.Length / 2) - 0.1f);

            if (AbilitySelectionManager.instance.Abilities[middleNumber] == gameObject)
            {
                Debug.Log("yield break");
                StartCoroutine(MoveAbilityToUI());
                yield break;
            }
        }
        */

        while (elapsedTime < ChooseAbility.instance._moveActivationTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector3 lerpedPos = Vector3.Lerp(transform.position, averageAbilityPosition, (elapsedTime / ChooseAbility.instance._moveActivationTime));

            transform.position = lerpedPos;

            yield return null;
        }

        StartCoroutine(MoveAbilityToUI());
    }

    IEnumerator MoveAbilityToUI()
    {
        yield return new WaitForSecondsRealtime(ChooseAbility.instance._moveToUI_WaitTime);

        _abilityNameText.SetActive(false);

        float elapsedTime = 0f;

        StartCoroutine(StartLevel(ChooseAbility.instance._countDown, ChooseAbility.instance._countdownText.rectTransform.localScale));

        while (elapsedTime < ChooseAbility.instance._moveToUI_Time)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector3 lerpedPos = Vector3.Lerp(transform.position, ChooseAbility.instance._endPosUI.transform.position, (elapsedTime / ChooseAbility.instance._moveToUI_Time));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, ChooseAbility.instance._endPosUI.transform.localScale, (elapsedTime / ChooseAbility.instance._moveToUI_Time));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    IEnumerator StartLevel(int countdown, Vector3 textStartScale)
    {
        Vector3 endScale = ChooseAbility.instance._textScale;
        float elapsedTime = 0f;

        ChooseAbility.instance._titleText.text = "Level Begins in";
        ChooseAbility.instance._countdownText.rectTransform.localScale = textStartScale;

        if(countdown == ChooseAbility.instance._countDown)
        {
            StartCoroutine(FadeBackground());
        }

        if (countdown <= 0)
        {
            Time.timeScale = 1;
            ChooseAbility.instance._countdownText.text =  "GO!!!!";

            while (elapsedTime < 1)
            {
                elapsedTime += Time.unscaledDeltaTime;

                Vector3 lerpedScale = Vector3.Lerp(ChooseAbility.instance._countdownText.rectTransform.localScale, endScale, (elapsedTime / 1));
                ChooseAbility.instance._countdownText.rectTransform.localScale = lerpedScale;

                yield return null;
            }

            ChooseAbility.instance._countdownText.text = "";
            ChooseAbility.instance._titleText.text = "";
        }
        else
        {
            ChooseAbility.instance._countdownText.text = "" + countdown;

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

    public void OnSelect(BaseEventData eventData)
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
        _abilitySelected = true;

        // change Text
        ChooseAbility.instance._titleText.text = "Ability was selected";
        ChooseAbility.instance._explanationText.text = "";
        ChooseAbility.instance._submitText.text = "";
        // add text animation, fade-in, fade-out

        // select Ability for Game purposes

        // disable the other Ability Game Objects

        for (int i = 0; i < ChooseAbility.instance._drawnAbilities.Count; i++)
        {
            if (ChooseAbility.instance._drawnAbilities[i] != gameObject)
            {
                // ChooseAbility.instance._drawnAbilities[i].SetActive(false);
            }
        }

        // move Ability Game Object into the Middle while being highlighted

        StartCoroutine(MoveAbilityOnActivation());

        // move Ability to Icon Corner and re-size it

        // start Level Countdown

    }
}
