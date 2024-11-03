using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    public void Iteraction(GameObject player)
    {
        PlayerStateMachine stateMachine = player.GetComponent<PlayerStateMachine>();
        stateMachine.SwitchState(new PlayerCraftingState(stateMachine));
    }
}
