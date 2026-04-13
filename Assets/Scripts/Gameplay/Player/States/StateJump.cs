using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody))]
public class StateJump : StateBase
{
    public override void Initialize(FsmPlayerManager fsmManager, PlayerContext playerContext)
    {
        base.Initialize(fsmManager, playerContext);
        StateType = StateType.Jump;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Cursor.lockState = CursorLockMode.Locked;
        ApplyJumpImpulse();
    }

    public override void OnUpdate()
    {
        if (!IsGrounded()) return;

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
        const float aerialMultiplier = 0.7f;

        Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);
        Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection);
        PlayerContext.Rb.AddForce(aerialMultiplier * PlayerContext.Data.Force * worldDirection);

        ClampHorizontalVelocity();
    }

    private void ClampHorizontalVelocity()
    {
        Vector3 horizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
        horizontal = Vector3.ClampMagnitude(horizontal, PlayerContext.Data.MaxHorizontalSpeed * 0.7f);
        PlayerContext.Rb.linearVelocity = new Vector3(horizontal.x, PlayerContext.Rb.linearVelocity.y, horizontal.z);
    }

    private bool IsGrounded()
    {
        var origin = PlayerContext.CapsuleCollider.bounds.center;
        float rayLength = PlayerContext.CapsuleCollider.bounds.extents.y + 0.15f;
        return Physics.Raycast(origin, Vector3.down, rayLength, PlayerContext.Data.LayerCollision);
    }
}