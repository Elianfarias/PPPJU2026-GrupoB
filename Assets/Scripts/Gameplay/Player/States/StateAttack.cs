using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States
{
    public class StateAttack : StateBase
    {
        private float nextTimeCast;

        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Attack;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PlayerContext.JumpPressed = false;
            PlayerContext.AttackPressed = false;
            TryCast();
        }

        public override void OnUpdate()
        {
            PlayerContext.AttackPressed = false;
            Manager.ReturnFromAttack();
        }

        public override void OnFixedUpdate() { }

        public override void OnExit() { }

        public override void OnAnimatorIK(int layerIndex) { }

        private void TryCast()
        {
            if (nextTimeCast > Time.time) return;

            nextTimeCast = Time.time + PlayerContext.Data.CdAttack;

            if (!PlayerContext.OrbInventory.TryConsumeOrbs(out var first, out var second)) return;
            if (!PlayerContext.Data.SpellBook.TryGetSpell(first.Element, second.Element, out var spell)) return;

            ExecuteSpell(spell);
        }

        private void ExecuteSpell(SpellSettingsSO spell)
        {
            var instance = spell.GetPool().GetSpell();
            instance.Execute(PlayerContext.FirePoint.position, PlayerContext.FirePoint.forward, spell);
        }
    }
}