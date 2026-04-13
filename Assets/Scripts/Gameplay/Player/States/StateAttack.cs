using Assets.Scripts.Gameplay.Systems;
using UnityEngine;

public class StateAttack : StateBase
{
    private float nextTimeShoot;

    public StateAttack()
    {
        StateType = StateType.Attack;
    }

    public override void OnEnter() => base.OnEnter();

    public override void OnUpdate()
    {
        if (nextTimeShoot < Time.time)
        {
            nextTimeShoot = Time.time + PlayerContext.Data.CdAttack;
            TryCast();
        }
        
        PlayerContext.AttackPressed = false;
        Manager.SwitchState(PlayerContext.MoveInput != Vector2.zero ? StateType.Running : StateType.Idle);
    }

    public override void OnFixedUpdate() {
        Manager.ApplyRotation();
    }
    public override void OnExit() { }
    public override void OnAnimatorIK(int layerIndex) { }

    private void TryCast()
    {
        if (!PlayerContext.OrbInventory.TryConsumeOrbs(out OrbSettingsSO first, out OrbSettingsSO second))
            return;

        if (!PlayerContext.Data.SpellBook.TryGetSpell(first.Element, second.Element, out var spell))
            return;

        ExecuteSpell(spell);
    }

    private void ExecuteSpell(SpellSettingsSO spell)
    {
        var instance = spell.GetPool().GetSpell();
        instance.Execute(PlayerContext.FirePoint.position, PlayerContext.FirePoint.forward, spell.Damage);
    }
}