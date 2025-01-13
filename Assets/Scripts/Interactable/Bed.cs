using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine;
using RPG.StateMachine.NPC;
using RPG.StateMachine.Player;
using TMPro;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    private bool isOccupied = false;
    [SerializeField] private Transform snapingPosition;
    [SerializeField] private string raycastText;
    

    public void SetisOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }
    
    public void Interaction(GameObject interactor)
    {
        if(isOccupied) return;

        if(interactor.TryGetComponent(out PlayerStateMachine player))
        {
            player.SwitchState(new PlayerSleepingState(player,snapingPosition,this));
            isOccupied = true;
            return;
        }
        else if(interactor.TryGetComponent(out NPCStateMachine npc))
        {
            npc.SwitchState(new NPCSleepingState(npc,snapingPosition,this));
            isOccupied = true;
            return;
        }
    }

    public void HandleRaycast(GameObject player)
    {
        TMP_Text interactionText = player.GetComponent<PlayerStateMachine>().InteractionText;
        interactionText.enabled = true;
        if(string.IsNullOrEmpty(raycastText))
        {
            interactionText.text = $"Press F to sleep";
            return;
        }
        else
        {
            interactionText.text = $"{raycastText}";
        }
    }
}
