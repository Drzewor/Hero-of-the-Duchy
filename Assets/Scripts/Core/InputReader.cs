using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue {get; private set;}
    public bool IsAttacking {get; private set;}
    public bool IsBlocking {get; private set;}
    public bool IsSprinting {get; private set;}
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action InteractEvent;
    public event Action SaveEvent;
    public event Action LoadEvent;
    public event Action DeleteEvent;
    public event Action PressIEvent;
    public event Action PressESCEvent;
    public event Action PressJEvent;
    public bool isGamePaused = false;

    private Controls controls;
    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy() 
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //used in cinemachine
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if (!context.performed) return;
        TargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }

    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;

        InteractEvent?.Invoke();
    }

    public void OnSave(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        SaveEvent?.Invoke();
    }

    public void OnLoad(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        LoadEvent?.Invoke();
    }

    public void OnDelete(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        DeleteEvent?.Invoke();
    }

    public void OnPressI(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        PressIEvent?.Invoke();
    }

    public void OnPressESC(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        PressESCEvent?.Invoke();
    }

    public void OnPressJ(InputAction.CallbackContext context)
    {
        if(isGamePaused) return;
        if(!context.performed) return;
        PressJEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            IsSprinting = true;
        }
        else if (context.canceled)
        {
            IsSprinting = false;
        }
    }

    public bool ESCHasMultipleHandleres()
    {
        return PressESCEvent.GetInvocationList().Length > 1;
    }
}
