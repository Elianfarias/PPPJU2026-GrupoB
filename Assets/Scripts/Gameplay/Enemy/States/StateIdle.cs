namespace Assets.Scripts.Gameplay.Enemy.States
{
    public class StateIdle : StateBase
    {
        public override void Initialize(EnemyContext context)
        {
            base.Initialize(context);
            StateType = EnemyStateType.Idle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            context.Agent.isStopped = true;
        }

        public override void OnUpdate()
        {
            if (TryTransitionToDying()) return;

            if (context.Manager.IsBlind) return;

            if (IsPlayerInRange(context.Settings.PlayerDetectionRadius))
            {
                var chase = context.Manager.FindState(EnemyStateType.Chase);
                context.Manager.SwitchState(chase);
            }
        }

        public override void OnExit() { }
    }
}