using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySeletionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _verticalMoveAmount = 30f;
    [SerializeField] private float _moveSelectionTime = 0.1f;
    [SerializeField] private float _moveActivationTime = 0.5f;
    [SerializeField] private float _moveActivationWaitTime = 0.25f;
    [SerializeField] private float _moveToUI_Time = 1f;
    [SerializeField] private float _moveToUI_WaitTime = 0.25f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;

    [Header("References")]
    [SerializeField] private GameObject _endPosUI;
    [SerializeField] private GameObject _abilityText;
    [SerializeField] private GameObject _selectionMarker;
    private bool _abilitySelected = false;

    [HideInInspector] public Vector3 _startPos;
    [HideInInspector] public Vector3 _startScale;

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    IEnumerator MoveAbilityOnSelection(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;

        if (startingAnimation)
        {
            _selectionMarker.SetActive(true);
        }
        else
        {
            _selectionMarker.SetActive(false);
        }  

        while (elapsedTime < _moveSelectionTime)
        {
            elapsedTime += Time.deltaTime;

            if (startingAnimation)
            {
                endPosition = _startPos + new Vector3(0f, _verticalMoveAmount, 0f);
                endScale = _startScale * _scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveSelectionTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveSelectionTime));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;    
        }
    }

    IEnumerator MoveAbilityOnActivation()
    {
        yield return new WaitForSeconds(_moveActivationWaitTime);

        Vector3 averageAbilityPosition = Vector3.zero;

        float elapsedTime = 0f;

        for (int i = 0; i < AbilitySelectionManager.instance.Cards.Length; i++)
        {
            averageAbilityPosition += AbilitySelectionManager.instance.Cards[i].GetComponent<AbilitySeletionHandler>()._startPos;
        }

        averageAbilityPosition /= AbilitySelectionManager.instance.Cards.Length;
        averageAbilityPosition += new Vector3(0f, _verticalMoveAmount, 0f);

        if(AbilitySelectionManager.instance.Cards.Length % 2 != 0)
        {
            int middleNumber = Mathf.RoundToInt((AbilitySelectionManager.instance.Cards.Length / 2) - 0.1f);

            if (AbilitySelectionManager.instance.Cards[middleNumber] == gameObject)
            {
                Debug.Log("yield break");
                StartCoroutine(MoveAbilityToUI());
                yield break;
            }
        }

        while (elapsedTime < _moveActivationTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 lerpedPos = Vector3.Lerp(transform.position, averageAbilityPosition, (elapsedTime / _moveActivationTime));

            transform.position = lerpedPos;

            yield return null;
        }

        StartCoroutine(MoveAbilityToUI());
    }

    IEnumerator MoveAbilityToUI()
    {
        yield return new WaitForSeconds(_moveToUI_WaitTime);

        _abilityText.SetActive(false);

        float elapsedTime = 0f;

        while (elapsedTime < _moveToUI_Time)
        {
            elapsedTime += Time.deltaTime;

            Vector3 lerpedPos = Vector3.Lerp(transform.position, _endPosUI.transform.position, (elapsedTime / _moveToUI_Time));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, _endPosUI.transform.localScale, (elapsedTime / _moveToUI_Time));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

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

            for (int i = 0; i < AbilitySelectionManager.instance.Cards.Length; i++)
            {
                if(AbilitySelectionManager.instance.Cards[i] == gameObject)
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

        for (int i = 0; i < AbilitySelectionManager.instance.Cards.Length; i++)
        {
            if (AbilitySelectionManager.instance.Cards[i] != gameObject)
            {
                AbilitySelectionManager.instance.Cards[i].SetActive(false);
            }
        }

        // move Ability Game Object into the Middle while being highlighted

        StartCoroutine(MoveAbilityOnActivation());

        // move Ability to Icon Corner and re-size it



        // start Level Countdown

    }
}
