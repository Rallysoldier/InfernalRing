using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using BlackGardenStudios.HitboxStudioPro;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    [SerializeField]
    public PlayerGameObj[] Characters;

    public List<PlayerGameObj> Players;
    public List<string> characterNames = new List<string>{"Xonin","Xonin"};
    public List<int> selectedPalettes = new List<int>{1,3};

    public int Global_Time = 0;
    //Determines which player paused the game during a character pause
    public int playerPaused = -1;
    public int pause = 0;

    Camera mainCamera = Camera.main;
    float cameraLerp = 15f;

    public GameController() {
        Instance = this;
    }

    void Awake()
    {
        for (int c = 0; c < characterNames.Count; c++) {
            for (int i = 0; i < Characters.Length; i++) {
                if (Characters[i].name == characterNames[c]) {
                    Players.Add(Instantiate(Characters[i], new Vector3(5 * c == 0 ? 1 : -1, 0, 0), Quaternion.identity));
                    goto end_of_loop;
                }
            }

            Players.Add(Instantiate(Characters[0], new Vector3(5 * c == 0 ? 1 : -1, 0, 0), Quaternion.identity));

            end_of_loop: {}
        }

        float startPosP1 = -5.0f;

        for (int i = 0; i < 2; i++)
        {
            var j = i;

            //Instantiates player objects and their state machines, then gives an input handler from the player to their character (machine).
            //Both state machines set to XoninStateMachine for now, will be able to set the state machines to different characters later.
            PlayerGameObj playerObj = Players[i];
            playerObj.gameObject.SetActive(true);

            playerObj.stateMachine = CreateStateMachine(characterNames[i]);
            playerObj.stateMachine.inputHandler = playerObj.inputHandler;

            playerObj.stateMachine.anim = playerObj.GetComponent<Animator>();
            playerObj.stateMachine.body = playerObj.GetComponent<Rigidbody2D>();
            playerObj.stateMachine.spriteRenderer = playerObj.GetComponent<SpriteRenderer>();

            playerObj.stateMachine.body.position = new Vector3(startPosP1 * i * -1,0,0);
            
            if (i%2 == 0) {
                playerObj.inputHandler.mapP1Inputs();
            } else {
                playerObj.inputHandler.mapP2Inputs();
            }

            if (i > 0) {
                Players[i-1].stateMachine.enemy = Players[i].stateMachine;
                Players[i].stateMachine.enemy = Players[i-1].stateMachine;
            }

            //Swap palettes if characters and selected palettes are the same
            if (i > 0 && selectedPalettes[i] == selectedPalettes[i-1] && Players[i].name == Players[i-1].name) {
                playerObj.SetPalette(playerObj.NextPaletteIndex(selectedPalettes[i]));
            } else {
                playerObj.SetPalette(selectedPalettes[i]);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            Players[i].stateMachine.currentState.EnterState();
        }
    }

    CharacterStateMachine CreateStateMachine(string characterName) {
        switch (characterName) {
            case "Xonin":
                Debug.Log("Creating state machine for " + characterName);
                return ScriptableObject.CreateInstance<XoninStateMachine>();
        }
        return ScriptableObject.CreateInstance<CharacterStateMachine>();
    }

    //Actual Game Controller loop. "Loop steps" in the design go here.
    int trueGameTicks;
    ContactSummary P1_Hits;
    ContactSummary P2_Hits;
    void FixedUpdate()
    {
        //True game ticks happen ten times as fast, every 0.00167s instead of 0.0167. This is for things that must be
        //calculated quickly like hitbox collisions.
        trueGameTicks++;
        if (trueGameTicks%10 != 0) {
            return;
        }

        Global_Time++;

        for (int i = 0; i < Players.Count; i++)
        {
            if (this.pause > 0) {
                if (i != this.playerPaused) {
                    if (Players[i].m_Animator != null) {
                        Players[i].m_Animator.speed = 0;
                    }
                    if (Players[i].m_RigidBody != null) {
                        Players[i].m_RigidBody.Sleep();
                    }
                }
            } else {
                this.playerPaused = -1;
                if (Players[i].m_Animator != null) {
                    Players[i].m_Animator.speed = 1;
                }
                if (Players[i].m_RigidBody != null) {
                    Players[i].m_RigidBody.WakeUp();
                }
            }

            if (Players[i].stateMachine != null && (i == this.playerPaused || this.pause == 0)) {
                Players[i].stateMachine.UpdateStates();
            }
        }

        if (this.pause > 0) {
            this.pause--;
        } else {
            P1_Hits = Players[1].stateMachine.contactSummary;
            P2_Hits = Players[0].stateMachine.contactSummary;
        }

        if (P1_Hits.NotEmpty() || P2_Hits.NotEmpty()) {
            int scoreP1 = 0;
            int maxPriorityP1 = 0;
            foreach (ContactData data in P1_Hits.bodyColData) {
                scoreP1 += (int)data.AttackPriority;
                if ((int)data.AttackPriority > maxPriorityP1) {
                    maxPriorityP1 = (int)data.AttackPriority;
                }
            }
            
            int scoreP2 = 0;
            int maxPriorityP2 = 0;
            foreach (ContactData data in P2_Hits.bodyColData) {
                scoreP2 += (int)data.AttackPriority;
                if ((int)data.AttackPriority > maxPriorityP2) {
                    maxPriorityP2 = (int)data.AttackPriority;
                }
            }

            int winningPriority = scoreP1 == scoreP2 ? scoreP1 : scoreP1 > scoreP2 ? maxPriorityP1 : maxPriorityP2;
            List<ContactData> winningHits = scoreP1 == scoreP2 ? null : scoreP1 > scoreP2 ? P1_Hits.bodyColData : P2_Hits.bodyColData;
            CharacterStateMachine characterHitting = scoreP1 == scoreP2 ? null : scoreP1 > scoreP2 ? Players[0].stateMachine : Players[1].stateMachine;
            CharacterStateMachine characterHurt = scoreP1 == scoreP2 ? null : scoreP1 < scoreP2 ? Players[0].stateMachine : Players[1].stateMachine;
            if (winningHits != null) {
                winningHits.RemoveAll(hit => (int) hit.AttackPriority < winningPriority);

                foreach (ContactData hit in winningHits) {
                    bool blocked = false;
                    bool enemyHoldingBack = characterHurt.inputHandler.held(characterHurt.inputHandler.BackInput(characterHurt));
                    bool enemyHoldingDown = characterHurt.inputHandler.held("D");
                    if (enemyHoldingBack && hit.GuardType != GuardType.UNBLOCKABLE) {//If holding back and attack isn't unblockable
                        if (hit.GuardType == GuardType.MID) {
                            blocked = true;
                            //go into stand or crouch block
                        } else if (enemyHoldingDown && hit.GuardType == GuardType.LOW) {
                            blocked = true;
                            //go into crouch block
                        } else if (!enemyHoldingDown) {
                            blocked = true;
                            //go into standing block
                        }
                    }

                    if (blocked) {
                        characterHurt.Block(hit);
                    } else {
                        characterHurt.Hit(hit);
                    }
                }
            } else {
                Pause(winningPriority*5);
            }
        }

        //Always clear hit lists at the end
        P1_Hits.Clear();
        P2_Hits.Clear();
    }

    void Update() {//Camera movement by linearly interpolating through points

        //TODO: Add more camera modes: CameraFocus.Player1, CameraFocus.Player2, CameraFocus.Both, CameraFocus.None
        float avgX = (Players[0].stateMachine.body.position.x + 
            Players.Count < 2 ? 0 : Players[1].stateMachine.body.position.x)/2;
        float avgY = (Players[0].stateMachine.body.position.y + 
            Players.Count < 2 ? 0 : Players[1].stateMachine.body.position.y)/2;
        if (avgX < -4.7f) {
            avgX = -4.7f;
        } else if (avgX > 4.7f) {
            avgX = 4.7f;
        }

        Vector3 cameraDestination = new Vector3(avgX, avgY + 2.5f, -10); //Replace 2.5f with the average of characters' heights
        LerpCamera(cameraDestination);
    }

    void LerpCamera(Vector3 cameraDestination) {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraDestination, cameraLerp * Time.deltaTime);
    }

    public void SetCameraPos(Vector2 pos) {
        Vector3 cameraDestination = new Vector3(pos.x, pos.y, -10);
        mainCamera.transform.position = cameraDestination;
    }

    public void SetCameraPos(float x, float y) {
        Vector3 cameraDestination = new Vector3(x, y,-10);
        mainCamera.transform.position = cameraDestination;
    }

    public void SetCameraLerp(float lerp) {
        cameraLerp = lerp;
    }
    public float GetCameraLerp() {
        return cameraLerp;
    }

    //Used for hitpause
    public void Pause(int time) {
        this.playerPaused = -1;
        this.pause = time;
    }

    //Pauses the game and disables physics for one character. Useful in super/dramatic pauses where only one character is affected.
    public void CharacterPause(CharacterStateMachine character, int time) {
        for (int i = 0; i < 2; i++)
        {
            if (character ==  Players[i].stateMachine) {
                this.playerPaused = i;
                break;
            }
        }
        this.pause = time;
    }
}