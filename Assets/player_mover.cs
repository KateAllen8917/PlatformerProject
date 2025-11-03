using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float slideSpeed = 18f;
    public float slideDuration = 3f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Camera & Mouse")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    public float cameraDipAmount = 0.5f;
    public float cameraDipSpeed = 5f;

    [Header("Crouching")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchCameraOffset = -0.5f;
    public float crouchSpeed = 3f;

    [Header("Wall Running")]
    public float wallRunGravity = -1f;
    public float wallRunSpeed = 8f;
    public float wallCheckDistance = 0.6f;
    public float minWallRunHeight = 1.5f;
    public LayerMask wallLayer;
    public float wallJumpForce = 8f;
    public float wallJumpUpForce = 5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    private bool isSliding = false;
    private float slideTimer = 0f;
    private float slideDipProgress = 0f;

    private Vector3 originalCameraLocalPos;
    private Vector3 targetCameraLocalPos;

    // Wall run state
    private bool isWallRunning = false;
    private bool isExitingWall = false;
    private float exitWallTime = 0.2f;
    private float exitWallTimer;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        originalCameraLocalPos = cameraTransform.localPosition;
        targetCameraLocalPos = originalCameraLocalPos;
    }

    void Update()
    {
        HandleWallRun(); // 👈 Must come first
        HandleCrouch();
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        HandleSlide();
        HandleCameraDip();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float speed = walkSpeed;

        if (isSliding)
        {
            speed = slideSpeed;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            speed = crouchSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (!isWallRunning)
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !isSliding && !Input.GetKey(KeyCode.C))
        {
            isSliding = true;
            slideTimer = slideDuration;
            slideDipProgress = 0f;
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            slideDipProgress = Mathf.Clamp01(slideDipProgress + Time.deltaTime * cameraDipSpeed);
            float dipAmount = Mathf.Lerp(0f, cameraDipAmount, slideDipProgress);
            targetCameraLocalPos = originalCameraLocalPos + Vector3.down * dipAmount;

            if (slideTimer <= 0f)
            {
                isSliding = false;
                targetCameraLocalPos = originalCameraLocalPos;
            }
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.C))
        {
            isSliding = false;
            controller.height = crouchHeight;
            targetCameraLocalPos = originalCameraLocalPos + Vector3.up * crouchCameraOffset;
        }
        else
        {
            controller.height = standingHeight;
            if (!isSliding)
                targetCameraLocalPos = originalCameraLocalPos;
        }
    }

    void HandleCameraDip()
    {
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetCameraLocalPos, Time.deltaTime * cameraDipSpeed);
    }

    // ------------- WALL RUNNING METHODS -------------

    void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wallLayer);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wallLayer);
    }

    void StartWallRun()
    {
        isWallRunning = true;
        velocity.y = 0f;
    }

    void StopWallRun()
    {
        isWallRunning = false;
    }

    void HandleWallRun()
    {
        CheckForWall();

        bool isAboveGround = !Physics.Raycast(transform.position, Vector3.down, minWallRunHeight);

        if ((wallLeft || wallRight) && Input.GetKey(KeyCode.W) && isAboveGround && !isExitingWall)
        {
            StartWallRun();

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
            Vector3 wallDirection = Vector3.Cross(wallNormal, Vector3.up);

            if ((transform.forward - wallDirection).magnitude > (transform.forward + wallDirection).magnitude)
                wallDirection = -wallDirection;

            controller.Move(wallDirection.normalized * wallRunSpeed * Time.deltaTime);

            velocity.y = wallRunGravity;

            if (Input.GetButtonDown("Jump"))
            {
                Vector3 jumpDir = Vector3.up + wallNormal;
                velocity = jumpDir.normalized * wallJumpUpForce;
                isExitingWall = true;
                exitWallTimer = exitWallTime;
                StopWallRun();
            }
        }
        else if (isExitingWall)
        {
            exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0f)
            {
                isExitingWall = false;
            }
        }
        else
        {
            if (isWallRunning)
                StopWallRun();
        }
    }

    // Optional Gizmos to visualize wall check rays
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.right * wallCheckDistance);
        Gizmos.DrawRay(transform.position, -transform.right * wallCheckDistance);
    }
}
