using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public Vector2 _direction;
    public Vector2 _velocity;
    public Rigidbody2D _rb2D;
    private bool _isJumped = false;
    private bool _isGrounded = false;
    private bool _isDashed = false;
    private bool _isPlanned = false;
    private bool _isInterracting = false;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Vector2 groundCheckerSize = Vector2.one;
    public Transform groundCheckerTransform;

    [SerializeField]
    public float currentSpeed;
    public float moveSpeed;
    public float airSpeed;
    public float boostSpeed;
    public enum States
    {
        ILDE, RUN, JUMP, DASH, FALL, FLY
    }

    public States currentStates = States.ILDE;
    void Start()
    {
        _rb2D.freezeRotation = true;
    }

    void Update()
    {
        OnStatesUpdate();
        Collider2D ground = Physics2D.OverlapBox(groundCheckerTransform.position, groundCheckerSize, 0f, groundLayer);

        if (ground != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
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
                currentSpeed = airSpeed;
                _rb2D.velocity = new Vector2(_rb2D.velocity.x, 3f);
                break;
            case States.DASH: 
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
                break ;
            case States.JUMP:
                _velocity.x = _direction.x * currentSpeed;
                _velocity.y = _rb2D.velocity.y;
                _rb2D.velocity = _velocity;
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
            case States.DASH:
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
                _isJumped = false;
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
}
