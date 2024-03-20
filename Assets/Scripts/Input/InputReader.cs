using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

// read input from player using Controls input system

[CreateAssetMenu(fileName = "NewInputReader", menuName = "Input/InputReader", order = 0)]
public class InputReader : ScriptableObject, IPlayerActions {

    // events for input actions   
    public event Action<bool> PrimaryFireEvent;

    public event Action<Vector2> MoveEvent;

    public event Action<Vector2> AimEvent;
    public Vector2 AimPosition {
        get;
        private set;
    }


    // enable Controls object and set up callbacks
    private Controls controls;

    public Controls Controls { get => controls; set => controls = value; }

    private void OnEnable() {
        if(Controls == null) {
            Controls = new Controls();
            Controls.Player.SetCallbacks(this);
        }
        Controls.Player.Enable();
    }

    // callback for player movement input
    public void OnMove(InputAction.CallbackContext context) {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    // callback for firing input
    public void OnFire(InputAction.CallbackContext context) {
        if(context.performed) {
            PrimaryFireEvent?.Invoke(true);
        }
        else if (context.canceled) {
            PrimaryFireEvent?.Invoke(false);
        }
    }

    // callback for aiming input
    public void OnAim(InputAction.CallbackContext context) {
        AimEvent?.Invoke(context.ReadValue<Vector2>());
    }

}
