using Assets.Scripts.Gameplay.Systems;
using UnityEngine;

public class StateAttack : StateBase
{
    private float nextTimeShoot;
    private float nextTimeThrowRope;

    public StateAttack()
    {
        StateType = StateType.Attack;
    }

    public override void OnEnter() => base.OnEnter();

    public override void OnUpdate()
    {
        if (PlayerContext.AttackPressed && nextTimeShoot < Time.time)
        {
            nextTimeShoot = Time.time + PlayerContext.Data.CdAttack;
            Shoot();
        }
    }

    public override void OnFixedUpdate() { }
    public override void OnExit() { }
    public override void OnAnimatorIK(int layerIndex) { }

    private void Shoot()
    {
        IPoolable poolable = PlayerBulletPool.Instance.Get();
        MonoBehaviour mb = poolable as MonoBehaviour;
        mb.transform.SetPositionAndRotation(PlayerContext.FirePoint.position, Manager.transform.rotation);
        mb.GetComponent<ProjectileController>().Launch(PlayerContext.FirePoint.forward, PlayerContext.Data.Damage);
    }
}