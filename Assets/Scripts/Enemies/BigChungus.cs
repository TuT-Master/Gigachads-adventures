using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigChungus : MeleeAgresive
{
    [Header("Jump Attack Settings")]
    [SerializeField] private float jumpAttackDamage;
    [SerializeField] private float jumpAttackRange_min;
    [SerializeField] private float jumpAttackRange_max;
    [SerializeField] private float jumpAttackAOERange;
    [SerializeField] private float jumpAttackCooldown;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpHeight;
    private bool canJumpAttackAgain = true;
    private bool isJumpAttacking = false;


    private void Update()
    {
        if(!IsStillAlive())
            return;
        PlayWalkAnimation();

        if (isJumpAttacking)
            return;

        if (CanJumpAttack())
            StartCoroutine(JumpRoutine());
        else if (CanAttack())
        {
            StopMovement();
            Attack();
        }
        else
            Move();
    }
    private void Move()
    {
        if (target != null)
        {
            ResumeMovement();
            agent.SetDestination(target.transform.position);
        }
        else
            StopMovement();
    }

    private bool CanJumpAttack()
    {
        if(target != null &&
           canJumpAttackAgain &&
           !isJumpAttacking &&
           Vector3.Distance(transform.position, target.transform.position) <= jumpAttackRange_max &&
           Vector3.Distance(transform.position, target.transform.position) >= jumpAttackRange_min)
        {
            return true;
        }
        return false;
    }
    private IEnumerator JumpAttackCooldown()
    {
        yield return new WaitForSeconds(jumpAttackCooldown);
        canJumpAttackAgain = true;
    }

    private IEnumerator JumpRoutine()
    {
        canJumpAttackAgain = false;
        isJumpAttacking = true;

        // Stop movement
        StopMovement();

        // Trigger jump animation
        animator.SetTrigger("Jump");

        // Wait for animation to end
        yield return new WaitForSeconds(1f);

        Vector3 start = transform.position;
        Vector3 end = target.transform.position;
        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;

            // Arc: Lerp position + sine curve for height
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

            transform.position = pos;
            yield return null;
        }

        // Snap to end position
        transform.position = end;

        // Wait for landing
        while (transform.position.y >= 0.5f)
        {
            yield return null;
        }

        // Trigger landing animation
        animator.SetTrigger("Land");

        // Wait a bit before resetting
        yield return new WaitForSeconds(0.2f);

        // Deal damage
        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= jumpAttackAOERange)
            target.GetComponent<PlayerStats>().DealDamage(jumpAttackDamage, 0, 0);

        isJumpAttacking = false;

        // Start jump attack cooldown
        StartCoroutine(JumpAttackCooldown());
    }
}
