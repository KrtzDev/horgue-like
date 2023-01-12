public class InputManager : Singleton<InputManager>
{
    public Player_Input_Mappings CharacterInputActions { get; set; }

    public void DisableCharacterInputs()
    {
        CharacterInputActions?.Disable();
    }

    public void EnableCharacterInputs()
    {
        CharacterInputActions?.Enable();
    }
}
