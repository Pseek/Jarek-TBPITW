using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class Dog : MonoBehaviour
{
    public MailBox mB;
    public Transform currentTarget;
    public Transform positionPlayer;
    public Transform positionGarde;
    public Animator animDog;
    public PositionTriggerDog pTD;

    public bool isEnterDogDetection = false;
    public bool isExitDogDetection = false;
    public bool isStayDogDetection = false;

    public float speedEnemy = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

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
        InvokeRepeating("UpdatePath", 0f, 0.5f);
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
  
    void FixedUpdate()
    {
        OnStatesUpdate();
    }
    public void OnGardePath()
    {

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

        if (direction.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (direction.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public void OnStatesEnter()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                animDog.SetBool("IsGarde", false);
                break;
            case DogStates.GARDE:
                speedEnemy = 0f;
                animDog.SetBool("IsGarde", true);
                break;
            case DogStates.RUNTOTARGET:
                animDog.SetBool("IsRunning", true);
                speedEnemy = 300f;
                break;
            case DogStates.WALKTOGARDE:
                speedEnemy = 200f;
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
                break;
            case DogStates.GARDE:
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
                currentTarget = positionPlayer;
                if (isExitDogDetection)
                {
                    TransitionToStates(DogStates.WALKTOGARDE);
                }
                
                break;
            case DogStates.WALKTOGARDE:
                OnTargetPath();
                currentTarget = positionGarde;
                if (isEnterDogDetection)
                {
                    TransitionToStates(DogStates.RUNTOTARGET);
                }
                if (pTD.isOnPointDog)
                {
                    TransitionToStates(DogStates.GARDE);
                }
                    break;
        }
    }
    public void OnStatesExit()
    {
        switch (currentDogStates)
        {
            case DogStates.SLEEPING:
                isEnterDogDetection = false; 
                break;
            case DogStates.GARDE:
                animDog.SetBool("IsGarde",false);
                isExitDogDetection = false;
                break;
            case DogStates.RUNTOTARGET:
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
            animDog.SetBool("IsPlayerHere", true);
            isEnterDogDetection = true;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStayDogDetection = true;
        }
        else
        {
            isStayDogDetection = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animDog.SetBool("IsPlayerHere", false);
            isExitDogDetection = true;
        }
    }
}
