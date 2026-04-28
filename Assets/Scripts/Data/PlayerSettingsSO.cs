using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Player")]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField] private int damage = 10;
    [Header("Movement")]
    [SerializeField] private float maxHorizontalSpeed = 10f;
    [SerializeField] private float maxVerticalSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [Header("Dodge")]
    [SerializeField] private float dodgeForce = 8f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private float dodgeCooldown = 1.5f;
    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float raycastDistance = 1f;
    [Header("Rotation")]
    [SerializeField] private float rotationSpeedX = 10f;
    [SerializeField] private float rotationSpeedY = 2f;
    [SerializeField] private float maxPitchAngle = 60f;
    [SerializeField] private float tiltSpeed = 8f;
    [SerializeField] private float tiltAngle = 20f;
    [Header("Layer Collision")]
    [SerializeField] private LayerMask layerCollision;
    [SerializeField] private float multiplyDamageCollision = 2f;
    [Header("Attack")]
    [SerializeField] private SpellBookSO spellBook;
    [SerializeField] private float cdAttack = 2f;
    [Header("Audio")]
    public float volumeMusic;
    public float volumeSFX;
    public float volumeUI;

    public int Damage { get { return damage; } }
    public float RotationSpeedX { get { return rotationSpeedX; } }
    public float RotationSpeedY { get { return rotationSpeedY; } }
    public float MaxPitchAngle { get { return maxPitchAngle; } }
    public float MaxHorizontalSpeed { get { return maxHorizontalSpeed; } }
    public float MaxVerticalSpeed { get { return maxVerticalSpeed; } }
    public LayerMask LayerCollision { get { return layerCollision; } }
    public float TiltSpeed { get { return tiltSpeed; } }
    public float TiltAngle { get { return tiltAngle; } }
    public float CdAttack { get { return cdAttack; } }
    public float MultiplyDamageCollision { get { return multiplyDamageCollision; } }
    public SpellBookSO SpellBook { get  { return spellBook; } }
    public float JumpForce => jumpForce;
    public float DodgeForce => dodgeForce;
    public float DodgeDuration => dodgeDuration;
    public float DodgeCooldown => dodgeCooldown;
    public float Acceleration => acceleration;
    public float RaycastDistance => raycastDistance;

}
