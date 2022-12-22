using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public SceneFader SceneFader { get; private set; }
    public Endscreen Endscreen { get; private set; }
    public PauseMenu PauseMenu { get; private set; }

    [SerializeField]
    private SceneFader _sceneFaderUI_prefab;
    [SerializeField]
    private Endscreen _endScreenUI_prefab;
    [SerializeField]
    private PauseMenu _pauseMenuUI_prefab;

    protected override void Awake()
    {
        base.Awake();
        SceneFader = Instantiate(_sceneFaderUI_prefab);
        SceneFader.gameObject.SetActive(false);
        Endscreen = Instantiate(_endScreenUI_prefab);
        Endscreen.gameObject.SetActive(false);
        PauseMenu = Instantiate(_pauseMenuUI_prefab);
        PauseMenu.gameObject.SetActive(false);
    }
}
