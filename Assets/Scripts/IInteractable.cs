public interface IInteractable
{
    public bool CanInteract();
    public void Interact();
}

public interface IInteractableEnemy
{
    public bool CanInteract();
    public void HurtEnemy(float damage);
    public void StunEnemy(float seconds);
}
