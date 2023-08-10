using UnityEngine;

public class InputManager : Singleton<InputManager>
{

	public PlayerInputMappings CharacterInputActions { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		CharacterInputActions = new PlayerInputMappings();
	}

	public void DisableCharacterInputs() => CharacterInputActions?.Disable();

	public void EnableCharacterInputs() => CharacterInputActions?.Enable();
}
