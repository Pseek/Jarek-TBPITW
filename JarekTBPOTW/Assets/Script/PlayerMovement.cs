using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variable")]
    private Vector2 _direction;
    private Vector2 _velocity;
    public Rigidbody2D _rb2D;
    public float gravityPlane;

    [Header("Jump")]
    private bool _isJumped = false;
    private bool _isGrounded = false;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Range(1f,10f)]
    public float jumpForce;

    [Header("Dash")]
    private Vector2 dashDir;
    public float dashForce;
    public float dashDuration;
    public float chronoDash;
    private bool _isDashed = false;
    private bool _canDash = false;

    [Header("WallJump")]
    private bool _isOnWall = false;
    public bool isSlinding;
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
    public Transform wallCheckerTransformLeft;
    public Transform wallCheckerTransformRight;

    [Header("Speed")]
    public float moveSpeed;
    public float currentSpeed;
    public enum States
    {
        ILDE, RUN, JUMP, DASH, FLY ,WALLJUMP, WALLSLIDE, FALL
    }

    public States currentStates = States.ILDE;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        _rb2D.freezeRotation = true;
    }

    void Update()
    {
        Collider2D ground = Physics2D.OverlapBox(groundCheckerTransform.position, groundCheckerSize, 0f, groundLayer);
        Collider2D wallLeft =  Physics2D.OverlapBox(wallCheckerTransformLeft.position, wallCheckerSize, 0f , wallLayer);
        Collider2D wallRight = Physics2D.OverlapBox(wallCheckerTransformRight.position, wallCheckerSize, 0f, wallLayer);

        if (ground != null)
        {
            _isGrounded = true;
            _canDash = true;
        }
        else
        {  
            _isGrounded = false;
        }

        if (wallLeft != null || wallRight != null)
        {
            _isOnWall = true;
            _canDash = true;
        }
        else
        {
            _isOnWall= false;
        }

        if (_velocity.x < 0f)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (_velocity.x > 0f)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        OnStatesUpdate();
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

        Gizmos.DrawCube(wallCheckerTransformLeft.position, wallCheckerSize);
        Gizmos.DrawCube(wallCheckerTransformRight.position, wallCheckerSize);
    }

    public void OnStatesEnter()
    {
        switch(currentStates)
        {
            case States.ILDE:
                _isJumped = false;
                break;
            case States.RUN:
                _isJumped = false;
                currentSpeed = moveSpeed;
                break;
            case States.JUMP:
                _rb2D.velocity = new Vector2(_rb2D.velocity.x,jumpForce);
                break;
            case States.FALL:
                
                break;
            case States.WALLJUMP:
                _rb2D.velocity = new Vector2(-_direction.x*wallJumpForce.x, wallJumpForce.y);
                wallJumpChrono = 0;
                break;
            case States.WALLSLIDE:
                _isJumped = false;
                currentSpeed = moveSpeed;
                break;
            case States.DASH:
                dashDir = _direction;
                chronoDash = 0f;
                _rb2D.gravityScale = 0f;
                _canDash = false;
                _isDashed = false;
                break;
            case States.FLY:
                _rb2D.gravityScale = -gravityPlane;
                break;
        }
    }
    public void OnStatesUpdate()
    {
        switch(currentStates) 
        {
            case States.ILDE:
                if(_direction.magnitude > 0f)
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
                break ;
            case States.RUN:
                _velocity.x = _direction.x * currentSpeed;
                _velocity.y = _rb2D.velocity.y;
                _rb2D.velocity = _velocity;

                if (_direction.magnitude == 0f)
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
                    TransitionToStates(States.FALL);
                }
                break;
            case States.JUMP:
                _velocity.x = _direction.x * currentSpeed;
                _velocity.y = _rb2D.velocity.y;
                _rb2D.velocity = _velocity; 
                
                if (_rb2D.velocity.y > 0f && !_isJumped)
                {
                    _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
                } 
                if (_rb2D.velocity.y < 0f && !_isGrounded)
                {
                    TransitionToStates(States.FALL);
                }
                if ( _isOnWall && !_isGrounded && _direction.magnitude != 0f)
                {
                    TransitionToStates(States.WALLSLIDE);
                }
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }
                break;
            case States.FALL:
                _velocity.x = _direction.x * currentSpeed;
                _velocity.y = _rb2D.velocity.y;
                _rb2D.velocity = _velocity;
                _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }

                if (_isJumped && !_isGrounded)
                {
                    TransitionToStates(States.FLY);
                }
                if (_isOnWall && !_isGrounded && _direction.x != 0)
                {
                    TransitionToStates(States.WALLSLIDE);
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

                break;
            case States.WALLJUMP:
                wallJumpChrono += Time.deltaTime;
                if (_rb2D.velocity.y < 0f || wallJumpChrono > wallJumpDuration)
                {
                    TransitionToStates(States.FALL);
                }
                break;
            case States.DASH:
                chronoDash += Time.deltaTime;
                _rb2D.velocity = new Vector2(Mathf.Clamp(dashDir.x,-1,1),Mathf.Clamp(dashDir.y,-1,1)) * dashForce;
                if(_isJumped && _isGrounded)
                {
                    currentSpeed = dashForce;
                    TransitionToStates(States.JUMP);
                } 
                else if(chronoDash > dashDuration)
                {
                    TransitionToStates(States.ILDE);
                }
                if(_rb2D.velocity.y < 0f && !_isGrounded)
                {
                    TransitionToStates(States.FALL);
                }
                
                break;
            case States.FLY:
                _velocity.x = _direction.x * currentSpeed;
                _velocity.y = -gravityPlane;
                _rb2D.velocity = _velocity;
                if (!_isJumped)
                {
                    TransitionToStates(States.FALL);
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
                break;
            case States.JUMP:
                break;
            case States.FALL:
                break;
            case States.WALLSLIDE:
                break;
            case States.WALLJUMP:
                _rb2D.velocity = Vector2.zero;
                break;
            case States.DASH:
                _rb2D.velocity = Vector2.zero;
                _rb2D.gravityScale = 1f;
                break;
            case States.FLY:
                _rb2D.gravityScale = 1f;
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
        switch(context.phase) 
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
            case InputActionPhase.Performed:

                _isJumped = true;
                break;
            case InputActionPhase.Canceled:
                _isJumped = false;
                break;
        }
    }
}
