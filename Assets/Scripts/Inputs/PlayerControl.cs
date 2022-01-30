using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private SkinnedMeshRenderer firstEyeMesh;
    [SerializeField] private SkinnedMeshRenderer secondEyeMesh;

    [Header("Hunter/Victim fov parameters")] 
    [SerializeField] private float victimFov = 70;
    [SerializeField] private float hunterFov = 50;

    [Header("Default movement parameters")]
    [SerializeField] private float victimRunSpeed = 7;
    [SerializeField] private float hunterRunSpeed = 10;

    [Header("Gravity parameters")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float forceDownVelocity = -2;
    [SerializeField] private Transform groundCheckComponent;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump parameters")]
    [SerializeField] private float defaultJumpHeight = 2;

    [Header("Crouch parameters")]
    [SerializeField] private float victimCrouchSpeed = 3;
    [SerializeField] private float hunterCrouchSpeed = 4;
    [SerializeField] private float crouchingHeight = 1;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, -0.5f, 0);

    [Header("Dash settings")]
    [SerializeField] private float dashSpeed = 20;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashCooldownTime = 3;

    [Header("Attack parameters")]
    [SerializeField] private float attackDistance = 3;
    [SerializeField] private LayerMask playerLayer;

    [Header("Effects parameters")] 
    [SerializeField] private float effectDuration = 5;
    [SerializeField] private float speedUpMultiplier = 2;
    [SerializeField] private float slowDownMultiplier = 0.5f;
    [SerializeField] private float bigJumpHeight = 3;

    private const string MaterialMixParameterName = "Vector1_1dda082405be4b85b58f95f41111bdcf";

    private Material playerMaterial;
    private float playerSpeed;
    private float speedModificator = 1;
    private float playerJump;
    private Camera mainCamera;
    private Transform mainCameraTransform;
    private Vector3 velocity;
    private float standingHeight;
    private Vector3 standingCenter;
    private bool canJump;
    private bool isGrounded;
    private bool isCrouching;
    private bool isDashing;
    private bool isChangingHeight;
    private bool isTransforming;
    private float nextDashTime = 0;
    private Coroutine heightRoutine;
    private bool isHunter = false;
    private SkinnedMeshRenderer[] meshRenderers;

    public bool IsHunter
    {
        get => isHunter;
        set => isHunter = value;
    }
    
    protected virtual void Start()
    {
        meshRenderers = new[] { playerMesh, firstEyeMesh, secondEyeMesh };
        playerMaterial = playerMesh.material;
        playerMaterial.SetFloat(MaterialMixParameterName, 1);
        
        playerJump = defaultJumpHeight;
        canJump = true;
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        standingHeight = characterController.height;
        standingCenter = characterController.center;
    }

    protected virtual void Update()
    {
        if (isTransforming) return;
        
        if (isHunter)
        {
            HunterControl();
            return;
        }
        
        VictimControl();
    }

    private void HunterControl()
    {
        if(!isChangingHeight) 
            StartCoroutine(SetPlayerHeight());

        playerAnimator.SetBool("Crouch", isCrouching);

        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded)
            PlayerJump(playerJump);
        SimulatePhysics();

        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            Crouch(hunterCrouchSpeed);
            MovePlayer(playerSpeed * speedModificator);
            return;
        }

        playerSpeed = hunterRunSpeed;

        if (Input.GetMouseButtonDown(0))
            Attack();

        isCrouching = false;

        MovePlayer(playerSpeed * speedModificator);
    }

    private void VictimControl()
    {
        if(!isChangingHeight) 
            StartCoroutine(SetPlayerHeight());

        playerAnimator.SetBool("Crouch", isCrouching);

        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded)
            PlayerJump(playerJump);
        SimulatePhysics();

        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            Crouch(victimCrouchSpeed);
            MovePlayer(playerSpeed * speedModificator);
            return;
        }

        playerSpeed = victimRunSpeed;
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime && isGrounded)
            StartCoroutine(Dash());

        isCrouching = false;

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

    protected virtual void Crouch(float speed)
    {
        isCrouching = true;
        playerSpeed = speed;
    }

    protected virtual IEnumerator Dash()
    {
        isDashing = true;
        var startTime = Time.time;
        nextDashTime = startTime + dashCooldownTime;
        while (Time.time < startTime + dashTime)
        {
            if (!isDashing) yield break;
            characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
    }

    protected virtual void Attack()
    {
        playerAnimator.SetTrigger("Kick");
        var ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
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

    protected virtual IEnumerator SetPlayerHeight()
    {
        if (!isGrounded) yield break;
        
        characterController.height = isCrouching ? crouchingHeight : standingHeight;
        characterController.center = isCrouching ? crouchingCenter : standingCenter;
        var startCameraPosition = mainCameraTransform.position;
        isChangingHeight = true;
        for (float i = 0; i < 1; i += Time.deltaTime * 3.5f)
        {
            var currentCameraPosition = mainCameraTransform.position;
            currentCameraPosition.y = Mathf.Lerp(startCameraPosition.y, characterController.bounds.max.y, i);
            mainCameraTransform.position = currentCameraPosition;
            yield return null;
        }
        isChangingHeight = false;
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

    public virtual IEnumerator TransformToMouseCoroutine()
    {
        isTransforming = true;
        playerAnimator.SetBool("IsTransforming", isTransforming);
        ResetAnimationParameters();
        for (float i = 0; i < 1; i += Time.deltaTime / 2)
        {
            SimulatePhysics();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0,100,EasingSmoothSquared(i)));
            }
            playerMaterial.SetFloat(MaterialMixParameterName, Mathf.Lerp(0, 1, EasingSmoothSquared(i)));
            mainCamera.fieldOfView = Mathf.Lerp(hunterFov, victimFov, EasingSmoothSquared(i));
            
            yield return null;
        }
        isTransforming = false;
        isHunter = false;
        playerAnimator.SetBool("IsTransforming", isTransforming);
    }

    public virtual void TransformToCat()
    {
        StartCoroutine(TransformToCatCoroutine());
    }
    
    public virtual IEnumerator TransformToCatCoroutine()
    {
        isTransforming = true;
        playerAnimator.SetBool("IsTransforming", isTransforming);
        ResetAnimationParameters();
        for (float i = 0; i < 1; i += Time.deltaTime / 2)
        {
            SimulatePhysics();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(100,0,EasingSmoothSquared(i)));
            }
            playerMaterial.SetFloat(MaterialMixParameterName, Mathf.Lerp(1, 0, EasingSmoothSquared(i)));
            mainCamera.fieldOfView = Mathf.Lerp(victimFov, hunterFov, EasingSmoothSquared(i));
            
            yield return null;
        }
        isTransforming = false;
        isHunter = true;
        playerAnimator.SetBool("IsTransforming", isTransforming);
    }

    private void ResetAnimationParameters()
    {
        playerAnimator.SetFloat("Speed", 0);
        playerAnimator.SetBool("Crouch", false);
        playerAnimator.SetBool("Jump", false);
    }

    public virtual void TeleportPlayer(Vector3 newPosition)
    {
        StartCoroutine(TeleportPlayerCoroutine(newPosition));
    }
    
    public virtual IEnumerator TeleportPlayerCoroutine(Vector3 newPosition)
    {
        yield return null;
        isDashing = false;
        transform.position = newPosition;
    }

    private float EasingSmoothSquared(float x)
    {
        return x < 0.5f ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
    }
}