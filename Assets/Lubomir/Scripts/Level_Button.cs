using UnityEngine;
using TMPro;

public class Level_Button : MonoBehaviour
{
    public Main_Menu _main_menu;
    public int _levelIndex;
    public TMP_Text _name;

    public void StartLevel()
    {
        _main_menu.LevelButton(_levelIndex);
    }
}
