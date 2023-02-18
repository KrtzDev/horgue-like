using UnityEngine;
using UnityEngine.SceneManagement;

public class Endscreen : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneLoader.Instance.LoadScene(1);
    }

	public void CraftingMenu()
	{
		SceneLoader.Instance.LoadScene("SCENE_Weapon_Crafting");
	}

    public void NextWave()
    {
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }
}
