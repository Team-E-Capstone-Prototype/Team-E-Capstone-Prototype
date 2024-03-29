using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowMonster : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent Monster;

    float StunTimer;
    float currTime;

    bool m_enableTimer;

    enum AIState
    {
        Idle,
        Listening,
        Following,
    }

    [SerializeField]
    public float MaxListenDist;

    GameObject playerObject;

    AIState currState;

    void Start()
    {
        EnterState(AIState.Listening);

        playerObject = GameObject.FindGameObjectWithTag("Player");

        StunTimer = 3.0f;

        m_enableTimer = false;
    }

    void Update()
    {
        if (currState == AIState.Listening)
        {
            ListenForPlayer();
        }

        if (currState == AIState.Following)
        {
            FollowPlayer();
        }

        if (currState == AIState.Idle)
        {
            Monster.velocity = Vector3.zero;
        }

        if (m_enableTimer == true)
        {
            currTime -= 1 * Time.deltaTime;

            if (currTime <= 0.0f)
            {
                EnterState(AIState.Listening);
                m_enableTimer = false;
            }
        }

        if (currState == AIState.Following)
        {
            playerObject.GetComponent<AudioSource>().volume = (Vector3.Distance(playerObject.transform.position, transform.position) / MaxListenDist);
            Debug.Log(Monster.velocity.magnitude);
            //if (Vector3.Distance(playerObject.transform.position, transform.position) <= Monster.stoppingDistance)
            if (Monster.velocity.magnitude == 0)
            {
                playerObject.GetComponent<AudioSource>().volume = 0;
            }
        }

        if (Vector3.Distance(playerObject.transform.position, transform.position) > MaxListenDist)
        {
            EnterState(AIState.Listening);
        }
    }

    void EnterState(AIState newState)
    {
        switch (newState)
        {
            case AIState.Idle:
                {
                    currState = AIState.Idle;
                }
                break;

            case AIState.Listening:
                {
                    currState = AIState.Listening;
                }
                break;

            case AIState.Following:
                {
                    currState = AIState.Following;
                }
                break;
        }
    }

    void ListenForPlayer()
    {
        //Debug.Log("Listening for Player");
        if (Vector3.Distance(playerObject.transform.position, transform.position) < MaxListenDist)
        {
            Debug.Log("Player is Near");
            EnterState(AIState.Following);
        }
    }

    void FollowPlayer()
    {
        Debug.Log("Following player");
        Monster.SetDestination(playerObject.transform.position);
    }

    public void Stunned()
    {
        Debug.Log("Stunned");
        EnterState(AIState.Idle);
        currTime = StunTimer;
        m_enableTimer = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MaxListenDist);
    }
}
