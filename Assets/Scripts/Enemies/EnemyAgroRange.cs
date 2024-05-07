using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAgroRange : MonoBehaviour
{
    public bool playerInRange;

    private void OnTriggerEnter(Collider other) { playerInRange = true; }
    private void OnTriggerExit(Collider other) { playerInRange = false; }
}
