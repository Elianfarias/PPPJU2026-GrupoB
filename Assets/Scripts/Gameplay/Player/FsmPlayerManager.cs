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
            OrbInventory = GetComponent<OrbInventory>(),
            AttackPressed = false,
            DodgePressed = false,
            JumpPressed = false,
            SprintPressed = false
        };

        states.Add(new StateIdle());
        states.Add(new StateWalking());
        states.Add(new StateRunning());
        states.Add(new StateAttack());
        states.Add(new StateJump());
        states.Add(new StateDodge());

        foreach (var state in states)
            state.Initialize(this, ctx);

        currentState = FindState(StateType.Idle);
        currentState.OnEnter();
    }
    private void Update() { currentState.OnUpdate(); }
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
    private void OnSprint(InputValue value)
    {
        ctx.SprintPressed = value.isPressed;
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

    public void OnDodgeFinished()
    {
        if (currentState.StateType != StateType.Dodge) return;

        var next = ctx.MoveInput != Vector2.zero
            ? (ctx.SprintPressed ? StateType.Running : StateType.Walking)
            : StateType.Idle;

        SwitchState(next);
    }
}