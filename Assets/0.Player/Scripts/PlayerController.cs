using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if(instance == null)
            {
                return instance = new PlayerController();
            }

            return instance;
        }
    }

    [HideInInspector]
    public CharacterController ch;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -10f;

    private InputManager input;
    private Transform cameraTransform;
    [SerializeField]
    private Transform arm;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ch = GetComponent<CharacterController>();
        input = InputManager.Instance;
        cameraTransform = Camera.main.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        groundedPlayer = ch.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Movement();

        if (input.OnJump() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        ch.Move(playerVelocity * Time.deltaTime);

        OnChangeSpeed();
    }

    public void Movement()
    {
        //if (OptionManager.Instance == null)
        //    return;

        if (OptionManager.Instance.isMenu || InventoryManager.Instance.isUi)
            return;

        Vector2 movement = input.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);

        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        move = move.normalized;
        ArmController.Instance.OnRun(move);
        ch.Move(move * Time.deltaTime * playerSpeed);
    }

    public void OnChangeSpeed()
    {
        if (InventoryManager.Instance.isUse)
        {
            playerSpeed = 2f;
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
            playerSpeed = 3f;
        else if(Input.GetKeyUp(KeyCode.LeftShift))
            playerSpeed = 2f;
    }
}
