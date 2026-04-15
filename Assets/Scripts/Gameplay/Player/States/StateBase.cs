using UnityEngine;

public abstract class StateBase
{
    protected static readonly int StateHash = Animator.StringToHash("State");

    public StateType StateType;
    protected FsmPlayerManager Manager;
    protected PlayerContext PlayerContext;

    public virtual void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
    {
        Manager = manager;
        PlayerContext = playerContext;
    }

    public virtual void OnEnter()
    {
        PlayerContext.Animator.SetInteger(StateHash, (int)StateType);
    }

    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
    public abstract void OnAnimatorIK(int layerIndex);
}