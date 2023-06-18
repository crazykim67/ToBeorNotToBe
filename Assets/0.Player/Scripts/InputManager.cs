using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance
    {
        get 
        { 
            if(instance == null)
            {
                instance = new InputManager();
                return instance;
            }

            return instance;
        }
    }

    private PlayerControls playerControls;

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        playerControls = new PlayerControls();
    }

    private void OnEnable() => playerControls.Enable();

    private void OnDisable() => playerControls.Disable();

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool OnJump()
    {
        return playerControls.Player.Jump.triggered;
    }
}
