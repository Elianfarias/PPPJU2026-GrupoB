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

            int slotIndex = PlayerContext.LeftAttackPressed ? 0 : 1;
            if (!PlayerContext.OrbInventory.TryConsumeSlot(slotIndex, out var orb)) return;
            if (!PlayerContext.Data.SpellBook.TryGetSpell(orb.Element, out var spell)) return;

            nextTimeCast = Time.time + PlayerContext.Data.CdAttack;
            ExecuteSpell(spell);
        }

        private void ExecuteSpell(SpellSettingsSO spell)
        {
            var instance = spell.GetPool().GetSpell();
            instance.Execute(PlayerContext.FirePoint.position, PlayerContext.FirePoint.forward, spell);
        }
    }
}