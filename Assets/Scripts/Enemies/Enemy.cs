using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractableEnemy
{
    public bool CanInteract()
    {
        return true;
    }

    public void HurtEnemy(float damage)
    {
        Debug.Log(damage);
    }

    public void StunEnemy(float seconds)
    {

    }
}
