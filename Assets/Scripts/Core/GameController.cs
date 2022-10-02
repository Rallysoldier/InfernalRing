using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    public CharGameObj charGameObj1;
    public CharGameObj charGameObj2;

    public GameController() {
        Instance = this;
    }

    void Awake()
    {
        GameObject char1Obj = GameObject.FindGameObjectWithTag("Char1");
        GameObject char2Obj = GameObject.FindGameObjectWithTag("Char2");

        charGameObj1 = char1Obj.GetComponent<CharGameObj>();
        charGameObj1.stateMachine = ScriptableObject.CreateInstance<XoninStateMachine>();
        charGameObj2 = char2Obj.GetComponent<CharGameObj>();
        charGameObj2.stateMachine = ScriptableObject.CreateInstance<XoninStateMachine>();

        charGameObj1.stateMachine.anim = char1Obj.GetComponent<Animator>();
        charGameObj1.stateMachine.body = char1Obj.GetComponent<Rigidbody2D>();

        charGameObj2.stateMachine.anim = char2Obj.GetComponent<Animator>();
        charGameObj2.stateMachine.body = char2Obj.GetComponent<Rigidbody2D>();

        charGameObj1.stateMachine.facing = 1;
        charGameObj2.stateMachine.facing = -1;

        charGameObj1.stateMachine.inputHandler.mapP1Inputs();
        charGameObj2.stateMachine.inputHandler.mapP2Inputs();

        charGameObj1.stateMachine.currentState.EnterState();
        charGameObj2.stateMachine.currentState.EnterState();
    }

    void Update()
    {
        if (charGameObj1.stateMachine != null && charGameObj2.stateMachine != null) {
            charGameObj1.stateMachine.UpdateState();
            charGameObj2.stateMachine.UpdateState();
        }
    }

    public CharacterStateMachine setEnemyStateMachine(CharacterStateMachine sm) {
        if (sm == charGameObj1.stateMachine) {
            return charGameObj2.stateMachine;
        }
        return charGameObj1.stateMachine;
    }
}