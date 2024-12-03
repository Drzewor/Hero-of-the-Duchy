using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IInteractable
{
    public void Interaction(GameObject player);
    public void HandleRaycast(GameObject player);
}
