using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Dog : MonoBehaviour
{
    public MailBox mB;
    public PlayerMovement pM;
    public Transform currentTarget;
    public Transform positionPlayer;
    public Transform positionGarde;
    public Animator animDog;
    public PositionTriggerDog pTD;

    private float chronoToAttack;
    [SerializeField] private float timerToAttack;
    [SerializeField] private bool dogHasAttacked = false;


    public float moveSpeedRTT;
    public float moveSpeedWTG;

    [SerializeField] private LayerMask layerLimitDog;
    [SerializeField] private Transform boxDetectionLimitDog;
    [SerializeField] private Vector2 sizeBoxDetectionLimitDog;
    [SerializeField] private bool isLimit = false;

    public bool isEnterDogDetection = false;
    public bool isExitDogDetection = false;
    public bool isStayDogDetection = false;

    public float speedEnemy;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    public AudioClip acDogBark;
    public AudioSource asDog;

    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2D;

    public DogStates currentDogStates = DogStates.SLEEPING;
    public enum DogStates
    {
        SLEEPING, GARDE, RUNTOTARGET, WALKTOGARDE
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2D = GetComponent<Rigidbody2D>();
        gameObject.transform.position = positionGarde.position;
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())   
            seeker.StartPath(rb2D.position, currentTarget.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private void Update()
    {
        OnStatesUpdate();
        LimitDogDetection();
    }

    public void LimitDogDetection()
    {
        Collider2D limitDogDectection = Physics2D.OverlapBox(boxDetectionLimitDog.position, sizeBoxDetectionLimitDog,0f, layerLimitDog);

        if(limitDogDectection != null)
        {
            isLimit = false;
        }
        else 
        { 
            isLimit = true;
        }
    }

    public void OnDrawGizmos()
    {
        if (isLimit)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube(boxDetectionLimitDog.position, sizeBoxDetectionLimitDog);
    }

    public void OnTargetPath()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2D.position).normalized;
        Vector2 movementDog = new Vector2 (direction.x * speedEnemy, direction.y * 0.01f);
        
        rb2D.velocity = movementDog;
        
        float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(currentTarget.position.x > transform.position.x)
        {
            enemyGFX.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (currentTarget.position.x < transform.position.x)
        {
            enemyGFX.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    public void OnStatesEnter()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                animDog.SetBool("IsSleeping", true);
                break;
            case DogStates.GARDE:
                Physics2D.IgnoreLayerCollision(9, 10, true);
                animDog.SetBool("IsGarde", true);
                break;
            case DogStates.RUNTOTARGET:
                Physics2D.IgnoreLayerCollision(9, 10, false);
                currentTarget = positionPlayer;
                animDog.SetBool("IsRunning", true);
                speedEnemy = moveSpeedRTT;
                break;
            case DogStates.WALKTOGARDE:
                currentTarget = positionGarde;
                speedEnemy = moveSpeedWTG;
                animDog.SetBool("IsWalk", true);
                break;
        }
    }
    public void OnStatesUpdate()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                if (dogHasAttacked)
                {
                    chronoToAttack += Time.deltaTime;
                }
                if (chronoToAttack > timerToAttack)
                {
                    dogHasAttacked = false;
                    chronoToAttack = 0; 
                }

                if (isEnterDogDetection && mB.isBAL == true)
                {
                    TransitionToStates(DogStates.GARDE);
                }
                if (isStayDogDetection && mB.isBAL == true)
                {
                    TransitionToStates(DogStates.GARDE);
                }
                if (isStayDogDetection && !dogHasAttacked || isEnterDogDetection && !dogHasAttacked)
                {
                    TransitionToStates(DogStates.GARDE);
                }
                break;
            case DogStates.GARDE:
                if (positionPlayer.localPosition.x > transform.position.x)
                {
                    enemyGFX.localEulerAngles = new Vector3(0, 180, 0);
                }
                else if (positionPlayer.localPosition.x < transform.position.x)
                {
                    enemyGFX.localEulerAngles = new Vector3(0, 0, 0);
                }
                if (isExitDogDetection)
                {
                    TransitionToStates(DogStates.SLEEPING);
                }
                if (isStayDogDetection && mB.isBAL == false)
                {
                    TransitionToStates(DogStates.RUNTOTARGET);
                }
                break;
            case DogStates.RUNTOTARGET:

                OnTargetPath();

                if (pM.isDogged)
                {
                    asDog.clip = acDogBark;
                    asDog.Play();
                    TransitionToStates(DogStates.WALKTOGARDE);
                    dogHasAttacked = true;
                }
                if (isLimit)
                {
                    TransitionToStates(DogStates.WALKTOGARDE);
                }
                if (isExitDogDetection)
                {
                    TransitionToStates(DogStates.WALKTOGARDE);
                }
                break;
            case DogStates.WALKTOGARDE:
                
                OnTargetPath();
                if (isEnterDogDetection)
                {
                    TransitionToStates(DogStates.RUNTOTARGET);
                }
                if (pTD.isOnPointDog)
                {
                    TransitionToStates(DogStates.SLEEPING);
                }
                break;
        }
    }
    public void OnStatesExit()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                animDog.SetBool("IsSleeping", false);
                isEnterDogDetection = false;
                break;
            case DogStates.GARDE:
                animDog.SetBool("IsGarde",false);
                isExitDogDetection = false;
                break;
            case DogStates.RUNTOTARGET:
                Physics2D.IgnoreLayerCollision(9, 10, true);
                pM.isDogged = false;
                isExitDogDetection = false;
                animDog.SetBool("IsRunning", false);
                break;
            case DogStates.WALKTOGARDE:
                isEnterDogDetection = false;
                animDog.SetBool("IsWalk", false);
                break;
        }
    }
    public void TransitionToStates(DogStates newStates)
    {
        OnStatesExit();
        currentDogStates = newStates;
        OnStatesEnter();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnterDogDetection = true;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStayDogDetection = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isExitDogDetection = true;
            isStayDogDetection = false;
        }
    }
}
