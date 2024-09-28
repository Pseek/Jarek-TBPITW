using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variable")]
    public GM gM;
    public StoreBumper sB;
    private Vector2 _direction;
    private Vector2 _velocity;
    public Rigidbody2D _rb2D;
    public float gravityPlane;
    public bool _isInterracting;
    public float fallDumpsterSpeed;
    public bool canGetUp = false;
    public bool isFallDumpster = false;
    public float buffSpeed;
    public Animator m_Animator;
    public bool isDogged = false;
   
    [Header("SoundEffect")]
    public AudioSource aS;
    public AudioClip acJump;
    public AudioClip acFly;
    public AudioClip acDash;
    public AudioClip acWallJump;

    [Header("Jump")]
    public bool _isFly = false;
    public bool _isJumped = false;
    private bool _isGrounded = false;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpForce;
    public float coyoteTime;
    private float _coyoteCounterTime;
    private int _canCoyote;
    private float flySpeed;
    private float jumpSpeed;

    [Header("Dash")]
    public float dashForce;
    public float dashDuration;
    public float chronoDash;
    private bool _isDashed = false;
    public bool _canDash = false;
    public TrailRenderer _tRD; 
    private Vector2 dashDir;
     
    [Header("WallJump")]
    public bool isSlinding;
    private bool _isOnWall = false;
    public float wallSlideSpeed;
    public Vector2 wallJumpForce;
    public float wallJumpDuration;
    public float wallJumpChrono;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Vector2 groundCheckerSize = Vector2.one;
    public Transform groundCheckerTransform;

    [Header("WallDectection")]
    public LayerMask wallLayer;
    public Vector2 wallCheckerSize = Vector2.one;
    public Transform wallCheckerTransformRight;

    [Header("SlopeDectection")]
    public LayerMask slopeLayer;
    public Vector2 slopeCheckerSize = Vector2.one;
    public Transform slopeCheckerTransformRight;

    [Header("Speed")]
    public float moveSpeed;
    public float currentSpeed;
    public float decelSpeed;
    public Vector2 decelDir;

    [Header("TutoSlope")]
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    [SerializeField]
    private PhysicsMaterial2D zeroF;
    [SerializeField]
    private PhysicsMaterial2D fullF;
    [SerializeField]
    private PhysicsMaterial2D normalF;
    private CapsuleCollider2D cc;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerp;
    private Vector2 capsuleColliderSize;
    private bool isOnSlope;
    private bool canWalkOnSlope;

    public enum States
    {
        ILDE, RUN, JUMP, DASH, FLY, WALLJUMP, WALLSLIDE, FALL, FALLDUMPSTER
    }

    public States currentStates = States.ILDE;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        currentSpeed = moveSpeed;
        _rb2D.freezeRotation = true;
        cc = GetComponent<CapsuleCollider2D>();

        capsuleColliderSize = cc.size;
    }

    void Update()
    {
        FlipGraphics();
        MenuPauseND();
        WallCheck();
        GroundCheck();
        OnStatesUpdate();
        SlopeCheck();
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
            {
                canWalkOnSlope = false;
            }
            else
            {
                canWalkOnSlope = true;
            }

            if(isOnSlope && canWalkOnSlope && _direction.x == 0.0f)
            {
                _rb2D.sharedMaterial = fullF;
            }
            else if (isOnSlope && canWalkOnSlope && _direction.x != 0.0f)
            {
                _rb2D.sharedMaterial = zeroF;
            }
            else
            {
                _rb2D.sharedMaterial = normalF;
            }
        }
    }
    public void MenuPauseND()
    {
        if (Time.timeScale == 0f)
        {
            _direction.x = 0f;
        }
    }

    public void GroundCheck()
    {
        Collider2D ground = Physics2D.OverlapBox(groundCheckerTransform.position, groundCheckerSize, 0f, groundLayer);

        if (ground != null)
        {
            _isGrounded = true;
            _canDash = true;
        }
        else
        {
            _isGrounded = false;
        }
        m_Animator.SetBool("IsGrounded", _isGrounded);
    }

    public void WallCheck()
    {
        Collider2D wallRight = Physics2D.OverlapBox(wallCheckerTransformRight.position, wallCheckerSize, 0f, wallLayer);

        if (wallRight != null)
        {
            _isOnWall = true;
            _canDash = true;
        }
        else
        {
            _isOnWall = false;
        }
    }

    public void FlipGraphics()
    {
        if (_velocity.x < 0f)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (_velocity.x > 0f)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (_isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube(groundCheckerTransform.position, groundCheckerSize);

        if (_isOnWall)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube(wallCheckerTransformRight.position, wallCheckerSize);
    }

    public void OnStatesEnter()
    {
        switch (currentStates)
        {
            case States.ILDE:
                
                currentSpeed = moveSpeed;
                _canCoyote = 0;
                m_Animator.SetFloat("VelocityX", 0f);
                _isJumped = false;
                break;
            case States.RUN:
                _canCoyote = 0;
                m_Animator.SetFloat("VelocityX", 1f);
                _isJumped = false;
                currentSpeed = moveSpeed;
                break;
            case States.JUMP:
                if (gM.isPause)
                {
                    aS.mute = true;
                }
                else
                {
                    aS.mute = false;
                }
                jumpSpeed = currentSpeed;
                aS.clip = acJump;
                aS.Play();
                _isFly = false;
                m_Animator.SetFloat("VelocityY", 0.1f);
                _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
                break;
            case States.FALL:
                _isJumped = false;
                m_Animator.SetFloat("VelocityY", 0.5f);
                break;
            case States.FALLDUMPSTER:
                m_Animator.SetBool("IsFallDumpster", true);
                isFallDumpster = false;
                break;
            case States.WALLJUMP:
                m_Animator.SetFloat("VelocityY", 0.1f);
                aS.clip = acWallJump;
                aS.Play();
                _rb2D.velocity = new Vector2(-_direction.x * wallJumpForce.x, wallJumpForce.y);
                wallJumpChrono = 0;
                break;
            case States.WALLSLIDE:
                m_Animator.SetBool("IsWallSlide", true); 
                _isJumped = false;
                currentSpeed = moveSpeed;
                break;
            case States.DASH:
                aS.clip = acDash;
                aS.Play();
                _tRD.emitting = true;
                dashDir = _direction;
                chronoDash = 0f;
                _rb2D.gravityScale = 0f;
                _canDash = false;
                _isDashed = false;
                break;
            case States.FLY:
                flySpeed = currentSpeed;
                aS.loop = true;
                aS.clip = acFly;
                aS.Play();
                m_Animator.SetBool("IsFly", true);
                break;
        }
    }
    public void OnStatesUpdate()
    {
        switch (currentStates)
        {
            case States.ILDE:
                if (_direction.magnitude > 0f)
                {
                    TransitionToStates(States.RUN);
                }
                if (_isJumped && _isGrounded)
                {
                    TransitionToStates(States.JUMP);
                }
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                if (_rb2D.velocity.y < 0f && !_isGrounded)
                {
                    ++_canCoyote;
                    TransitionToStates(States.FALL);
                }
                break;
            case States.RUN:
                if (_direction.magnitude == 0f)
                {
                    currentSpeed -= Time.deltaTime * decelSpeed;
                    currentSpeed = Mathf.Max(0f, currentSpeed);
                    _velocity.x = decelDir.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                }
                else if (_isGrounded && isOnSlope && !_isJumped && canWalkOnSlope)
                { 
                    currentSpeed = moveSpeed;
                    _velocity.x = -_direction.x * currentSpeed * slopeNormalPerp.x;
                    _velocity.y = -_direction.x * currentSpeed * slopeNormalPerp.y;
                    decelDir = _direction;
                }
                else 
                {
                    currentSpeed = moveSpeed;
                    _velocity.x = _direction.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                    decelDir = _direction;
                    
                }

                
                _rb2D.velocity = _velocity;

                if (currentSpeed == 0f)
                {
                    TransitionToStates(States.ILDE);
                }
                if (_isJumped && _isGrounded)
                {
                    TransitionToStates(States.JUMP);
                }
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }
                if (_rb2D.velocity.y < 0f && !_isGrounded)
                {
                    ++_canCoyote;
                    TransitionToStates(States.FALL);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                if (sB.isStored)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, sB.bumperForce);
                    TransitionToStates(States.FALL);
                }
                break;
            case States.JUMP:
                if (_direction.magnitude == 0f)
                {
                    currentSpeed -= Time.deltaTime * decelSpeed;
                    currentSpeed = Mathf.Max(0f, currentSpeed);
                    _velocity.x = decelDir.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                }
                else
                {
                    currentSpeed = jumpSpeed;
                    _velocity.x = _direction.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                    decelDir = _direction;
                }
                _rb2D.velocity = _velocity;

                if (_rb2D.velocity.y > 0f && !_isJumped)
                {
                    _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
                   
                }
                if (_isFly && _rb2D.velocity.y >= 0f)
                {
                    TransitionToStates(States.FLY);
                }
                if (_rb2D.velocity.y < 0f && !_isGrounded)
                    {                             
                        TransitionToStates(States.FALL);
                    }   
                if (_isOnWall && !_isGrounded && _direction.magnitude != 0f)
                {
                    TransitionToStates(States.WALLSLIDE);
                }
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                if (sB.isStored)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, sB.bumperForce);
                    TransitionToStates(States.FALL);
                }

                if (_rb2D.velocity.y < 0f && _isGrounded)
                {
                    TransitionToStates(States.FALL);
                }
                break;
            case States.FALL:
                if (_direction.magnitude == 0f)
                {
                    currentSpeed -= Time.deltaTime * decelSpeed;
                    currentSpeed = Mathf.Max(0f, currentSpeed);
                    _velocity.x = decelDir.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                }
                else
                {
                    currentSpeed = jumpSpeed;
                    _velocity.x = _direction.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                    decelDir = _direction;
                }
                _rb2D.velocity = _velocity;
                _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
                _coyoteCounterTime += Time.deltaTime;
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }
                if (_coyoteCounterTime < coyoteTime && _isJumped && _canCoyote == 1)
                {
                    --_canCoyote;
                    TransitionToStates(States.JUMP);
                }
                else if (_isJumped && !_isGrounded)
                {
                    _canCoyote = 0;
                    TransitionToStates(States.FLY);
                }
                if (_isOnWall && !_isGrounded && _direction.x != 0)
                {
                    TransitionToStates(States.WALLSLIDE);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                if (_isGrounded)
                {
                    if (_direction.magnitude > 0f)
                    {
                        TransitionToStates(States.RUN);
                    }
                    if (_direction.magnitude == 0f)
                    {
                        TransitionToStates(States.ILDE);
                    }
                }
                if (sB.isStored)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, sB.bumperForce);
                    TransitionToStates(States.FALL);
                }
                break;
            case States.FALLDUMPSTER:
                if (_isJumped && canGetUp && _isGrounded)
                {
                    StartCoroutine(GetUpDumpster());
                    StopCoroutine(FallDumpster());
                    TransitionToStates(States.ILDE);
                }
                break;
            case States.WALLSLIDE:
                _rb2D.velocity = new Vector2(_rb2D.velocity.x, Mathf.Clamp(_rb2D.velocity.y, -wallSlideSpeed, float.MaxValue));
                if (_isJumped && _direction.magnitude > 0f)
                {
                    TransitionToStates(States.WALLJUMP);
                }
                if (_direction.magnitude == 0f)
                {
                    TransitionToStates(States.ILDE);
                }
                if (_direction.magnitude > 0f && !_isOnWall)
                {
                    TransitionToStates(States.FALL);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }

                break;
            case States.WALLJUMP:
                wallJumpChrono += Time.deltaTime;
                if (_rb2D.velocity.y < 0f || wallJumpChrono > wallJumpDuration)
                {
                    TransitionToStates(States.FALL);
                }
                if (sB.isStored)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, sB.bumperForce);
                    TransitionToStates(States.FALL);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                break;
            case States.DASH:
                chronoDash += Time.deltaTime;
                _rb2D.velocity = new Vector2(Mathf.Clamp(dashDir.x, -1, 1), Mathf.Clamp(dashDir.y, -1, 1)) * dashForce;
                if (_isJumped && _isGrounded)
                {
                    currentSpeed = dashForce;
                    TransitionToStates(States.JUMP);
                }
                else if (chronoDash > dashDuration)
                {
                    TransitionToStates(States.FALL);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                if (_isOnWall && !_isGrounded && _direction.magnitude != 0f)
                {
                    TransitionToStates(States.WALLSLIDE);
                }
                if (sB.isStored)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, sB.bumperForce);
                    TransitionToStates(States.FALL);
                }
                break;
            case States.FLY:
                if (_direction.magnitude == 0f)
                {
                    currentSpeed -= Time.deltaTime * decelSpeed;
                    currentSpeed = Mathf.Max(0f, currentSpeed);
                    _velocity.x = decelDir.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                }
                else
                {
                    currentSpeed = flySpeed;
                    if (currentSpeed == 0f)
                    {
                        currentSpeed = moveSpeed;
                    }
                    _velocity.x = _direction.x * currentSpeed;
                    _velocity.y = _rb2D.velocity.y;
                    decelDir = _direction;
                }
                _velocity.y = -gravityPlane;
                _rb2D.velocity = _velocity;
                if (!_isJumped)
                {
                    TransitionToStates(States.FALL);
                }
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }
                if(sB.isStored)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, sB.bumperForce);
                    TransitionToStates(States.FALL);
                }
                if (isFallDumpster)
                {
                    TransitionToStates(States.FALLDUMPSTER);
                }
                if (_isGrounded)
                {
                    if (_direction.magnitude > 0f)
                    {
                        TransitionToStates(States.RUN);
                    }
                    if (_direction.magnitude == 0f)
                    {
                        TransitionToStates(States.ILDE);
                    }
                }
                break;
        }
    }
    public void OnStatesExit()
    {
        switch (currentStates)
        {
            case States.ILDE:
                break;
            case States.RUN:
                decelDir.x = 0f;
                break;
            case States.JUMP:
                aS.Stop();
                break;
            case States.FALL:
                decelDir.x = 0f;
                m_Animator.SetFloat("VelocityY", 0f);
                _coyoteCounterTime = 0f;
                break;
            case States.FALLDUMPSTER:
                m_Animator.SetBool("IsFallDumpster", false);
                isDogged = false;
                break;
            case States.WALLSLIDE:
                m_Animator.SetBool("IsWallSlide", false);
                break;
            case States.WALLJUMP:
                aS.Stop();
                _rb2D.velocity = Vector2.zero;
                break;
            case States.DASH:
                aS.Stop();
                _tRD.emitting = false;
                _rb2D.velocity = Vector2.zero;
                _rb2D.gravityScale = 1.75f;
                break;
            case States.FLY:
                m_Animator.SetBool("IsFly", false);
                decelDir.x = 0f;
                aS.loop = false;
                aS.Stop();
                break;
        }
    }

    public void TransitionToStates(States newStates)
    {
        OnStatesExit();
        currentStates = newStates;
        OnStatesEnter();
    }

    public void Move(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _direction = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                _direction = Vector2.zero;
                break;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _isDashed = true;
                break;
            case InputActionPhase.Canceled:
                _isDashed = false;
                break;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _isFly = true;
                break;
            case InputActionPhase.Performed:
                _isJumped = true;
                break;
            case InputActionPhase.Canceled:
                _isJumped = false;
                _isFly = false;
                break;
        }
    }

    public void Interract(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _isInterracting = true;
                break;
            case InputActionPhase.Canceled:
                _isInterracting = false;
                break;
        }
    }
    public void OpenMap(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                gM.isPause = true;
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }

    IEnumerator FallDumpster()
    {
        currentSpeed = fallDumpsterSpeed;
        yield return new WaitForSeconds(0.5f);
        currentSpeed = 0;
        _direction.x = 0;
        canGetUp = true;
    }

    IEnumerator GetUpDumpster()
    {
        yield return new WaitForSeconds(1f);
        canGetUp = false;
        currentSpeed = moveSpeed;
        StopCoroutine(GetUpDumpster());
    }
    public void StartFallDumpster()
    {
        isFallDumpster = true;
        StartCoroutine(FallDumpster());
    }

    IEnumerator GetSpeedUp()
    {
        moveSpeed = buffSpeed;
        yield return new WaitForSeconds(15f);
        moveSpeed = 7.5f;
        StopCoroutine(GetSpeedUp());
    }

    public void GetBuffSpeed()
    {
        StartCoroutine(GetSpeedUp());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            StartFallDumpster();
            isDogged = true;
        }
    }
}
