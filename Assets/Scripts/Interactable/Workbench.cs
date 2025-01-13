using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.NPC;
using RPG.StateMachine.Player;
using TMPro;
using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    [SerializeField] Transform snapingPosition;
    [SerializeField] private string raycastText;

    public void Interaction(GameObject interactor)
    {
        if(interactor.GetComponent<NPCStateMachine>() != null) return;

        PlayerStateMachine stateMachine = interactor.GetComponent<PlayerStateMachine>();
        stateMachine.SwitchState(new PlayerCraftingState(stateMachine, snapingPosition));
    }
    public void HandleRaycast(GameObject player)
    {
        TMP_Text interactionText = player.GetComponent<PlayerStateMachine>().InteractionText;
        interactionText.enabled = true;
        if(string.IsNullOrEmpty(raycastText))
        {
            interactionText.text = $"Press F to craft in {name}";
            return;
        }
        else
        {
            interactionText.text = $"{raycastText}";
        }
    }
}
