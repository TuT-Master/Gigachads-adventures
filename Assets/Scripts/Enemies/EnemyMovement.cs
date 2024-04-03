using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float weight;


    private float defaultSpeed;


    private void Start()
    {
        defaultSpeed = speed;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = FindAnyObjectByType<PlayerMovement>().transform.position;
        agent.speed = speed;
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
