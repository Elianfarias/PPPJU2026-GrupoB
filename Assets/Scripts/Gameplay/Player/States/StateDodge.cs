using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StateDodge : StateBase
{
    private float elapsedTime;

    public override void Initialize(FsmPlayerManager fsmManager, PlayerContext playerContext)
    {
        base.Initialize(fsmManager, playerContext);
        StateType = StateType.Dodge;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Cursor.lockState = CursorLockMode.Locked;
        elapsedTime = 0f;

        ApplyDodgeImpulse();
        PlayerContext.HealthSystem.SetInvulnerable(true);
    }

    public override void OnUpdate()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < PlayerContext.Data.DodgeDuration) return;

        PlayerContext.HealthSystem.SetInvulnerable(false);
        Manager.SwitchState(PlayerContext.MoveInput != Vector2.zero ? StateType.Running : StateType.Idle);
    }

    public override void OnFixedUpdate()
    {
        Manager.ApplyRotation();
    }

    public override void OnExit() { 
        PlayerContext.HealthSystem.SetInvulnerable(false);
        PlayerContext.DodgePressed = false;
        PlayerContext.NextDodgeTime = Time.time + PlayerContext.Data.DodgeCooldown;
    }
    public override void OnAnimatorIK(int layerIndex) { }

    private void ApplyDodgeImpulse()
    {
        Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);

        if (localDirection == Vector3.zero)
            localDirection = Vector3.back;

        Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection.normalized);

        PlayerContext.Rb.linearVelocity = new Vector3(0f, PlayerContext.Rb.linearVelocity.y, 0f);
        PlayerContext.Rb.AddForce(worldDirection * PlayerContext.Data.DodgeForce, ForceMode.Impulse);
    }
}