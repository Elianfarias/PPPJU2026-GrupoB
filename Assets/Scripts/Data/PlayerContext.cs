using UnityEngine;

public class PlayerContext
{
    public Vector2 MoveInput;
    public Vector2 LookInput;
    public float VerticalInput;
    public bool AttackPressed;
    public FsmPlayerManager FsmPlayerManager;
    public Rigidbody Rb;
    public Animator Animator;
    public PlayerSettingsSO Data;
    public HealthSystem HealthSystem;
    public CapsuleCollider CapsuleCollider;
    public Transform FirePoint;
    public OrbInventory OrbInventory;
    public bool JumpPressed;
    public bool DodgePressed;
    public float NextDodgeTime;
    public bool IsGrounded;
}