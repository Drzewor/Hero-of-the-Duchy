using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerItemStashState : PlayerBaseState
{
    private Transform itemsParent;
    private ItemContainer container;
    public PlayerItemStashState(PlayerStateMachine stateMachine, Transform itemsParent, ItemContainer container) : base(stateMachine)
    { 
        this.itemsParent = itemsParent;
        this.container = container;
    }

     public override void Enter()
    {
        ShowMouseCursor();
        itemsParent.gameObject.SetActive(true);
        stateMachine.InventoryManager.inventory.gameObject.SetActive(true);
        stateMachine.InventoryManager.OpenItemContainer(container);

        stateMachine.InputReader.PressESCEvent += OnExit;
        stateMachine.InputReader.InteractEvent += OnExit;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        HideMouseCursor();
        itemsParent.gameObject.SetActive(false);
        stateMachine.InventoryManager.inventory.gameObject.SetActive(false);
        stateMachine.InventoryManager.CloseItemContainer(container);

        stateMachine.InputReader.PressESCEvent -= OnExit;
        stateMachine.InputReader.InteractEvent -= OnExit;
    }

    private void OnExit()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
