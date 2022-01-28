using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;
    [SerializeField] private Material playerMaterial;

    [Header("Default movement parameters")]
    [SerializeField] private float runSpeed = 7;

    [Header("Gravity parameters")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float forceDownVelocity = -2;
    [SerializeField] private Transform groundCheckComponent;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump parameters")]
    [SerializeField] private float defaultJumpHeight = 2;

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

    [Header("Effects parameters")] 
    [SerializeField] private float effectDuration = 5;
    [SerializeField] private float speedUpMultiplier = 2;
    [SerializeField] private float slowDownMultiplier = 0.5f;
    [SerializeField] private float bigJumpHeight = 3;

    private const string MaterialMixParameterName = "Vector1_1dda082405be4b85b58f95f41111bdcf";

    private float playerSpeed;
    private float speedModificator = 1;
    private float playerJump;
    private Transform mainCamera;
    private Vector3 velocity;
    private float standingHeight;
    private Vector3 standingCenter;
    private bool canJump;
    private bool isGrounded;
    private bool isCrouching;
    private bool isDashing;

    protected virtual void Start()
    {
        playerMaterial.SetFloat(MaterialMixParameterName, 1);
        
        playerSpeed = runSpeed;
        playerJump = defaultJumpHeight;
        canJump = true;
        mainCamera = Camera.main.transform;
        standingHeight = characterController.height;
        standingCenter = characterController.center;
    }

    protected virtual void Update()
    {
        playerAnimator.SetBool("Crouch", isCrouching);

        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded)
            PlayerJump(playerJump);

        SimulatePhysics();

        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            Crouch();
            MovePlayer(playerSpeed * speedModificator);
            return;
        }

        playerSpeed = runSpeed;
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching && isGrounded &&!isDashing)
            StartCoroutine(Dash());

        if (Input.GetMouseButtonDown(0))
            Attack();

        isCrouching = false;
        SetPlayerHeight();

        MovePlayer(playerSpeed * speedModificator);
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
        playerSpeed = crouchSpeed;
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

    public virtual void ApplyNewSpeed(bool isPositive)
    {
        StartCoroutine(NewSpeedCoroutine(isPositive));
    }
    
    public virtual IEnumerator NewSpeedCoroutine(bool isPositive)
    {
        speedModificator = isPositive ? speedUpMultiplier : slowDownMultiplier;
        yield return new WaitForSeconds(effectDuration);
        speedModificator = 1;
    }

    public virtual void ApplyBigJump()
    {
        StartCoroutine(BigJumpCoroutine());
    }
    
    public virtual IEnumerator BigJumpCoroutine()
    {
        playerJump = bigJumpHeight;
        yield return new WaitForSeconds(effectDuration);
        playerJump = defaultJumpHeight;
    }

    public virtual void DisableJump()
    {
        StartCoroutine(DisableJumpCoroutine());
    }

    public virtual IEnumerator DisableJumpCoroutine()
    {
        canJump = false;
        yield return new WaitForSeconds(effectDuration);
        canJump = true;
    }

    public virtual IEnumerator TransformToMouse()
    {
        for (float i = 0; i < 1; i += Time.deltaTime / 3)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0,100,EasingSmoothSquared(i)));
            }
            playerMaterial.SetFloat(MaterialMixParameterName, Mathf.Lerp(0, 1, EasingSmoothSquared(i)));
            
            yield return null;
        }
    }

    public virtual void TransformToCat()
    {
        StartCoroutine(TransformToCatCoroutine());
    }
    
    public virtual IEnumerator TransformToCatCoroutine()
    {
        for (float i = 0; i < 1; i += Time.deltaTime / 3)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(100,0,EasingSmoothSquared(i)));
            }
            playerMaterial.SetFloat(MaterialMixParameterName, Mathf.Lerp(1, 0, EasingSmoothSquared(i)));
            
            yield return null;
        }
    }

    public virtual void TeleportPlayer(Vector3 newPosition)
    {
        StartCoroutine(TeleportPlayerCoroutine(newPosition));
    }
    
    public virtual IEnumerator TeleportPlayerCoroutine(Vector3 newPosition)
    {
        yield return null;
        transform.position = newPosition;
    }

    private float EasingSmoothSquared(float x)
    {
        return x < 0.5f ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
    }
}
