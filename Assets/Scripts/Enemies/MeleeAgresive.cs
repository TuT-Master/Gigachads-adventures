using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAgresive : Enemy
{
    private void Start()
    {
        SetStats();
        target = FindAnyObjectByType<PlayerStats>().gameObject;
    }
    private void Update()
    {
        CheckHealth();
        PlayWalkAnimation();

        if (CanAttack())
            Attack();
        else
            Move();
    }

    private void Move()
    {
        if(target != null)
        {
            ResumeMovement();
            agent.SetDestination(target.transform.position);
        }
        else
            StopMovement();
    }
}
