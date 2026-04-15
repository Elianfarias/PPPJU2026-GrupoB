using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody))]
public class StateIdle : StateBase
{
    public override void Initialize(FsmPlayerManager fsmManager, PlayerContext playerContext)
    {
        base.Initialize(fsmManager, playerContext);
        StateType = StateType.Idle;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnUpdate()
    {
        if (PlayerContext.JumpPressed)
            Manager.SwitchState(StateType.Jump);
        if (PlayerContext.AttackPressed)
            Manager.SwitchState(StateType.Attack);
        if (PlayerContext.DodgePressed)
            Manager.SwitchState(StateType.Dodge);

        if (PlayerContext.MoveInput != Vector2.zero)
        {
            if (PlayerContext.SprintPressed)
                Manager.SwitchState(StateType.Running);
            else
                Manager.SwitchState(StateType.Walking);
        }
    }

    public override void OnFixedUpdate()
    {
        Manager.ApplyRotation();
    }

    public override void OnExit()
    {
    }

    public override void OnAnimatorIK(int layerIndex)
    {
    }
}