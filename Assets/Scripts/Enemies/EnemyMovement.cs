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
    [SerializeField]
    private float weight;


    private float defaultSpeed;

    private EnemyFight enemyFight;


    private void Start()
    {
        enemyFight = GetComponent<EnemyFight>();
        defaultSpeed = speed;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
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
        agent.destination = FindAnyObjectByType<PlayerMovement>().transform.position;
    }
    private void MeleeEvasive()
    {
        if(enemyFight.CanAttack())
            agent.destination = FindAnyObjectByType<PlayerMovement>().transform.position;
        else
        {

        }
    }
    private void MeleeWandering()
    {

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
