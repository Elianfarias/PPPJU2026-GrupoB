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
        Cursor.visible = false;
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
        ClampVelocity();
        ApplyVisualTilt();
    }

    public override void OnExit()
    {
    }

    public override void OnAnimatorIK(int layerIndex)
    {
    }

    private void ApplyMovement()
    {
        Vector3 localDirection = new(PlayerContext.MoveInput.x, PlayerContext.VerticalInput, PlayerContext.MoveInput.y);
        Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection);
        PlayerContext.Rb.AddForce(worldDirection * PlayerContext.Data.Force);
        PlayerContext.Rb.AddForce(PlayerContext.Data.VerticalForce * PlayerContext.VerticalInput * Vector3.up);
    }

    private void ApplyRotation()
    {
        float angle = PlayerContext.LookInput.x * PlayerContext.Data.RotationSpeedX * Time.fixedDeltaTime;
        PlayerContext.FsmPlayerManager.transform.Rotate(Vector3.up, angle, Space.World);
    }

    private void ClampVelocity()
    {
        Vector3 horizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
        float vertical = PlayerContext.Rb.linearVelocity.y;

        horizontal = Vector3.ClampMagnitude(horizontal, PlayerContext.Data.MaxHorizontalSpeed);
        vertical = Mathf.Clamp(vertical, -PlayerContext.Data.MaxVerticalSpeed, PlayerContext.Data.MaxVerticalSpeed);

        PlayerContext.Rb.linearVelocity = new Vector3(horizontal.x, vertical, horizontal.z);
    }

    private void ApplyVisualTilt()
    {
        float targetPitch = PlayerContext.MoveInput.y <= 0 ? (PlayerContext.MoveInput.y * PlayerContext.Data.TiltAngle * 0.1f) : (PlayerContext.MoveInput.y * PlayerContext.Data.TiltAngle);
        float targetRoll = -PlayerContext.MoveInput.x * PlayerContext.Data.TiltAngle;

        Quaternion playerRotation = Quaternion.Euler(targetPitch, 0f, targetRoll);

        PlayerContext.FirePoint.localRotation = Quaternion.Lerp(
            PlayerContext.FirePoint.localRotation,
            playerRotation,
            Time.fixedDeltaTime * PlayerContext.Data.TiltSpeed
            );
    }
}