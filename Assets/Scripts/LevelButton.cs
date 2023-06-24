using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public MainMenu _mainMenu;
    public int _levelIndex;
    public TMP_Text _name;

    public void StartLevel()
    {
        _mainMenu.LevelButton(_levelIndex);
    }
}
