using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody))]
public class StateMove : StateBase
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

    public override void OnUpdate() { }
    public override void OnFixedUpdate()
    {
        ApplyMovement();
        Manager.ApplyRotation();
        ClampVelocity();
    }
    public override void OnExit() { }
    public override void OnAnimatorIK(int layerIndex) { }

    private void ApplyMovement()
    {
        Vector3 localDirection = new(PlayerContext.MoveInput.x, PlayerContext.VerticalInput, PlayerContext.MoveInput.y);
        Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection);
        PlayerContext.Rb.AddForce(worldDirection * PlayerContext.Data.Force);
        PlayerContext.Rb.AddForce(PlayerContext.Data.VerticalForce * PlayerContext.VerticalInput * Vector3.up);
    }
    private void ClampVelocity()
    {
        Vector3 horizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
        float vertical = PlayerContext.Rb.linearVelocity.y;

        horizontal = Vector3.ClampMagnitude(horizontal, PlayerContext.Data.MaxHorizontalSpeed);
        vertical = Mathf.Clamp(vertical, -PlayerContext.Data.MaxVerticalSpeed, PlayerContext.Data.MaxVerticalSpeed);

        PlayerContext.Rb.linearVelocity = new Vector3(horizontal.x, vertical, horizontal.z);
    }
}