using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        ReturningToStart
    }

    private NavMeshAgent navAgent;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private State state;

    public float targetRange = 30f;
    public float stopChaseDistance = 30f;
    public float attackRange = 10f;

    public bool SlowdownFromAttack { get; private set; }

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        state = State.Roaming;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        SlowdownFromAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Target>().isDead)
        {
            navAgent.SetDestination(transform.position);
            return;
        }

        if (SlowdownFromAttack)
        {
            navAgent.SetDestination(transform.position);
            return;
        }

        if (state == State.Roaming)
        {
            if (!navAgent.SetDestination(roamPosition))
            {
                Debug.LogError("Incorrect path");
                navAgent.SetDestination(startingPosition);
            }

            GetComponent<Animator>().SetBool("Walk Forward", true);

            float reachedPositionDistance = 1f;
            if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
            {
                //reached position
                roamPosition = GetRoamingPosition();
            }

            FindTarget();
        }
        else if(state == State.ChaseTarget)
        {
            navAgent.SetDestination(Player.Instance.GetPosition());

            
            if(Vector3.Distance(transform.position, Player.Instance.GetPosition()) < attackRange)
            {
                //attack player
                //play anim, add event to anim event that calls deal damage method
                GetComponent<Animator>().Play("Stab Attack");
            }

            if(Vector3.Distance(transform.position, Player.Instance.GetPosition()) > stopChaseDistance)
            {
                //stop chasing player
                state = State.ReturningToStart;
            }
        }
        else if(state == State.ReturningToStart)
        {
            navAgent.SetDestination(startingPosition); 
            float reachedPositionDistance = 10f;
            if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance)
            {
                //reached start
                state = State.Roaming;
            }
        }
    }

    public void SetAttackSlowdown(int toggle)
    {
        SlowdownFromAttack = System.Convert.ToBoolean(toggle);
    }

    public void DealDamage()
    {
        Player.Instance.TakeDamage(5);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + GetRandomDirection() * Random.Range(2f, 20f);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void FindTarget()
    {
        
        if(Vector3.Distance(transform.position, Player.Instance.GetPosition()) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }
}
