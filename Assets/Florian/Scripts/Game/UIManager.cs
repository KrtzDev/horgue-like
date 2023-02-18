using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{

    public Endscreen Endscreen { get; private set; }
    public Endscreen WaveEndScreen { get; private set; }
    public PauseMenu PauseMenu { get; private set; }
    public GameObject GameUI { get; private set; }

    [SerializeField]
    private Endscreen _endScreenUI_prefab;
    [SerializeField]
    private Endscreen _waveEndScreenUI_prefab;
    [SerializeField]
    private PauseMenu _pauseMenuUI_prefab;
	[SerializeField]
	private GameObject _gameUI_prefab;

    private void Start()
    {
        SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;
    }

    private void OnCompletedSceneLoad()
    {
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu" || SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting") 
			return;

        PauseMenu = Instantiate(_pauseMenuUI_prefab);
        PauseMenu.gameObject.SetActive(false);
		Endscreen = Instantiate(_endScreenUI_prefab);
		Endscreen.gameObject.SetActive(false);
		WaveEndScreen = Instantiate(_waveEndScreenUI_prefab);
		WaveEndScreen.gameObject.SetActive(false);
		GameUI = Instantiate(_gameUI_prefab);
		GameUI.gameObject.SetActive(true);
    }
}
