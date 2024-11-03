using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.NPC;
using UnityEngine;

public class NPCsuspiciousState : NPCBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float suspiciousTime = 0;
    public NPCsuspiciousState(NPCStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        suspiciousTime = Random.Range(stateMachine.MinSuspiciousTime, stateMachine.MaxSuspiciousTime);

        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        suspiciousTime -= deltaTime;
        if(stateMachine.NPCTargeter.target != null && !stateMachine.NPCTargeter.target.isDead)
        {
            stateMachine.SwitchState(new NPCChasingState(stateMachine));
            return;
        }
        if(suspiciousTime <= 0)
        {
            stateMachine.SwitchState(new NPCIdleState(stateMachine));
            return;
        }
        stateMachine.Animator.SetFloat(SpeedHash,0, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        
    }


}
