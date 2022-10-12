using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackGardenStudios.HitboxStudioPro;
using TeamRitual.Character;
using TeamRitual.Stage;

namespace TeamRitual.Core {
public class GameController : MonoBehaviour {
    public static GameController Instance;

    [SerializeField]
    public List<string> characterNames = new List<string>{"Xonin","Xonin"};
    [SerializeField]
    public List<int> selectedPalettes = new List<int>{1,3};
    [SerializeField]
    public string StageName = "BloodMoon";

    public BaseStageObj Stage;
    public List<PlayerGameObj> Players;

    public int Global_Time = 0;
    //Determines which player paused the game during a character pause
    public int playerPaused = -1;
    public int pause = 0;

    float cameraLerp = 10f;

    [SerializeField]
    public int MaxRoundTime = 90;
    public int remainingRoundTime;
    public GameObject TimerUI;
    Text Timer ;

    public GameController() {
        Instance = this;
    }

    void Start()
    {
        remainingRoundTime = MaxRoundTime;

        Timer = TimerUI.transform.GetComponent<Text>();
        Timer.text = ""+remainingRoundTime;
        GameObject P1HealthBar = GameObject.Find("P1HealthBar");
        GameObject P2HealthBar = GameObject.Find("P2HealthBar");
        Image P1HealthBarFill = null;
        Image P2HealthBarFill = null;
        if (P1HealthBar != null)
        {
            P1HealthBarFill = P1HealthBar.transform.GetChild(0).GetComponent<Image>();
            Debug.Log(P1HealthBar.transform.GetChild(0).name);
        }
        if (P2HealthBar != null)
        {
            P2HealthBarFill = P2HealthBar.transform.GetChild(0).GetComponent<Image>();
            Debug.Log(P2HealthBar.transform.GetChild(0).name);
        }

        GameObject stageObj = Instantiate(Resources.Load("Prefabs/Stages/StagePrefab_BloodMoon", typeof(GameObject))) as GameObject;
        Stage = stageObj.GetComponent<BaseStageObj>();
        
        for (int i = 0; i < 2; i++) {
            GameObject playerGO = Instantiate(Resources.Load("Prefabs/Characters/CharacterPrefab_"+characterNames[i], typeof(GameObject))) as GameObject;
            Instantiate(playerGO);
            Players.Add(playerGO.GetComponent<PlayerGameObj>());
        }
        
        float startPosP1 = -5.0f;
        for (int i = 0; i < Players.Count; i++)
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
                playerObj.healthBarFill = P1HealthBarFill;
            } else {
                playerObj.inputHandler.mapP2Inputs();
                playerObj.healthBarFill = P2HealthBarFill;
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
            Players[i].stateMachine.updateInputHandler();

            if (this.pause > 0) {
                if (i != this.playerPaused) {
                    if (Players[i].m_Animator != null) {
                        Players[i].m_Animator.enabled = false;
                    }
                    if (Players[i].m_RigidBody != null) {
                        Players[i].m_RigidBody.Sleep();
                    }
                }
            } else {
                this.playerPaused = -1;
                if (Players[i].m_Animator != null) {
                    Players[i].m_Animator.enabled = true;
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

        if (P1_Hits.NotEmpty() && Players[0].stateMachine.currentState.stateType == StateType.ATTACK
         || P2_Hits.NotEmpty() && Players[1].stateMachine.currentState.stateType == StateType.ATTACK) {
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
                    if (characterHitting.currentState.moveContact >= hit.AttackHits)
                        break;

                    bool blocked = false;
                    if (characterHurt.currentState.inputChangeState || characterHurt.blockstun > 0) {
                        bool enemyHoldingBack = characterHurt.inputHandler.held(characterHurt.inputHandler.BackInput(characterHurt));
                        bool enemyHoldingDown = characterHurt.inputHandler.held("D");
                        if (enemyHoldingBack && hit.GuardType != GuardType.UNBLOCKABLE && characterHurt.currentState.moveType != MoveType.AIR) {//If holding back and attack isn't unblockable
                            if (hit.GuardType == GuardType.MID) {
                                blocked = true;
                            } else if (enemyHoldingDown && hit.GuardType == GuardType.LOW) {
                                blocked = true;
                            } else if (!enemyHoldingDown && hit.GuardType == GuardType.HIGH) {
                                blocked = true;
                            }
                        }
                    }

                    if (blocked) {
                        blocked = characterHurt.Block(hit, hit.GuardType);
                        if (blocked) {
                            characterHitting.currentState.moveContact++;
                        }
                    }
                    if (!blocked) {
                        if (characterHurt.Hit(hit)) {
                            characterHitting.currentState.moveContact++;
                            characterHitting.currentState.moveHit++;
                            Debug.Log(characterHitting.currentState.moveContact +" "+ hit.AttackHits);
                        }
                    }
                }
            } else if (winningPriority > 0 && (Players[0].stateMachine.currentState.stateType == StateType.ATTACK || Players[1].stateMachine.currentState.stateType == StateType.ATTACK)) {
                Pause(winningPriority*5);
                if (P1_Hits.bodyColData.Count > 0) {
                    EffectSpawner.PlayHitEffect(
                        10, P1_Hits.bodyColData[0].Point, Players[0].stateMachine.spriteRenderer.sortingOrder + 1, !P1_Hits.bodyColData[0].TheirHitbox.Owner.FlipX
                    );
                } else if (P2_Hits.bodyColData.Count > 0) {
                    EffectSpawner.PlayHitEffect(
                        10, P2_Hits.bodyColData[0].Point, Players[1].stateMachine.spriteRenderer.sortingOrder + 1, !P2_Hits.bodyColData[0].TheirHitbox.Owner.FlipX
                    );
                }
            }
        }

        //Always clear hit lists at the end
        P1_Hits.Clear();
        P2_Hits.Clear();

        //Update HUD
        EffectHealth(Players[0]);
        EffectHealth(Players[1]);
        if (Global_Time%80 == 0 && this.pause == 0) {
            CountDownTimer();
        }
    }

