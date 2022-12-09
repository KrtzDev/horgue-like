using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Temp_Menu : MonoBehaviour
{
    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard.oKey.wasPressedThisFrame)
        {
            Debug.Log("Load Main Menu");
            SceneManager.LoadScene(0);
        }
    }
}
