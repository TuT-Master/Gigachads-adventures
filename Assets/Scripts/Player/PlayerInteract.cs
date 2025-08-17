using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject interactabilityIconPrefab;

    private Dictionary<IInteractable, GameObject> activeIcons = new();
    private IInteractable interactableInRange;

    [Header("Visualization Distances")]
    [SerializeField] private float maxVisualizationRange = 3f;
    [SerializeField] private float fullOpacityRange = 1.5f;

    private List<IInteractable> allInteractables = new();

    private void Update()
    {
        allInteractables = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToList();

        UpdateVisibleInteractables();
        HandleInteraction();
        CleanupNullIcons();
    }

    private void UpdateVisibleInteractables()
    {
        foreach (IInteractable interactable in allInteractables)
        {
            if (interactable == null) continue;

            float dist = Vector3.Distance(interactable.GetTransform().position, transform.position);

            if (dist <= maxVisualizationRange)
            {
                float alpha = Mathf.Clamp01(1 - ((dist - fullOpacityRange) / (maxVisualizationRange - fullOpacityRange)));
                UpdateIcon(interactable, alpha);
            }
            else if (activeIcons.ContainsKey(interactable))
                RemoveIcon(interactable);
        }
        foreach(InteractabilityIcon icon in FindObjectsOfType<InteractabilityIcon>())
        {
            GameObject interactableObj = icon.interactableObj;
            if(interactableObj == null) continue;
            if (!interactableObj.activeInHierarchy)
            {
                RemoveIcon(interactableObj.GetComponent<IInteractable>());
                Destroy(icon.gameObject);
            }
        }
    }

    private void UpdateIcon(IInteractable interactable, float alpha)
    {
        if (!activeIcons.ContainsKey(interactable))
            activeIcons[interactable] = CreateIcon(interactable);

        var icon = activeIcons[interactable];
        var sr = icon.GetComponentInChildren<SpriteRenderer>();
        var text = icon.GetComponentInChildren<TextMesh>();

        Color c = new(1, 1, 1, alpha);
        sr.color = c;
        text.color = c;

        if (interactableInRange == interactable)
            text.text = GetInteractionText(interactable);
        else
            text.text = "";
    }

    private GameObject CreateIcon(IInteractable interactable)
    {
        var icon = Instantiate(interactabilityIconPrefab, interactable.GetTransform().position, Quaternion.identity);
        icon.GetComponent<InteractabilityIcon>().interactableObj = interactable.GetTransform().gameObject;
        return icon;
    }

    private string GetInteractionText(IInteractable interactable)
    {
        if (interactable.GetTransform().TryGetComponent(out ItemOnDaFloor item))
            return $"Press F to pick up {item.item.itemName}";

        if (interactable.GetTransform().TryGetComponent(out Door door))
        {
            return door.sceneName.ToLower() switch
            {
                "dungeon" => "Press F to enter dungeon",
                "shop" => "Press F to enter shop",
                "home" => "Press F to go home",
                _ when door.leadToRoom != null => "Press F to enter another room",
                _ => "Press F to interact"
            };
        }

        if (interactable.GetTransform().TryGetComponent(out OtherInventory otherInv))
            return $"Press F to open {otherInv.otherInventoryName}";

        if (interactable.GetTransform().TryGetComponent(out Bed _))
            return "Press F to save game";

        return "Press F to interact";
    }

    private void HandleInteraction()
    {
        if (Input.GetButtonDown("Interact") && interactableInRange != null)
        {
            if (interactableInRange.CanInteract())
            {
                interactableInRange.Interact();
                RemoveIcon(interactableInRange);
            }
            else
                RemoveInteractable(interactableInRange);
        }
    }

    private void RemoveIcon(IInteractable interactable)
    {
        if (activeIcons.TryGetValue(interactable, out var icon))
        {
            Destroy(icon);
            activeIcons.Remove(interactable);
        }
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        if (interactableInRange == interactable)
            interactableInRange = null;

        RemoveIcon(interactable);
    }

    private void CleanupNullIcons()
    {
        foreach (var key in activeIcons.Keys.Where(k => k == null).ToList())
            RemoveIcon(key);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<IInteractable>() is { } interactable && interactable.CanInteract())
            interactableInRange = interactable;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<IInteractable>() is { } interactable)
            RemoveInteractable(interactable);
    }
}
