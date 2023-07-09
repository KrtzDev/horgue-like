using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	[SerializeField] private PlayerInputMappings _inputActions;

	public PlayerInputMappings CharacterInputActions
	{
		get
		{
			if (_inputActions == null)
				_inputActions = new PlayerInputMappings();

			return _inputActions;
		}
	}

	public void DisableCharacterInputs() => CharacterInputActions?.Disable();

	public void EnableCharacterInputs() => CharacterInputActions?.Enable();
}
