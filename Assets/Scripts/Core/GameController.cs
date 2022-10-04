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
        playerObj1.stateMachine.inputHandler = playerObj1.inputHandler;

        playerObj2 = char2Obj.GetComponent<PlayerGameObj>();
        playerObj2.stateMachine = ScriptableObject.CreateInstance<XoninStateMachine>();
        playerObj2.stateMachine.inputHandler = playerObj2.inputHandler;

        playerObj1.stateMachine.anim = char1Obj.GetComponent<Animator>();
        playerObj1.stateMachine.body = char1Obj.GetComponent<Rigidbody2D>();
        playerObj1.stateMachine.spriteRenderer = char1Obj.GetComponent<SpriteRenderer>();

        playerObj2.stateMachine.anim = char2Obj.GetComponent<Animator>();
        playerObj2.stateMachine.body = char2Obj.GetComponent<Rigidbody2D>();
        playerObj2.stateMachine.spriteRenderer = char2Obj.GetComponent<SpriteRenderer>();

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

    void Update() {
        Camera mainCamera = Camera.main;
        float avgX = (playerObj1.stateMachine.body.position.x + playerObj2.stateMachine.body.position.x)/2;
        float avgY = (playerObj1.stateMachine.body.position.y + playerObj2.stateMachine.body.position.y)/2;
        Vector3 playerPosition = new Vector3(avgX,avgY + 2,-10);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,playerPosition,15.0f * Time.deltaTime);
    }
}