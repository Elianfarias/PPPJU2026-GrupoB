using MoreMountains.Feedbacks;
using UnityEngine;
using Assets.Scripts.Gameplay.Player;

public class PlayerContext
{
    public Vector2 MoveInput;
    public Vector2 LookInput;
    public float VerticalInput;
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
    public bool SprintPressed;
    public float NextDodgeTime;
    public StateType PreviousRootState;
    public MMF_Player DodgeFeedback;
    public MMF_Player JumpFeedback;
    public bool LeftAttackPressed;
    public bool RightAttackPressed;
}