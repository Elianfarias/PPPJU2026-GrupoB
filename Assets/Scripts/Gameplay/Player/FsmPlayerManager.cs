using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FsmPlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO data;
    [SerializeField] private Animator animator;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject cameraThirdPerson;
    [SerializeField] private GameObject cameraFirstPerson;

    private PlayerContext ctx;
    private readonly IList<StateBase> states = new List<StateBase>();
    private StateBase currentState;

    private void Awake()
    {
        ctx = new PlayerContext
        {
            Rb = GetComponent<Rigidbody>(),
            Animator = animator,
            Data = data,
            HealthSystem = healthSystem,
            CapsuleCollider = capsuleCollider,
            FirePoint = firePoint,
            FsmPlayerManager = this,
            IsGrounded = true,
            OrbInventory = GetComponent<OrbInventory>()
        };

        states.Add(new StateIdle());
        states.Add(new StateMove());
        states.Add(new StateAttack());
        states.Add(new StateJump());
        states.Add(new StateDodge());

        foreach (var state in states)
            state.Initialize(this, ctx);

        currentState = FindState(StateType.Idle);
        currentState.OnEnter();
    }
    private void Update()
    {
        currentState.OnUpdate();

        HandleTransitions();
    }
    private void FixedUpdate() { currentState.OnFixedUpdate(); }
    private void OnAnimatorIK(int layerIndex) { currentState.OnAnimatorIK(layerIndex); }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * data.RaycastDistance);
    }
    private void OnMove(InputValue value)
    {
        ctx.MoveInput = value.Get<Vector2>();
    }
    private void OnLook(InputValue value)
    {
        ctx.LookInput = value.Get<Vector2>();
    }
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
            ctx.AttackPressed = true;
    }
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
            ctx.JumpPressed = true;
    }
    private void OnDodge(InputValue value)
    {
        if (value.isPressed)
            ctx.DodgePressed = true;
    }

    private void HandleTransitions()
    {
        ctx.IsGrounded = IsGrounded();

        if (ctx.DodgePressed && ctx.NextDodgeTime <= Time.time)
        {
            SwitchState(StateType.Dodge);
            return;
        }
        if (ctx.JumpPressed && ctx.IsGrounded)
        {
            SwitchState(StateType.Jump);
            return;
        }
        if (ctx.AttackPressed)
        {
            SwitchState(StateType.Attack);
            return;
        }

        if (ctx.IsGrounded)
            SwitchState(ctx.MoveInput != Vector2.zero ? StateType.Running : StateType.Idle);
    }

    private void OnSwitchCamera(InputValue value)
    {
        if (!value.isPressed) return;
        bool isThird = cameraThirdPerson.activeSelf;
        cameraThirdPerson.SetActive(!isThird);
        cameraFirstPerson.SetActive(isThird);
    }

    public void SwitchState(StateType stateType)
    {
        StateBase next = FindState(stateType);
        if (next == null || next == currentState) return;
        currentState.OnExit();
        currentState = next;
        currentState.OnEnter();
    }

    public StateBase FindState(StateType stateType)
    {
        foreach (var state in states)
            if (state.StateType == stateType) return state;
        return null;
    }

    public Coroutine StartManagedCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    public void ApplyRotation()
    {
        float angle = ctx.LookInput.x * ctx.Data.RotationSpeedX * Time.fixedDeltaTime;
        ctx.FsmPlayerManager.transform.Rotate(Vector3.up, angle, Space.World);
    }

    private bool IsGrounded()
    {
        var origin = capsuleCollider.bounds.center;
        return Physics.Raycast(origin, Vector3.down, data.RaycastDistance, data.LayerCollision);
    }
}