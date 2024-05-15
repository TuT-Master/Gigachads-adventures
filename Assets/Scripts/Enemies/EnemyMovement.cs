using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum Attitude
    {
        MeleeAgressive,
        MeleeEvasive,
        MeleeWandering,
        MeleeStealth,
        RangedStatic,
        RangedWandering,
        Placeable,
    }
    public Attitude attitude;

    private NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField]
    private float speed;


    private float defaultSpeed;

    private EnemyFight enemyFight;
    private EnemyAgroRange agro;

    private bool getNewRandomPoint = true;



    private void Start()
    {
        enemyFight = GetComponent<EnemyFight>();
        agent = GetComponent<NavMeshAgent>();
        agro = GetComponentInChildren<EnemyAgroRange>();

        agent.speed = defaultSpeed = speed;
    }

    private void FixedUpdate()
    {

        switch(attitude)
        {
            case Attitude.MeleeAgressive:
                MeleeAggresive();
                break;
            case Attitude.MeleeEvasive:
                MeleeEvasive();
                break;
            case Attitude.MeleeWandering:
                MeleeWandering();
                break;
            case Attitude.MeleeStealth:
                MeleeStealth();
                break;
            case Attitude.RangedStatic:
                RangedStatic();
                break;
            case Attitude.RangedWandering:
                RangedWandering();
                break;
            case Attitude.Placeable:
                // Random se otáèí (rozhlíží)
                break;
            default:
                break;
        }
    }

    private void MeleeAggresive()
    {
        if (!enemyFight.CanAttack())
        {
            agent.isStopped = false;
            agent.destination = FindAnyObjectByType<PlayerMovement>().transform.position;
        }
        else
            agent.isStopped = true;
    }
    private void MeleeEvasive()
    {
        if(enemyFight.CanAttack())
        {
            agent.isStopped = true;
            agent.destination = FindAnyObjectByType<PlayerMovement>().transform.position;
        }
        else
        {
            agent.isStopped = false;
            // Evading
        }
    }
    private void MeleeWandering()
    {
        // Get random point
        if(getNewRandomPoint)
        {
            Debug.Log("Finding new point!");
            agent.destination = GetRandomPoint(transform.position);
            getNewRandomPoint = false;
            StartCoroutine(GetRandomPointDelay());
        }

        if (agro.playerInRange)
            attitude = Attitude.MeleeAgressive;
    }
    private void MeleeStealth()
    {

    }
    private void RangedStatic()
    {

    }
    private void RangedWandering()
    {

    }

    private IEnumerator GetRandomPointDelay()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        getNewRandomPoint = true;
    }

    Vector3 GetRandomPoint(Vector3 startPoint)
    {
        return new(startPoint.x + Random.Range(-10, 10), 0, startPoint.z + Random.Range(-10, 10));
    }

    public IEnumerator Stun(float seconds)
    {
        float defaultSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(seconds);
        speed = defaultSpeed;
        GetComponent<EnemyStats>().isStunned = false;
    }

    public void StopMovement()
    {
        speed = 0f;
    }

    public void ResumeMovement()
    {
        speed = defaultSpeed;
    }
}
