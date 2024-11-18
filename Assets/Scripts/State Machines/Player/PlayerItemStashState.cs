using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerItemStashState : PlayerBaseState
{
    private readonly int OpenItemStashAnimationtHash = Animator.StringToHash("OpenItemStash");
    private const float CrossFadeDuration = 0.5f;
    private Transform itemsParent;
    private ItemContainer container;
    public PlayerItemStashState(PlayerStateMachine stateMachine, Transform itemsParent, ItemContainer container) : base(stateMachine)
    { 
        this.itemsParent = itemsParent;
        this.container = container;
    }

     public override void Enter()
    {
        EnterUIMode();
        itemsParent.gameObject.SetActive(true);
        stateMachine.InventoryManager.inventory.gameObject.SetActive(true);
        stateMachine.InventoryManager.OpenItemContainer(container);
        stateMachine.Animator.CrossFadeInFixedTime(OpenItemStashAnimationtHash,CrossFadeDuration);

        stateMachine.InputReader.PressESCEvent += OnExit;
        stateMachine.InputReader.InteractEvent += OnExit;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        ExitUIMode();
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
