using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
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
        if (EditorBuildSettings.scenes.Length > 0)
        {
            for (int i = 1; i < EditorBuildSettings.scenes.Length; i++)
            {
                GameObject _newLevelButton = Instantiate(_levelButtonPrefab, _levelButtonParent);
                Level_Button _level_Button = _newLevelButton.GetComponent<Level_Button>();

                _level_Button._name.text = "Level " + i.ToString();
                _level_Button._levelIndex = i;
                _level_Button._main_menu = this;
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
