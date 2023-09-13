using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
	[SerializeField] private int _rotation = 90;

	private CinemachineVirtualCamera _cmVirtualCam;
	private CinemachineOrbitalTransposer _cmOrbitalTransposer;

	private PlayerInputMappings _inputActions;
	private float _moveDir;

	private void Start()
	{
		_cmVirtualCam = GetComponent<CinemachineVirtualCamera>();
		_cmOrbitalTransposer = _cmVirtualCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

		_inputActions = InputManager.Instance.CharacterInputActions;

		_inputActions.Character.Enable();

		_inputActions.Character.CameraMovement.performed += MoveCamera;
		_inputActions.Character.CameraMovement.canceled += ReleasedButton;
	}

	private void Update()
	{
		if (_moveDir < 0)
			_cmOrbitalTransposer.m_XAxis.Value -= Time.deltaTime * _rotation;
		else if (_moveDir > 0)
			_cmOrbitalTransposer.m_XAxis.Value += Time.deltaTime * _rotation;
	}

	private void MoveCamera(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
			_moveDir = ctx.ReadValue<float>();
	}

	private void ReleasedButton(InputAction.CallbackContext ctx)
	{
		_moveDir = 0;
	}
}
