using UnityEngine;

public interface IInteractable
{
    public bool CanInteract();
    public void Interact();
    public Transform GetTransform();
}

public interface IInteractableEnemy
{
    public bool CanInteract();
    public void HurtEnemy(float damage, float penetration, float armorIgnore, out float finalDamage);
    public void StunEnemy(float seconds);
}
