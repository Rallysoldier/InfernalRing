using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XoninStateMachine : MonoBehavior
{
    //state variables
    XoninBaseState currentState;
    XoninStateFactory states;

    //getters and setters
    public XoninBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public bool IsJumpPressed { get { return isJumpPressed; }}

    void Awake()
    {
        //setup state
        currentState = new XoninStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

    }

    public void SwitchState(XoninBaseState state)
    {
        ExitState();

        newState.EnterState();

        currentState = state;
        state.EnterState(this);
    }
}