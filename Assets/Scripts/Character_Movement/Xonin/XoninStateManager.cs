using System;
using UnityEngine;

public class XoninStateManager : MonoBehavior
{
    public XoninBaseState currentState;
    public XoninRunningState RunningState = new XoninRunningState();
    public XoninInAirState InAirState = new XoninInAirState();
    public XoninAttackingState AttackingState = new XoninAttackingState();

    void Start()
    {
        currentState = RunningState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

    }

    public void SwitchState(XoninBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}