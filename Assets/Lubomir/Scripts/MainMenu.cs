using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _levelButtonPrefab;
    [SerializeField]
    private Transform _levelButtonParent;

    private void Awake()
    {
        SpawnButtons();
    }

    private void SpawnButtons()
    {
        if (SceneManager.sceneCountInBuildSettings > 0)
        {
            for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                GameObject _newLevelButton = Instantiate(_levelButtonPrefab, _levelButtonParent);
                LevelButton _level_Button = _newLevelButton.GetComponent<LevelButton>();

                int levelNumber = i-1;

                _level_Button._name.text = "Level " +  levelNumber.ToString();
                _level_Button._levelIndex = i;
                _level_Button._mainMenu = this;
            }
        }
        else
        {
            Debug.Log("No scenes in build");
        }
    }

    public void LevelButton(int _levelIndex)
    {
        SceneLoader.Instance.LoadScene(_levelIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
