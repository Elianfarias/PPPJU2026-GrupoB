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
    }

    public override void OnFixedUpdate() { }
    public override void OnExit() { }
    public override void OnAnimatorIK(int layerIndex) { }

    private void TryCast()
    {
        if (!PlayerContext.OrbInventory.TryConsumeOrbs(out var first, out var second))
            return;

        if (!PlayerContext.Data.SpellBook.TryGetSpell(first.Element, second.Element, out var spell))
            return;

        ExecuteSpell(spell);
    }

    private void ExecuteSpell(SpellSettingsSO spell)
    {
        if (spell.VFXPrefab != null)
            Object.Instantiate(spell.VFXPrefab, PlayerContext.FirePoint.position, PlayerContext.FirePoint.rotation);
    }
}