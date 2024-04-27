using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFight : MonoBehaviour
{

    public bool canAttackAgain;

    [SerializeField]
    private float damage;
    [SerializeField]
    private float penetration;
    [SerializeField]
    private float armorIgnore;



    private GameObject player;


    private void Start()
    {
        canAttackAgain = true;
    }

    private void Update()
    {
        if(CanAttack() && canAttackAgain)
            Attack();
    }

    public bool CanAttack()
    {
        bool canAttack = false;
        if (player != null)
            canAttack = true;

        return canAttack;
    }

    void Attack()
    {
        if (player == null)
            return;

        canAttackAgain = false;

        player.GetComponent<PlayerStats>().DealDamage(damage, penetration, armorIgnore);

        GetComponent<EnemyMovement>().StopMovement();

        StartCoroutine(AttackAgain());
    }

    IEnumerator AttackAgain()
    {
        yield return new WaitForSeconds(1f);

        GetComponent<EnemyMovement>().ResumeMovement();

        canAttackAgain = true;
    }


    private void OnTriggerEnter(Collider other) { player = other.transform.parent.gameObject; }
    private void OnTriggerExit(Collider other) { player = null; }
}