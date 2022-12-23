using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    public Endscreen Endscreen { get; private set; }
    public PauseMenu PauseMenu { get; private set; }

    [SerializeField]
    private Endscreen _endScreenUI_prefab;
    [SerializeField]
    private PauseMenu _pauseMenuUI_prefab;

    private void Start()
    {
        SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;
    }

    private void OnCompletedSceneLoad()
    {
        Endscreen = Instantiate(_endScreenUI_prefab);
        Endscreen.gameObject.SetActive(false);
        PauseMenu = Instantiate(_pauseMenuUI_prefab);
        PauseMenu.gameObject.SetActive(false);
    }
}
