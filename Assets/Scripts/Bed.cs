using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    private DataPersistanceManager persistanceManager;

    void Start() { persistanceManager = FindAnyObjectByType<DataPersistanceManager>(); }

    public bool CanInteract() { return true; }

    public void Interact() { persistanceManager.SaveGame(); }
}
