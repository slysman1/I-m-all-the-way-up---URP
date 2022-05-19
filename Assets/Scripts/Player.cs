using UI_InputSystem.Base;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Player : MonoBehaviour, IKnockable
{
    [SerializeField] ParticleSystem[] particle;
    [SerializeField] ShakeData[] shakeData;

    public static Player instance;
    public int feathers;
    
    private Rigidbody2D rb;
    private Animator anim;
    



    private bool isRespawn;

  

    [Header("Impact info")]

    [SerializeField] private float impactRadius;
    [SerializeField] private LayerMask whatIsImpactable;
    [SerializeField] private LayerMask whatIsImpactableProps;
    [SerializeField] private float cageImpactPower;

    [Header("JetPack info")]
    [SerializeField] private float jetPuckSpeed;
    [SerializeField] private float jetPackTime;
    [SerializeField] GameObject jetPackSkin;
    private bool isJetPackOn;
    private float jetPackCountdown;

    [Header("Knockback info")]
    private bool isKnocked;

    [Header("Move and Jump info")]
    public float moveSpeed = 12f;
    private float movingDirection;
    private int facingDirection;
    private bool canMove;


    public float jumpForce = 15f;
    public float doubleJumpForce = 15f;
    private float defaultJumpForce;

    [SerializeField] private Vector2 wallJumpDirection;

    private bool canImpactOnLand;
    //private bool canJump;
    private bool canWallJump;
    private bool canDoubleJump;
    private float animFallVariant;


    #region Buffer jump && cayoteTime && ledge climb

    private float jumpBufferCountdown;
    [SerializeField] private float jumpBufferTime;

    private float cayoteCountdown;
    private bool canHaveCayoteJump;
    [SerializeField] private float cayoteTime;

    private bool canMoveAlongeTheWall;
    private bool canCountMoveAlongTheWall;
    public float moveAlongTheWallCountdown;
    [SerializeField] private float moveAlongTheWallBufferTime;

    #endregion

    [Header("Status info")]
    private bool isMoving;
    private bool isWallSliding;
    private bool canBeControlled;


    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;

    private bool isGrounded;
    private bool isWallDetected;
    private bool canWallSlide;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }


    private void Start()
    {


        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        AllowPlayerMovement();
        defaultJumpForce = jumpForce;


        jetPackSkin.SetActive(false);
    }

    public float timePlayed;    
    
    private void Update()
    {
        timePlayed += 1 * Time.deltaTime;
        jumpBufferCountdown -= 1 * Time.deltaTime;
        cayoteCountdown -= 1 * Time.deltaTime;
        moveAlongTheWallCountdown -= 1 * Time.deltaTime;


        if (transform.position.x >= 4.5f)
        {
            transform.position = new Vector3(-3.21f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -3.2f)
        {
            transform.position = new Vector3(4.5f, transform.position.y, transform.position.z);
        }



        if (canBeControlled)
            CheckInput();
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("SHAKE");
            ShakerInstance instance = CameraShakerHandler.Shake(shakeData[1]);
        }


        FlipControl();
        AnimationsController();
        CollisionChecks();

        if (!isKnocked)
        {

            if (isJetPackOn)
            {
                DoCageImpact();
                JetPuckMovement();
            }
            else
            {
                ApplyMovement();
            }
        }

        if (rb.velocity.y < 0 && !isGrounded)
            canImpactOnLand = true;
    }

    private void FixedUpdate()
    {
        
    }

    #region Anim info
    public void RespawnPlayer()
    {
        isRespawn = true;
    }

    public void AllowPlayerMovement()
    {
        canBeControlled = true;
        isRespawn = false;
        anim.SetBool("respawnOver", true);
    }
    private void AnimationsController()
    {
        isMoving = rb.velocity.x != 0;


        anim.SetFloat("fallAnimVariant", animFallVariant);
        anim.SetFloat("facingDirection", facingDirection);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSlide", isWallSliding);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isRespawn", isRespawn);
    }
    #endregion
    #region Inputs
    public bool PC;

    private void OnEnable()
    {
        UIInputSystem.ME.AddOnTouchEvent(ButtonAction.Jump, JumpButton);
    }

    private void OnDisable()
    {
        UIInputSystem.ME.RemoveOnTouchEvent(ButtonAction.Jump, JumpButton);
    }


    private void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }

        if (canMove)
        {


            if (PC)
                movingDirection = Input.GetAxisRaw("Horizontal");
            else
                movingDirection = UIInputSystem.ME.GetAxisHorizontal(JoyStickAction.Movement);
        }

        //if(movingDirection == 0)
        //    rb.velocity = new Vector2(0, rb.velocity.y);

        if (Mathf.Abs(rb.velocity.x) < 1)
            rb.velocity = new Vector2(0, rb.velocity.y);
        //if (rb.velocity.y > -1)
        //    rb.velocity = new Vector2(rb.velocity.x, 0);
    }


    #endregion

    public void DoPropsImpact()
    {
        //if not grounded for 2 seconds
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, impactRadius + 0.2f, whatIsImpactableProps);

        foreach (Collider2D collider in detectedObjects)
        {
            var impactable = collider.GetComponent<IImpactable>();

            if (impactable != null)
            {
                
            }
        }
    }
    public void DoCageImpact()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, impactRadius, whatIsImpactable);

        foreach (Collider2D collider in detectedObjects)
        {
            var impactable = collider.GetComponent<IImpactable>();

            if (impactable != null)
            {
                

                bool playerAboveCage = impactable.myTopPoint() < transform.position.y;
                bool playerAboveMiddlePoint = impactable.myTopPoint() / 2 < transform.position.y;
                bool impactableOnRight = impactable.myXposition() < transform.position.x;
                int impactDirection;

                if (impactableOnRight)
                    impactDirection = -1;
                else
                    impactDirection = 1;


                if (playerAboveCage)
                    return;
                else if (playerAboveMiddlePoint)
                {
                    impactable.TakeImpact(cageImpactPower * 0.4f * impactDirection);
                }
                else
                {
                 
                    impactable.TakeImpact(cageImpactPower * impactDirection);
                }
            }
        }
    }
    #region JetPack Pick up
    private void JetPuckMovement()
    {
        rb.velocity = new Vector2(movingDirection * moveSpeed, jetPuckSpeed);

        jetPackCountdown -= 1 * Time.deltaTime;

        if (jetPackCountdown < 0)
        {
            isJetPackOn = false;
        }

        jetPackSkin.SetActive(isJetPackOn);
        
    }
    public void JetPackOn()
    {
        rb.velocity = new Vector2(0, 0);
        isJetPackOn = true;
        jetPackCountdown = jetPackTime;
        ShakerInstance instance = CameraShakerHandler.Shake(shakeData[0]);
    }
    #endregion

    private void ToDoOnLanding()
    {
        if (canImpactOnLand)
        {
            //PlayParticle(dustFX);

            DoPropsImpact();
            canImpactOnLand = false;
        }


        Time.timeScale = 1;
        rb.velocity = new Vector2(0, rb.velocity.y);

        canHaveCayoteJump = true;
        canDoubleJump = true;
        canWallJump = true;
        canMove = true;
        //canJump = true;

        isWallSliding = false;
        canWallSlide = false;

        if (jumpBufferCountdown >= 0)
        {
            Jump();
            jumpBufferCountdown = -1;
        }
    }

    private void ApplyMovement()
    {
        

        if (isGrounded)
        {
            ToDoOnLanding();
        }
        else
        {
            canImpactOnLand = false;

            if (canHaveCayoteJump)
                cayoteCountdown = cayoteTime;
        }


        // get off wall 
        if (isWallSliding && Input.GetAxisRaw("Horizontal") != 0)
        {

            if (Input.GetAxisRaw("Horizontal") > 0 && facingDirection == -1)
            {
                isWallSliding = false;
                canWallSlide = false;

                Vector2 direction = new Vector2(4 * -facingDirection, 3);
                rb.AddForce(direction, ForceMode2D.Impulse);

                Time.timeScale = 0.8f;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0 && facingDirection == 1)
            {
                isWallSliding = false;
                canWallSlide = false;

                Vector2 direction = new Vector2(4 * -facingDirection, 3);
                rb.AddForce(direction, ForceMode2D.Impulse);

                Time.timeScale = 0.8f;
            }

        }

        if (canWallSlide && isWallDetected)
        {
            isWallSliding = true;
            canHaveCayoteJump = false;
            canMoveAlongeTheWall = true;
            canCountMoveAlongTheWall = true;

            rb.velocity = new Vector2(0, rb.velocity.y);



            if (rb.velocity.y < 0)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
        else if (!isWallDetected)
        {
            Move();

            if (canCountMoveAlongTheWall)
            {

                canCountMoveAlongTheWall = false;
                moveAlongTheWallCountdown = moveAlongTheWallBufferTime;
            }
        }


        if (moveAlongTheWallCountdown < 0)
        {
            anim.SetBool("climbUpTheWall", false);
            canMoveAlongeTheWall = false;
        }
        else if (canMoveAlongeTheWall && moveAlongTheWallCountdown > 0)
        {
            anim.SetBool("climbUpTheWall", true);
            Debug.Log("trying to go there");
            rb.velocity = new Vector2(4 * facingDirection, rb.velocity.y);
        }

    }
    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movingDirection * moveSpeed, rb.velocity.y);
        }
    }
    #region Jump info
    private void JumpButton()
    {
        #region Choose fall animation
        if (UnityEngine.Random.value < 0.5f)
            animFallVariant = -1;
        else
            animFallVariant = 1;
        #endregion
        canMoveAlongeTheWall = false;
        Time.timeScale = 1;

        canHaveCayoteJump = false;
        cayoteCountdown = -1;

        if (!isGrounded)
        {
            jumpBufferCountdown = jumpBufferTime;
        }

        if (isWallSliding && canWallJump) // wall jump
        {

            WallJump();
        }
        else if (isGrounded == true || cayoteCountdown > 0)// && canJump) // regular jump
        {
            Jump();
        }
        else if (canDoubleJump == true) // double jump
        {
            jumpForce = doubleJumpForce;
            Jump();
            canMove = true;
            canDoubleJump = false;
            jumpForce = defaultJumpForce;
        }

        DoCageImpact();

        isWallSliding = false;
    }

    private void Jump()
    {
        PlayParticle(particle[0]);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        PlayParticle(particle[0]);
        canMoveAlongeTheWall = false;
        canDoubleJump = true;
        canWallSlide = false;

        Vector2 direction;
        direction = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);




        rb.AddForce(direction, ForceMode2D.Impulse);

        canMove = false;
    }

    #endregion
    private void FlipControl()
    {
        if (rb.velocity.x != 0 && isGrounded)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(0, 0, movingDirection));
            transform.rotation = newRotation;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
        }

        if (!isGrounded && !isWallSliding)
        {
            if (rb.velocity.x > 0)
            {
                facingDirection = 1;
            }
            else if (rb.velocity.x < 0)
            {
                facingDirection = -1;
            }
        }
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, whatIsGround);


        if (facingDirection == 1 && !isGrounded)
            isWallDetected = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckDistance, whatIsGround);
        else if (facingDirection == -1 && !isGrounded)
            isWallDetected = Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckDistance, whatIsGround);
        else
            isWallDetected = false;

        if (!isGrounded)//&& rb.velocity.y < 0)
            canWallSlide = true;
    }


    public void Knockback(Vector2 direction, float power)
    {
        PlayParticle(particle[1]);
        ShakerInstance instance = CameraShakerHandler.Shake(shakeData[1]);

        isKnocked = true;
        canMove = false;

        Time.timeScale = 0.7f;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(direction * power, ForceMode2D.Impulse);

        Invoke("CancelKnockback", 0.45f);
    }

    private void CancelKnockback()
    {
        isKnocked = false;
        canMove = true;
        Time.timeScale = 1;
    }



    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckDistance);
        Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckDistance);
        Gizmos.DrawWireSphere(transform.position, impactRadius);
        //Gizmos.DrawWireSphere(breaktrhoughCheck.position, breaktrhoRadius);
    }


    private void PlayParticle(ParticleSystem particle)
    {
        particle.Play();
    }
}
