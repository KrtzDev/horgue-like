using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement_Mobility : MonoBehaviour
{
    [Header("Player General Ability")]
    [SerializeField]
    private float abilityCDTimer;
    private bool isUsingAbility;

    private Player_Character _character;
    private Player_Movement _playerMovement;
    private Player_Input_Mappings _inputActions;

    [Header("Which (one) Ability can be used?")]

    [SerializeField]
    bool canUseJumpAbility;
    [SerializeField]
    bool canUseDashAbility;
    [SerializeField]
    bool canUseStealthAbility;
    [SerializeField]
    bool canUseFlickerStrikeAbility;

    [Header("Player Jump Ability")]
    [SerializeField]
    private float jumpCD;
    private bool isUsingJumpAbility;

    [Header("Player Dash Ability")]
    [SerializeField]
    private float dashCD;
    [SerializeField]
    private float dashTime;
    private bool isUsingDashAbility;

    [SerializeField]
    private float dashForce = 30;

    [Header("Player Stealth Ability")]
    [SerializeField]
    private float stealthCD;
    private bool isUsingStealthAbility;

    [Header("Player Flicker Strike Ability")]
    [SerializeField]
    private float flickerStrikeCD;
    private bool isUsingFlickerStrikeAbility;

    private void Awake()
    {
        _inputActions = new Player_Input_Mappings();
        _inputActions.Character.MovementAction.performed += UseAbility;
    }

    private void Start()
    {
        _character = GetComponent<Player_Character>();
        _playerMovement = GetComponent<Player_Movement>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        TrackTimer();
    }

    // General

    private void UseAbility(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isUsingAbility)
        {
            if (canUseJumpAbility && abilityCDTimer <= 0)
            {
                JumpAbility();
            }

            if (canUseDashAbility && abilityCDTimer <= 0)
            {
                DashAbility();
            }

            if (canUseStealthAbility && abilityCDTimer <= 0)
            {
                StealthAbility();
            }

            if (canUseFlickerStrikeAbility && abilityCDTimer <= 0)
            {
                FlickerStrikeAbility();
            }
        }
    }

    private void TrackTimer()
    {
        if (abilityCDTimer > 0) abilityCDTimer -= Time.deltaTime;
    }

    // Jump

    private void ResetAbilityTimer(float cd)
    {
        abilityCDTimer = cd;
    }

    private void JumpAbility()
    {
        isUsingAbility = true;
        isUsingJumpAbility = true;
    }

    private void ResetJumpAbility()
    {
        isUsingAbility = false;
        isUsingJumpAbility = false;

        ResetAbilityTimer(jumpCD);
    }

    // Dash

    private void DashAbility()
    {
        isUsingAbility = true;
        isUsingDashAbility = true;

        Vector3 forceToApply = transform.TransformDirection(Vector3.forward) * dashForce;

        _character.CharacterRigidbody.velocity = Vector3.zero;
        _character.CharacterRigidbody.AddForce(forceToApply, ForceMode.Impulse);

        ResetDashAbility();
    }

    private void ResetDashAbility()
    {
        isUsingAbility = false;
        isUsingDashAbility = false;

        ResetAbilityTimer(dashCD);
    }

    // Stealth

    private void StealthAbility()
    {
        isUsingAbility = true;
        isUsingStealthAbility = true;
    }

    private void ResetStealthbility()
    {
        isUsingAbility = false;
        isUsingStealthAbility = false;

        ResetAbilityTimer(stealthCD);
    }

    // Flicker Strike

    private void FlickerStrikeAbility()
    {
        isUsingAbility = true;
        isUsingFlickerStrikeAbility = true;
    }

    private void ResetFlickerStrikeAbility()
    {
        isUsingAbility = false;
        isUsingFlickerStrikeAbility = false;

        ResetAbilityTimer(flickerStrikeCD);
    }
}
