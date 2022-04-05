using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the movement of the player with given input from the input manager
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Basic Settings")]
    [Tooltip("The speed at which the player moves")]
    public float moveSpeed = 10f;
    [Tooltip("The speed at which the player rotates to look left and right, calculated in degrees")]
    public float lookSpeed = 20f;
    [Tooltip("The power at which the player jumps")]
    public float jumpPower = 8f;
    [Tooltip("The strength of the gravity")]
    public static float gravity = 9.81f;

    [Header("Required References")]
    [Tooltip("The player shooter script that fires projectiles")]
    public Shooter playerShooter;
    public Health playerHealth;
    public List<GameObject> disableWhileDead;

    [Header("Jump Timing")]
    public float jumpTimeLeniency = 0.1f;
    private float timeToStopBeingLeniency = 0;

    private InputManager inputManager;
    private CharacterController controller;

    private bool doubleJumpAvailable = false;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update call
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetUpInputManager();
        SetUpCharacterController();
    }

    private void SetUpInputManager()
    {
        inputManager = InputManager.instance;
        if(inputManager == null)
        {
            Debug.LogError("There's no Input Manager in the scene!");
        }
    }

    private void SetUpCharacterController()
    {
        controller = gameObject.GetComponent<CharacterController>();

        if(controller == null)
        {
            Debug.LogError("There's no Character Controller attached to this game object!");
        }
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            foreach(GameObject childObject in disableWhileDead)
            {
                childObject.SetActive(false);
            }
            return;
        }
        else
        {
            foreach(GameObject childObject in disableWhileDead)
            {
                childObject.SetActive(true);
            }
        }
        ProcessMovement();
        ProcessRotation();
    }



    Vector3 moveDirection;

    private void ProcessMovement()
    {
        float leftRightInput = inputManager.horizontalMoveAxis;
        float forwardBackwardInput = inputManager.verticalMoveAxis;
        bool jumpPressed = inputManager.jumpPressed;
    
        if (controller.isGrounded)
        {
          doubleJumpAvailable = true;
          timeToStopBeingLeniency = Time.time + jumpTimeLeniency;
          moveDirection = new Vector3(leftRightInput * moveSpeed, 0, forwardBackwardInput * moveSpeed);  
          moveDirection = transform.TransformDirection(moveDirection);

            if (jumpPressed)
            {
                moveDirection.y = jumpPower;
            }
          
        }
        else
        {
            moveDirection = new Vector3(leftRightInput * moveSpeed, moveDirection.y, forwardBackwardInput * moveSpeed);
            moveDirection = transform.TransformDirection(moveDirection);

            if(jumpPressed && Time.time < timeToStopBeingLeniency)
            {
                moveDirection.y = jumpPower;
            }
            else if(jumpPressed && doubleJumpAvailable)
            {
                moveDirection.y = jumpPower;
                doubleJumpAvailable = false;
            }
        }
        //Debug.Log("The movement direction is "+ moveDirection);
        moveDirection.y -= gravity * Time.deltaTime;
    
        if (controller.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -0.3f;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void ProcessRotation()
    {
        float lookAroundInput = inputManager.horizontalLookAxis;
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + lookAroundInput * lookSpeed * Time.deltaTime, playerRotation.z));
    }
}
