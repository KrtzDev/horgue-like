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
