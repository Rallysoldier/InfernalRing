using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    public PlayerGameObj playerObj1;
    public PlayerGameObj playerObj2;

    public GameController() {
        Instance = this;
    }

    void Awake()
    {
        GameObject char1Obj = GameObject.FindGameObjectWithTag("Char1");
        GameObject char2Obj = GameObject.FindGameObjectWithTag("Char2");

        //Instantiates player objects and their state machines, then gives an input handler from the player to their character (machine).
        //Both state machines set to XoninStateMachine for now, will be able to set the state machines to different characters later.
        playerObj1 = char1Obj.GetComponent<PlayerGameObj>();
        playerObj1.stateMachine = ScriptableObject.CreateInstance<XoninStateMachine>();
        playerObj1.inputHandler = new InputHandler();
        playerObj1.stateMachine.inputHandler = playerObj1.inputHandler;

        playerObj2 = char2Obj.GetComponent<PlayerGameObj>();
        playerObj2.stateMachine = ScriptableObject.CreateInstance<XoninStateMachine>();
        playerObj2.inputHandler = new InputHandler();
        playerObj2.stateMachine.inputHandler = playerObj2.inputHandler;

        playerObj1.stateMachine.anim = char1Obj.GetComponent<Animator>();
        playerObj1.stateMachine.body = char1Obj.GetComponent<Rigidbody2D>();

        playerObj2.stateMachine.anim = char2Obj.GetComponent<Animator>();
        playerObj2.stateMachine.body = char2Obj.GetComponent<Rigidbody2D>();

        playerObj1.stateMachine.enemy = playerObj2.stateMachine;
        playerObj2.stateMachine.enemy = playerObj1.stateMachine;

        //Inputs are predefined by us for now
        playerObj1.inputHandler.mapP1Inputs();
        playerObj2.inputHandler.mapP2Inputs();

        playerObj1.stateMachine.currentState.EnterState();
        playerObj2.stateMachine.currentState.EnterState();
    }

    void FixedUpdate()//Actual Game Controller loop. "Loop steps" in the design go here.
    {
        if (playerObj1.stateMachine != null && playerObj2.stateMachine != null) {
            playerObj1.stateMachine.UpdateState();
            playerObj2.stateMachine.UpdateState();
        }
    }

    void Update() {//Process input, send to input handlers.
        playerObj1.inputHandler.releasedKeys.Clear();
        playerObj2.inputHandler.releasedKeys.Clear();
        playerObj1.inputHandler.heldKeys.Clear();
        playerObj2.inputHandler.heldKeys.Clear();
        foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(kc)) {
                playerObj1.inputHandler.receiveInput(kc);
                playerObj2.inputHandler.receiveInput(kc);
            }
            if (Input.GetKeyUp(kc)) {
                playerObj1.inputHandler.addReleasedKey(kc);
                playerObj2.inputHandler.addReleasedKey(kc);
            }
            if (Input.GetKey(kc)) {
                playerObj1.inputHandler.addHeldKey(kc);
                playerObj2.inputHandler.addHeldKey(kc);
            }
        }
    }
}