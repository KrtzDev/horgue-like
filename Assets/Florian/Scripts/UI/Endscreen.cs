using UnityEngine;

public class Endscreen : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneLoader.Instance.LoadScene(1);
    }
}
