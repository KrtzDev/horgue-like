using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AbilitySelectionManager: MonoBehaviour
{
    public static AbilitySelectionManager instance;

    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _countdownText;
    public TextMeshProUGUI _explanationText;
    public TextMeshProUGUI _submitText;

    public GameObject[] Abilities;

    public int _countDown;
    public float _verticalMoveAmount = 30f;
    public float _moveSelectionTime = 0.1f;
    public float _moveActivationTime = 0.5f;
    public float _moveActivationWaitTime = 0.25f;
    public float _moveToUI_Time = 1f;
    public float _moveToUI_WaitTime = 0.25f;
    [Range(0f, 2f)] public float _scaleAmount = 1.1f;
    public Vector3 _textScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject _endPosUI; // make script and attach to ui button thingy, then use get typeof script or something
    public GameObject _background;

    public GameObject LastSelected { get; set; }
    public int LastSelectedIndex { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SetSelectedCardAfterOneFrame());
    }

    private IEnumerator SetSelectedCardAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(Abilities[0]);
    }
}
