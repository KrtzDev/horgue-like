using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AbilitySeletionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject _abilityText;
    [SerializeField] private GameObject _selectionMarker;

    private bool _abilitySelected = false;

    [HideInInspector] public Vector3 _startPos;
    [HideInInspector] public Vector3 _startScale;

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
        _abilityText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponent<Button>().colors.normalColor;
    }

    IEnumerator MoveAbilityOnSelection(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;

        if (startingAnimation)
        {
            _selectionMarker.SetActive(true);
            _abilityText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponent<Button>().colors.selectedColor;
        }
        else
        {
            _selectionMarker.SetActive(false);
            _abilityText.GetComponent<TextMeshProUGUI>().color = gameObject.GetComponent<Button>().colors.normalColor;
        }  

        while (elapsedTime < AbilitySelectionManager.instance._moveSelectionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (startingAnimation)
            {
                endPosition = _startPos + new Vector3(0f, AbilitySelectionManager.instance._verticalMoveAmount, 0f);
                endScale = _startScale * AbilitySelectionManager.instance._scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / AbilitySelectionManager.instance._moveSelectionTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / AbilitySelectionManager.instance._moveSelectionTime));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;    
        }
    }

    IEnumerator MoveAbilityOnActivation()
    {
        _selectionMarker.SetActive(false);

        yield return new WaitForSecondsRealtime(AbilitySelectionManager.instance._moveActivationWaitTime);

        Vector3 averageAbilityPosition = Vector3.zero;

        float elapsedTime = 0f;

        for (int i = 0; i < AbilitySelectionManager.instance.Abilities.Length; i++)
        {
            averageAbilityPosition += AbilitySelectionManager.instance.Abilities[i].GetComponent<AbilitySeletionHandler>()._startPos;
        }

        averageAbilityPosition /= AbilitySelectionManager.instance.Abilities.Length;
        averageAbilityPosition += new Vector3(0f, AbilitySelectionManager.instance._verticalMoveAmount, 0f);

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

        while (elapsedTime < AbilitySelectionManager.instance._moveActivationTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector3 lerpedPos = Vector3.Lerp(transform.position, averageAbilityPosition, (elapsedTime / AbilitySelectionManager.instance._moveActivationTime));

            transform.position = lerpedPos;

            yield return null;
        }

        StartCoroutine(MoveAbilityToUI());
    }

    IEnumerator MoveAbilityToUI()
    {
        yield return new WaitForSecondsRealtime(AbilitySelectionManager.instance._moveToUI_WaitTime);

        _abilityText.SetActive(false);

        float elapsedTime = 0f;

        StartCoroutine(StartLevel(AbilitySelectionManager.instance._countDown, AbilitySelectionManager.instance._countdownText.rectTransform.localScale));

        while (elapsedTime < AbilitySelectionManager.instance._moveToUI_Time)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector3 lerpedPos = Vector3.Lerp(transform.position, AbilitySelectionManager.instance._endPosUI.transform.position, (elapsedTime / AbilitySelectionManager.instance._moveToUI_Time));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, AbilitySelectionManager.instance._endPosUI.transform.localScale, (elapsedTime / AbilitySelectionManager.instance._moveToUI_Time));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    IEnumerator StartLevel(int countdown, Vector3 textStartScale)
    {
        Vector3 endScale = AbilitySelectionManager.instance._textScale;
        float elapsedTime = 0f;

        AbilitySelectionManager.instance._titleText.text = "Level Begins in";
        AbilitySelectionManager.instance._countdownText.rectTransform.localScale = textStartScale;

        if(countdown == AbilitySelectionManager.instance._countDown)
        {
            StartCoroutine(FadeBackground());
        }

        if (countdown <= 0)
        {
            Time.timeScale = 1;
            AbilitySelectionManager.instance._countdownText.text =  "GO!!!!";

            while (elapsedTime < 1)
            {
                elapsedTime += Time.unscaledDeltaTime;

                Vector3 lerpedScale = Vector3.Lerp(AbilitySelectionManager.instance._countdownText.rectTransform.localScale, endScale, (elapsedTime / 1));
                AbilitySelectionManager.instance._countdownText.rectTransform.localScale = lerpedScale;

                yield return null;
            }

            AbilitySelectionManager.instance._countdownText.text = "";
            AbilitySelectionManager.instance._titleText.text = "";
        }
        else
        {
            AbilitySelectionManager.instance._countdownText.text = "" + countdown;

            while (elapsedTime < countdown + 1)
            {
                elapsedTime += Time.unscaledDeltaTime;

                if (elapsedTime >= 1)
                {
                    StartCoroutine(StartLevel(countdown - 1, textStartScale));
                    yield break;
                }

                Vector3 lerpedScale = Vector3.Lerp(AbilitySelectionManager.instance._countdownText.rectTransform.localScale, endScale, (elapsedTime / 1));
                AbilitySelectionManager.instance._countdownText.rectTransform.localScale = lerpedScale;

                yield return null;
            }
        }
    }

    private IEnumerator FadeBackground()
    {
        Color color = AbilitySelectionManager.instance._background.GetComponent<Image>().color;
        float alpha = color.a;

        float elapsedTime = 0f;

        while (elapsedTime < AbilitySelectionManager.instance._countDown)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, 0, elapsedTime / AbilitySelectionManager.instance._countDown));
            AbilitySelectionManager.instance._background.GetComponent<Image>().color = newColor;

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

            AbilitySelectionManager.instance.LastSelected = gameObject;

            for (int i = 0; i < AbilitySelectionManager.instance.Abilities.Length; i++)
            {
                if(AbilitySelectionManager.instance.Abilities[i] == gameObject)
                {
                    AbilitySelectionManager.instance.LastSelectedIndex = i;
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
        AbilitySelectionManager.instance._titleText.text = "Ability was selected";
        AbilitySelectionManager.instance._explanationText.text = "";
        AbilitySelectionManager.instance._submitText.text = "";
        // add text animation, fade-in, fade-out

        // select Ability for Game purposes

        // disable the other Ability Game Objects

        for (int i = 0; i < AbilitySelectionManager.instance.Abilities.Length; i++)
        {
            if (AbilitySelectionManager.instance.Abilities[i] != gameObject)
            {
                AbilitySelectionManager.instance.Abilities[i].SetActive(false);
            }
        }

        // move Ability Game Object into the Middle while being highlighted

        StartCoroutine(MoveAbilityOnActivation());

        // move Ability to Icon Corner and re-size it

        // start Level Countdown

    }
}
