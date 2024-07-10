using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _direction;
    private Vector2 _velocity;
    public Rigidbody2D _rb2D;
    
    //private bool _isPlanned = false;
    //private bool _isInterracting = false;
    

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

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Vector2 groundCheckerSize = Vector2.one;
    public Transform groundCheckerTransform;

    [Header("Speed")]
    public float moveSpeed;
    public float currentSpeed;
    public enum States
    {
        ILDE, RUN, JUMP, DASH, FALL, FLY
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

        if (ground != null)
        {
            _isGrounded = true;
            _canDash = true;
        }
        else
        {  
            _isGrounded = false;
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

    }

    public void OnStatesEnter()
    {
        switch(currentStates)
        {
            case States.ILDE: 
                break;
            case States.RUN:
                currentSpeed = moveSpeed;
                break;
            case States.JUMP:
                _rb2D.velocity = new Vector2(_rb2D.velocity.x,jumpForce);
                break;
            case States.DASH:
                dashDir = _direction;
                chronoDash = 0f;
                _rb2D.gravityScale = 0f;
                _canDash = false;
                _isDashed = false;
                break;
            case States.FLY: 
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
                break ;
            case States.JUMP:
                _velocity.x = _direction.x * currentSpeed;
                _velocity.y = _rb2D.velocity.y;
                _rb2D.velocity = _velocity;

                if(_rb2D.velocity.y < 0f)
                {
                    _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
                }
                else if (_rb2D.velocity.y > 0f && !_isJumped)
                {
                    _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
                }

                if (_canDash && _isDashed)
                {
                    TransitionToStates(States.DASH);
                }

                if (_isGrounded && _rb2D.velocity.y == 0f)
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
                
                break;
            case States.FLY:
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
            case States.DASH:
                _rb2D.velocity = Vector2.zero;
                _rb2D.gravityScale = 1f;
                break;
            case States.FLY:
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
                //Debug.Log("je saute");
                _isJumped = false;

                break;
        }
    }

    public void Interract(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //_isInterracting = true;
                break;
            case InputActionPhase.Canceled:
                //_isInterracting = false;
                break;
        }
    }
}
