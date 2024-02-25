using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float weight;

    public IEnumerator Stun(float seconds)
    {
        float defaultSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(seconds);
        speed = defaultSpeed;
        GetComponent<EnemyStats>().isStunned = false;
    }
}
