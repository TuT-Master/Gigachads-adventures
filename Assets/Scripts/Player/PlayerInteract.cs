using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private GameObject interactabilityIcon_prefab;
    private Dictionary<IInteractable, GameObject> interactabilityIcons = new();

    private IInteractable interactableInRange;

    private readonly float interactablesVisualizationRange = 3f;
    private readonly float interactablesMaxVisualizationRange = 1.5f;

    private void Update()
    {
        // Show possible interactables near player
        foreach (IInteractable interactable in FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>())
        {
            float distance = Vector3.Distance(interactable.GetTransform().position, transform.position);
            if (distance <= interactablesVisualizationRange)
            {
                float f = 1 - ((distance - interactablesMaxVisualizationRange) / (interactablesVisualizationRange - interactablesMaxVisualizationRange));
                Color color = new(1, 1, 1, f);
                if (interactable.GetTransform().TryGetComponent(out ItemOnDaFloor itemOnDaFloor))
                {
                    if (!interactabilityIcons.ContainsKey(interactable))
                    {
                        Vector3 newIconPos = new(interactable.GetTransform().position.x, interactable.GetTransform().position.y, interactable.GetTransform().position.z);
                        GameObject newIcon = Instantiate(interactabilityIcon_prefab, newIconPos, Quaternion.identity);
                        newIcon.GetComponent<InteractabilityIcon>().interactableObj = interactable.GetTransform().gameObject;
                        interactabilityIcons.Add(interactable, newIcon);
                    }

                    if (interactableInRange == interactable)
                    {
                        interactabilityIcons[interactable].GetComponentInChildren<SpriteRenderer>().color = color;
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().color = color;
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to pick up " + itemOnDaFloor.item.itemName;
                    }
                    else
                    {
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "";
                        interactabilityIcons[interactable].GetComponentInChildren<SpriteRenderer>().color = new(1, 1, 1, 0);
                    }
                }
                else
                {
                    if (interactabilityIcons.ContainsKey(interactable))
                    {
                        interactabilityIcons[interactable].GetComponentInChildren<SpriteRenderer>().color = color;
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().color = color;
                    }
                    else
                    {
                        Vector3 newIconPos = new(interactable.GetTransform().position.x, interactable.GetTransform().position.y, interactable.GetTransform().position.z);
                        GameObject newIcon = Instantiate(interactabilityIcon_prefab, newIconPos, Quaternion.identity);
                        newIcon.GetComponent<InteractabilityIcon>().interactableObj = interactable.GetTransform().gameObject;
                        newIcon.GetComponentInChildren<SpriteRenderer>().color = color;
                        newIcon.GetComponentInChildren<TextMesh>().color = color;
                        interactabilityIcons.Add(interactable, newIcon);
                    }

                    if(interactableInRange == interactable)
                    {
                        if(interactable.GetTransform().TryGetComponent(out Door door) && door.sceneName.ToLower() == "dungeon")
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to enter dungeon";
                        else if (interactable.GetTransform().TryGetComponent(out door) && door.sceneName.ToLower() == "shop")
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to enter shop";
                        else if (interactable.GetTransform().TryGetComponent(out door) && door.sceneName.ToLower() == "home")
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to go home";
                        else if (interactable.GetTransform().TryGetComponent(out door) && door.leadToRoom != null)
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to enter another room";
                        else if (interactable.GetTransform().TryGetComponent(out OtherInventory otherInventory))
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to open " + otherInventory.otherInventoryName;
                        else if (interactable.GetTransform().TryGetComponent(out Bed bed))
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to save game";
                        else
                            interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to interact";
                    }
                    else
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "";
                }
            }
            else if(distance > interactablesVisualizationRange && interactabilityIcons.ContainsKey(interactable))
            {
                Destroy(interactabilityIcons[interactable]);
                interactabilityIcons.Remove(interactable);
            }
        }

        // Interaction
        if (Input.GetButtonDown("Interact") && interactableInRange != null)
        {
            try
            {
                if(interactableInRange.CanInteract())
                {
                    interactableInRange.Interact();
                    Destroy(interactabilityIcons[interactableInRange]);
                    interactabilityIcons.Remove(interactableInRange);
                }
                if (!interactableInRange.CanInteract())
                    RemoveInteractable(interactableInRange);
            }
            catch
            {
                interactableInRange = null;
            }
        }

        foreach (InteractabilityIcon icon in FindObjectsOfType<MonoBehaviour>().OfType<InteractabilityIcon>())
            if(icon.interactableObj == null)
                Destroy(icon.gameObject);
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        if (interactableInRange == interactable)
            interactableInRange = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable != null && interactable.CanInteract())
            interactableInRange = interactable;
    }
    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        RemoveInteractable(interactable);
    }
}
