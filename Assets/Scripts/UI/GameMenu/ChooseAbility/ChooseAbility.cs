using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChooseAbility: MonoBehaviour
{
    [field: SerializeField] public RectTransform AbilityParent { get; private set; }

    [SerializeField] public List<Ability> _abilities = new List<Ability>();
    private List<Ability> _tempAbilities = new List<Ability>();
    public List<Ability> _drawnAbilities = new List<Ability>();
    [SerializeField] private AbilityUI _abilityUI_prefab;

    public static ChooseAbility instance;

    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _countdownText;
    public TextMeshProUGUI _explanationText;
    public TextMeshProUGUI _submitText;

    public int _countDown;
    public float _verticalMoveAmount = 30f;
    public float _moveSelectionTime = 0.1f;
    public float _moveActivationTime = 0.5f;
    public float _moveActivationWaitTime = 0.25f;
    public float _moveToUI_Time = 1f;
    public float _moveToUI_WaitTime = 0.25f;
    [Range(0f, 2f)] public float _scaleAmount = 1.1f;
    public Vector3 _textScale = new Vector3(0.5f, 0.5f, 0.5f);

    public int _abilitiesToDisplay;

    public GameObject _abilityCoolDownToReplace; // make script and attach to ui button thingy, then use get typeof script or something
    public GameObject _background;

    public GameObject LastSelected { get; set; }
    public int LastSelectedIndex { get; set; }

    public Ability GetRandomAbility()
    {
        Ability drawnAbility = _tempAbilities[Random.Range(0, _tempAbilities.Count - 1)];
        _drawnAbilities.Add(drawnAbility);
        _tempAbilities.Remove(drawnAbility);
        return drawnAbility;
    }

    /*
    public void PopulateAbilityUI(List<Ability> abilities)
    {
        foreach (Ability ability in abilities)
        {
            AbilityUI abilityUI = Instantiate(_abilityUI_prefab, AbilityParent);
            abilityUI.Initialize(ability);
        }
    }
    */

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

			Time.timeScale = 0;
			_tempAbilities = _abilities;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SetSelectedCardAfterOneFrame());
    }

    private IEnumerator SetSelectedCardAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(AbilityParent.transform.GetChild(0).gameObject);
    }
}
