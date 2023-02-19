public class InputManager : Singleton<InputManager>
{
    public PlayerInputMappings CharacterInputActions { get; set; }

    public void DisableCharacterInputs()
    {
        CharacterInputActions?.Disable();
    }

    public void EnableCharacterInputs()
    {
        CharacterInputActions?.Enable();
    }
}
