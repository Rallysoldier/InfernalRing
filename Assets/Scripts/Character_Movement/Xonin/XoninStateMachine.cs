using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XoninStateMachine : MonoBehaviour
{
    //state variables
    XoninBaseState currentState;
    XoninStateFactory states;

    //getters and setters
    public XoninBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public XoninStateFactory State { get { return states; } set { states = value; } }

    void Awake()
    {
        //setup state
        states = new XoninStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();

    }

    public void SwitchState(XoninBaseState state)
    {
        
    }
}