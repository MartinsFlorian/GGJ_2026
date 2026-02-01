using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    //[Header("Input")]
    [Header("Output")]
    [SerializeField] private RSE_OnPlayerDash rse_OnPlayerDash;
    [SerializeField] private RSE_OnPlayerJump rse_OnPlayerJump;
    [SerializeField] private RSE_OnPlayerLook rse_OnPlayerLook;
    [SerializeField] private RSE_OnPlayerMove rse_OnPlayerMove;
    [SerializeField] private RSE_OnPlayerPause rse_OnPlayerPause;

    private IA_PlayerInput ia_PlayerInput = null;

    private void Awake()
    {
        ia_PlayerInput = new IA_PlayerInput();
        playerInput.actions = ia_PlayerInput.asset;
    }
    private void OnEnable()
    {
        var player = ia_PlayerInput.Player;

        player.Move.performed += OnMoveInput;
        player.Move.canceled += OnMoveInput;
        player.Jump.performed += OnJumpInput;
        player.Dash.performed += OnDashInput;
        player.Look.performed += OnLookInput;
        player.Look.canceled += OnLookInput;
        player.Pause.performed += OnPauseInput;
    }


    private void OnDisable()
    {
        var player = ia_PlayerInput.Player;

        player.Move.performed -= OnMoveInput;
        player.Move.canceled -= OnMoveInput;
        player.Jump.performed -= OnJumpInput;
        player.Dash.performed -= OnDashInput;
        player.Look.performed -= OnLookInput;
        player.Look.canceled -= OnLookInput;
        player.Pause.performed -= OnPauseInput;
    }
    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        rse_OnPlayerMove.Call(ctx.ReadValue<Vector2>());
    }
    private void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) rse_OnPlayerJump.Call();
    }
    private void OnDashInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) rse_OnPlayerDash.Call();
    }
    private void OnLookInput(InputAction.CallbackContext ctx)
    {
        rse_OnPlayerLook.Call(ctx.ReadValue<Vector2>());
    }
    private void OnPauseInput(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) rse_OnPlayerPause.Call();
    }
}