    void Update() {//Camera movement by linearly interpolating through points

        //TODO: Add more camera modes: CameraFocus.Player1, CameraFocus.Player2, CameraFocus.Both, CameraFocus.None
        float avgX = (Players[0].m_RigidBody.position.x + Players[1].m_RigidBody.position.x)/2;
        float avgY = (Players[0].m_RigidBody.position.y + Players[1].m_RigidBody.position.y)/2;

        avgX = Mathf.Clamp(avgX, -11f + 4, 11f - 4);

        Vector3 cameraDestination = new Vector3(avgX, avgY + 2.5f, -10); //Replace 2.5f with the average of characters' heights
        LerpCamera(cameraDestination);
    }

    void LerpCamera(Vector3 cameraDestination) {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraDestination, cameraLerp * Time.deltaTime);
    }

    public void SetCameraPos(Vector2 pos) {
        Vector3 cameraDestination = new Vector3(pos.x, pos.y, -10);
        Camera.main.transform.position = cameraDestination;
    }

    public void SetCameraPos(float x, float y) {
        Vector3 cameraDestination = new Vector3(x, y,-10);
        Camera.main.transform.position = cameraDestination;
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

    public void EffectHealth(PlayerGameObj player)
    {
        player.healthBarFill.fillAmount = Mathf.Lerp(player.healthBarFill.fillAmount,player.stateMachine.health * 0.001f, 75f * Time.deltaTime);
        if (player.stateMachine.health >= 0)
        {
            
        }
        else
        {
            Debug.Log("Game Over");
        }
    }

    void CountDownTimer()
    {
        Timer = TimerUI.transform.GetComponent<Text>();

        if (this.remainingRoundTime > 0)
        {
            this.remainingRoundTime--;
            Timer.text = ""+this.remainingRoundTime;
        }
        else
        {
            Debug.Log("Game Over");
        }
    }
}
}