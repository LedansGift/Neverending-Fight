using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    private bool playerControlActive = false;
    public static InputManager Instance { get; private set; }
    public static bool disableInputs = false;
    private Controls controls;

    public Vector2 MovementValue { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public Action OnAttackEvent;
    public Action OnAttackReleaseEvent;
    public Action OnDashEvent;
    public Action OnDashReleaseEvent;
    public Action OnSpecialEvent;

    public Action OnPauseEvent;
    public EventHandler<int> OnSelectWeaponEvent;
    public EventHandler<float> OnSwapWeaponEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        //disableInputs = true;
    }

    private void OnEnable() { }

    private void OnDisable() { }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
        else if (context.canceled)
        {
            OnAttackReleaseEvent?.Invoke();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }
        if (context.performed)
        {
            OnDashEvent?.Invoke();
        }
        else if (context.canceled)
        {
            OnDashReleaseEvent?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            MovementValue = Vector2.zero;
            return;
        }

        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            MousePosition = Vector2.zero;
            return;
        }

        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!playerControlActive)
        {
            return;
        }

        if (context.performed)
        {
            OnPauseEvent?.Invoke();
        }
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnSpecialEvent?.Invoke();
        }
    }

    public void OnWeapon1Select(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnSelectWeaponEvent?.Invoke(this, 1);
        }
    }

    public void OnWeapon2Select(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnSelectWeaponEvent?.Invoke(this, 2);
        }
    }

    public void OnWeapon3Select(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnSelectWeaponEvent?.Invoke(this, 3);
        }
    }

    public void OnWeaponSwap(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            float swapValue = context.ReadValue<float>();

            OnSwapWeaponEvent?.Invoke(this, swapValue);
            //Value is either -1 or 1 depending on Q or E
        }
    }

    private void ToggleDisableInputs(object sender, bool toggle)
    {
        if (!playerControlActive)
        {
            return;
        }

        disableInputs = toggle;
    }
}
