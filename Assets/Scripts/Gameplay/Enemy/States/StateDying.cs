namespace Assets.Scripts.Gameplay.Enemy.States
{
    public class StateDying : StateBase
    {
        public override void Initialize(EnemyContext context)
        {
            base.Initialize(context);
            StateType = EnemyStateType.Dying;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            context.CapsuleCollider.enabled = false;
            context.Agent.isStopped = true;
        }

        public override void OnUpdate() { }

        public override void OnExit() { }
    }
}
