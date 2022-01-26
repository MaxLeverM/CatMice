using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator playerAnimator;

    [Header("Default movement parameters")]
    [SerializeField] private float runSpeed = 7;

    [Header("Gravity paramaters")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float forceDownVelocity = -2;
    [SerializeField] private Transform groundCheckComponent;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump parameters")]
    [SerializeField] private float defaultJumpHeight = 2;
    [SerializeField] private float bigJumpHeight = 3;

    [Header("Crouch parameters")]
    [SerializeField] private float crouchSpeed = 3;
    [SerializeField] private float crouchingHeight = 1;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, -0.5f, 0);

    [Header("Dash settings")]
    [SerializeField] private float dashSpeed = 20;
    [SerializeField] private float dashTime = 0.3f;

    [Header("Attack parameters")]
    [SerializeField] private float attackDistance = 3;
    [SerializeField] private LayerMask playerLayer;

    private Transform mainCamera;
    private Vector3 velocity;
    private float standingHeight;
    private Vector3 standingCenter;
    private bool isGrounded;
    private bool isCrouching;
    private bool isDashing;

    protected virtual void Start()
    {
        mainCamera = Camera.main.transform;
        standingHeight = characterController.height;
        standingCenter = characterController.center;
    }

    protected virtual void Update()
    {
        playerAnimator.SetBool("Crouch", isCrouching);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            PlayerJump(defaultJumpHeight);

        SimulatePhysics();

        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            Crouch();
            MovePlayer(crouchSpeed);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching && isGrounded &&!isDashing)
            StartCoroutine(Dash());

        if (Input.GetMouseButtonDown(0))
            Attack();

        isCrouching = false;
        SetPlayerHeight();

        MovePlayer(runSpeed);
    }

    protected virtual void MovePlayer(float speed)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        var rightDirectionMove = transform.right * horizontalInput;
        var forwardDirectionMove = transform.forward * verticalInput;
        var moveDirection = (rightDirectionMove + forwardDirectionMove) * speed * Time.deltaTime;

        playerAnimator.SetFloat("Speed", moveDirection.magnitude);
        characterController.Move(moveDirection);
        
    }

    protected virtual void PlayerJump(float jumpHeight)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * forceDownVelocity * gravity);
    }

    protected virtual void Crouch()
    {
        isCrouching = true;
        SetPlayerHeight();
    }

    protected virtual IEnumerator Dash()
    {
        isDashing = true;
        var startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
    }

    protected virtual void Attack()
    {
        playerAnimator.SetTrigger("Kick");
        var ray = new Ray(mainCamera.position, mainCamera.forward);
        if(Physics.Raycast(ray, attackDistance, playerLayer))
        {
            //TODO: kill the mouse
        }
    }

    protected virtual void SimulatePhysics()
    {
        isGrounded = Physics.CheckSphere(groundCheckComponent.position, groundCheckRadius, groundLayer);
        playerAnimator.SetBool("Jump", !isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = forceDownVelocity;

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    protected virtual void SetPlayerHeight()
    {
        characterController.height = isCrouching ? crouchingHeight : standingHeight;
        characterController.center = isCrouching ? crouchingCenter : standingCenter;
        var cameraPosition = mainCamera.position;
        cameraPosition.y = characterController.bounds.max.y;
        mainCamera.position = cameraPosition;
    }
}
