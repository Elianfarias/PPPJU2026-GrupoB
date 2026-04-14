using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody))]
public class StateJump : StateBase
{
    private const float AerialMultiplier = 0.7f;
    public override void Initialize(FsmPlayerManager fsmManager, PlayerContext playerContext)
    {
        base.Initialize(fsmManager, playerContext);
        StateType = StateType.Jump;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerContext.IsGrounded = false;
        Cursor.lockState = CursorLockMode.Locked;
        ApplyJumpImpulse();
    }

    public override void OnUpdate()
    {
        Manager.SwitchState(PlayerContext.MoveInput != Vector2.zero ? StateType.Running : StateType.Idle);
    }

    public override void OnFixedUpdate()
    {
        ApplyAerialMovement();
        Manager.ApplyRotation();
    }

    public override void OnExit() { PlayerContext.JumpPressed = false; }
    public override void OnAnimatorIK(int layerIndex) { }

    private void ApplyJumpImpulse()
    {
        var velocity = PlayerContext.Rb.linearVelocity;
        velocity.y = 0f;
        PlayerContext.Rb.linearVelocity = velocity;
        PlayerContext.Rb.AddForce(Vector3.up * PlayerContext.Data.JumpForce, ForceMode.Impulse);
    }

    private void ApplyAerialMovement()
    {
        Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);
        Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection.normalized);
        Vector3 targetVelocity = AerialMultiplier * PlayerContext.Data.MaxHorizontalSpeed * worldDirection;

        Vector3 currentHorizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
        Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetVelocity, PlayerContext.Data.Acceleration * AerialMultiplier * Time.fixedDeltaTime);

        PlayerContext.Rb.linearVelocity = new Vector3(newHorizontal.x, PlayerContext.Rb.linearVelocity.y, newHorizontal.z);
    }
}