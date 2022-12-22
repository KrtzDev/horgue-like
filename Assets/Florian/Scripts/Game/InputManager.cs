public class InputManager : Singleton<InputManager>
{
    public Player_Input_Mappings PlayerInputActions { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerInputActions = new Player_Input_Mappings();
    }
}
