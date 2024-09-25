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
    public LimitDog lD;

    public bool isEnterDogDetection = false;
    public bool isExitDogDetection = false;
    public bool isStayDogDetection = false;

    public float speedEnemy = 200f;
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
        Vector2 force = direction * speedEnemy * Time.deltaTime;

        rb2D.AddForce(force);

        float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb2D.velocity.x > 0f)
        {
            enemyGFX.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (rb2D.velocity.x < 0f)
        {
            enemyGFX.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    public void OnStatesEnter()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                animDog.SetBool("IsPlayerHere", false);
                animDog.SetBool("IsSleeping", true);
                break;
            case DogStates.GARDE:
                speedEnemy = 0f;
                animDog.SetBool("IsPlayerHere", true);
                animDog.SetBool("IsGarde", true);
                break;
            case DogStates.RUNTOTARGET:
                currentTarget = positionPlayer;
                animDog.SetBool("IsRunning", true);
                speedEnemy = 700f;
                break;
            case DogStates.WALKTOGARDE:
                Physics2D.IgnoreLayerCollision(9,10,true);
                currentTarget = positionGarde;
                speedEnemy = 600f;
                animDog.SetBool("IsWalk", true);
                break;
        }
    }
    public void OnStatesUpdate()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                if (isEnterDogDetection)
                {
                    TransitionToStates(DogStates.GARDE);
                }
                if (isStayDogDetection)
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
                }
                if (lD.isLimit)
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
                pM.isDogged = false;
                isExitDogDetection = false;
                animDog.SetBool("IsRunning", false);
                break;
            case DogStates.WALKTOGARDE:
                Physics2D.IgnoreLayerCollision(9, 10, false);
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
