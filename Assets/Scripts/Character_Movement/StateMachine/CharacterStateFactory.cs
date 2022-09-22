using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    //state variables
    CharacterBaseState currentState;
    CharacterStateFactory states;

    //getters and setters
    public CharacterBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterStateFactory State { get { return states; } set { states = value; } }

    void Awake()
    {
        //setup state
        states = new CharacterStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();

    }

    public void SwitchState(CharacterBaseState state)
    {

    }
}