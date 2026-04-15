using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody))]
public class StateRunning : StateBase
{
    public override void Initialize(FsmPlayerManager fsmManager, PlayerContext playerContext)
    {
        base.Initialize(fsmManager, playerContext);
        StateType = StateType.Running;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnUpdate() {
        if (PlayerContext.JumpPressed)
            Manager.SwitchState(StateType.Jump);
        if (PlayerContext.AttackPressed)
            Manager.SwitchState(StateType.Attack);
        if (PlayerContext.DodgePressed)
            Manager.SwitchState(StateType.Dodge);

        if (PlayerContext.MoveInput == Vector2.zero)
            Manager.SwitchState(StateType.Idle);
        else if (!PlayerContext.SprintPressed)
            Manager.SwitchState(StateType.Walking);
    }
    public override void OnFixedUpdate()
    {
        ApplyMovement();
        Manager.ApplyRotation();
    }
    public override void OnExit() { }
    public override void OnAnimatorIK(int layerIndex) { }

    private void ApplyMovement()
    {
        Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);
        Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection.normalized);
        Vector3 targetVelocity = worldDirection * PlayerContext.Data.MaxHorizontalSpeed;

        Vector3 currentHorizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
        Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetVelocity, PlayerContext.Data.Acceleration * Time.fixedDeltaTime);

        PlayerContext.Rb.linearVelocity = new Vector3(newHorizontal.x, PlayerContext.Rb.linearVelocity.y, newHorizontal.z);
    }
}