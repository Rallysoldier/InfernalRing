using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BlackGardenStudios.HitboxStudioPro;
using TeamRitual.Core;
using TeamRitual.Input;

namespace TeamRitual.Character {
public class CharacterStateMachine : ScriptableObject
{
    public int playerNumber;
    public float height;
    public float width;
    public string characterName;

    //Constant variables
    public float CONST_MAX_HEALTH = 1000;
    public float CONST_MAX_ENERGY = 1000;
    public const float CONST_GRAVITY = 0.85f;
    public Vector2 velocityWalkForward = new Vector2(2,0);
    public Vector2 velocityWalkBack = new Vector2(-2,0);
    public Vector2 velocityRunForward = new Vector2(15,0);
    public Vector2 velocityRunBack = new Vector2(-15,0);
    public Vector2 velocityAirdashForward = new Vector2(14,3);
    public Vector2 velocityAirdashBack = new Vector2(-14,3);
    public Vector2 velocityJumpNeutral = new Vector2(0,17);
    public Vector2 velocityJumpForward = new Vector2(6.5f,17f);
    public Vector2 velocityJumpBack = new Vector2(-6.5f,17f);
    public int CONST_MAX_AIRDASHES = 1;
    public int CONST_MAX_AIRJUMPS = 1;

    public int airdashCount = 0;
    public int airjumpCount = 0;
    public int maxAirdashes = 1;
    public int maxAirjumps = 1;

    RingMode ringMode = RingMode.FIRST;

    //input variables
    public InputHandler inputHandler;
    
    public SoundHandler soundHandler;
    public CharacterStateMachine enemy;

    //Animation Variables
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    //Physics/Motion Variables
    public Rigidbody2D body;
    public Vector2 velocity = new Vector2(0,0);
    public float standingFriction = 0.75f;
    public float crouchingFriction = 0.85f;
    public float gravity = 0.85f;
    public int facing;

    //Hit and health variables
    public float maxHealth = 1000;
    public float maxEnergy = 1000;
    public float health;
    public int hitstun;
    public int blockstun;
    public ContactData lastContact;
    public CharacterState lastContactState;

    //Energy variables
    private float energy;

    //state variables
    public CharacterState currentState;
    public CharacterStateFactory states;

    public ComboProcessor comboProcessor;
    public List<string> attackCancels = new List<string>();

    //getters and setters
    public CharacterState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterStateFactory State { get { return states; } set { states = value; } }

    //Temporary collision data, lasts one game tick
    public ContactSummary contactSummary;
    List<ContactData> bodyColData = new List<ContactData>();
    List<ContactData> hurtColData = new List<ContactData>();
    List<ContactData> guardColData = new List<ContactData>();
    List<ContactData> armorColData = new List<ContactData>();
    List<ContactData> grabColData = new List<ContactData>();
    List<ContactData> techColData = new List<ContactData>();
    //

    public CharacterStateMachine() {
        this.states = new CharacterStateFactory(this);
        this.contactSummary = new ContactSummary(this);
        this.comboProcessor = new ComboProcessor(this);
    }

    void Awake() {
        this.currentState = states.Stand();
        this.health = 1;
    }

    void Update() {
    }

    public void SetRingMode(RingMode ringMode) {
        this.ringMode = ringMode;
        switch (this.ringMode) {
            case RingMode.FOURTH:
                this.maxHealth = Mathf.Ceil(this.CONST_MAX_HEALTH*0.7f);
                break;
            case RingMode.SEVENTH:
                this.maxHealth = Mathf.Ceil(this.CONST_MAX_HEALTH*1.2f);
                break;
            case RingMode.EIGHTH:
                this.maxHealth = Mathf.Ceil(this.CONST_MAX_HEALTH*1.4f);
                this.maxAirdashes = 0;
                this.maxAirjumps = 0;
                break;
            case RingMode.NINTH:
                this.maxHealth = Mathf.Ceil(this.CONST_MAX_HEALTH*0.15f);
                break;
            case RingMode.FIFTH:
                this.maxAirdashes = this.CONST_MAX_AIRDASHES + 1;
                this.maxAirjumps = this.CONST_MAX_AIRJUMPS + 1;
                break;
        }
    }

    public RingMode GetRingMode() {
        return this.ringMode;
    }

    public virtual ContactSummary UpdateStates() {
        if (this.hitstun > 0) {
            this.hitstun--;
        }
        if (this.blockstun > 0) {
            this.blockstun--;
        }

        if (this.currentState.stateType != StateType.ATTACK) {
            this.attackCancels.Clear();
        }

        this.comboProcessor.Update();

        this.UpdateStatePhysics();

        this.ApplyHitVelocities();

        this.contactSummary.SetData(bodyColData,hurtColData,guardColData,armorColData,grabColData,techColData);
        this.ClearContactData();
        return this.contactSummary;
    }

    public void ApplyVelocity() {
        float velX = this.VelX()/500f;
        float resultPosX = this.PosX() + velX;

        float maxBound = GameController.Instance.StageMaxBound();
        float minBound = GameController.Instance.StageMinBound();

        float enemyWidth = this.enemy.width*2;

        bool closeToCornerEnemy = ((this.enemy.PosX() >= maxBound - 0.1f || this.enemy.PosX() <= minBound + 0.1f) && this.Distance(this.enemy.Pos()) < enemyWidth);

        if (resultPosX >= maxBound || resultPosX <= minBound || Mathf.Abs(resultPosX - this.enemy.PosX()) > 18.5f || closeToCornerEnemy) {
            velX = 0;
            if (resultPosX >= maxBound || resultPosX <= minBound) {
                this.PosX(resultPosX >= maxBound ? maxBound - 0.01f : minBound + 0.01f);
            }
            if (closeToCornerEnemy) {
                this.PosX(
                    this.enemy.PosX() + ((this.enemy.PosX() >= maxBound - 0.1f) ? -enemyWidth : enemyWidth)
                );
            }
        }

        this.SetPos(this.PosX() + velX, this.PosY() + this.VelY()/500f);
    }

    public void UpdateStatePhysics() {
        if (this.currentState.stateType == StateType.IDLE) {
            this.gravity = CONST_GRAVITY;
            if (this.currentState.moveType == MoveType.STAND) {
                airdashCount = 0;
                airjumpCount = 0;
            }
        }

        this.body.gravityScale = 0.0f;
        switch (this.currentState.physicsType)
        {
            default:
                break;
            case PhysicsType.STAND:
            case PhysicsType.CROUCH:
                float friction = this.currentState.physicsType == PhysicsType.STAND ? this.standingFriction : this.crouchingFriction;
                friction = (float) Mathf.Pow((float)friction, this.currentState.attackPriority > AttackPriority.NONE && 
                    this.currentState.attackPriority <= AttackPriority.HEAVY ? 0.5f : 1);
                this.VelXDirect(VelX()*friction);
                this.VelY(0);
                this.body.position = new Vector2(this.PosX(),0);
                break;
            case PhysicsType.AIR:
                VelYAdd(-this.gravity);
                break;
            case PhysicsType.CUSTOM:
                break;
        }
    }

    public void ApplyHitVelocities() {
        if (this.currentState.stateType == StateType.HURT && this.lastContact.HitVelocityTime > 0) {
            this.lastContact.HitVelocityTime--;
            this.SetVelocity(this.lastContact.HitVelocity);
        } else if (this.lastContact.BlockGroundVelocityTime > 0
            && (this.currentState is CommonStateGuardCrouch || this.currentState is CommonStateGuardStand)) {
            this.lastContact.BlockGroundVelocityTime--;
            this.SetVelocity(this.lastContact.BlockGroundVelocity);
        }
    }

    public virtual void ChangeStateOnInput() {
        string inputStr = this.GetInput();

        if (this.currentState.inputChangeState) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (inputStr.EndsWith("F,F") && !(this.currentState is CommonStateRunForward)) {
                    this.currentState.SwitchState(states.RunForward());
                } else if (inputStr.EndsWith("B,B") && !(this.currentState is CommonStateRunBack)) {
                    this.currentState.SwitchState(states.RunBack());
                } else if (((inputStr.EndsWith("D") || inputStr.EndsWith("D,F") || inputStr.EndsWith("F,D") || inputStr.EndsWith("D,B")  || inputStr.EndsWith("B,D")))
                     || inputHandler.held("D")) {
                    this.currentState.SwitchState(states.CrouchTransition());
                } else if (((inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U")))
                     || inputHandler.held("U")) {
                    this.currentState.SwitchState(states.JumpStart());
                } else if (!(this.currentState is CommonStateRunForward) && !(this.currentState is CommonStateWalkForward) && ((inputStr.EndsWith("F")) || inputHandler.held(inputHandler.ForwardInput(this)))) {
                    this.currentState.SwitchState(states.WalkForward());
                } else if (!(this.currentState is CommonStateRunBack) && !(this.currentState is CommonStateWalkBackward)  && (((inputStr.EndsWith("B")) || inputHandler.held(inputHandler.BackInput(this))))) {
                    this.currentState.SwitchState(states.WalkBackward());
                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                
            } else if (this.currentState.moveType == MoveType.AIR) {
                if (inputStr.EndsWith("B,B") && airdashCount < maxAirdashes && !(this.currentState is CommonStateAirdashBack) && this.currentState.stateTime >= 5) {
                    this.currentState.SwitchState(states.AirdashBack());
                } else if (inputStr.EndsWith("F,F") && airdashCount < maxAirdashes && !(this.currentState is CommonStateAirdashForward) && this.currentState.stateTime >= 5) {
                    this.currentState.SwitchState(states.AirdashForward());
                } else if (airjumpCount < maxAirjumps && airdashCount < maxAirdashes &&
                    this.currentState is CommonStateAirborne && this.currentState.stateTime >= 10 &&
                    (inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U")|| inputHandler.held("U"))) {
                    this.currentState.SwitchState(states.AirjumpStart());
                }
            }
        }
    }

    public string GetInput() {
        return this.inputHandler.characterInput;
    }

    public void ClearContactData() {
        this.bodyColData.Clear();
        this.hurtColData.Clear();
        this.guardColData.Clear();
        this.armorColData.Clear();
        this.grabColData.Clear();
        this.techColData.Clear();
    }

    public void SetPos(float posX, float posY) {
        this.body.position = new Vector2(posX,posY);
    }
    public Vector2 Pos() {
        return this.body.position;
    }

    public float Distance(Vector2 vecTo) {
        return Vector2.Distance(Pos(),vecTo);
    }
    public float XDistance(float x) {
        return Mathf.Sqrt(x*x + PosX()*PosX());
    }
    public float YDistance(float x) {
        return Mathf.Sqrt(x*x + PosX()*PosX());
    }

    public void PosX(float x) {
        this.SetPos(x, this.PosY());
    }
    public float PosX() {
        return this.body.position.x;
    }

    public void PosY(float y) {
        this.SetPos(PosX(), y);
    }
    public float PosY() {
        return this.body.position.y;
    }

    public float VelX() {
        return this.velocity.x;
    }

    public float VelY() {
        return this.velocity.y;
    }

    //Always use this to set velocity. Changes velocity based on the "facing" variable.
    public void SetVelocity(float velx, float vely) {
        this.velocity = new Vector2(velx*facing,vely);
    }

    public void SetVelocity(Vector2 velocity) {
        this.velocity = new Vector2(velocity.x*facing,velocity.y);
    }

    public void VelX(float velocity) {
        this.SetVelocity(velocity,VelY());
    }

    public void VelXDirect(float velocity) {
        this.velocity = new Vector2(velocity, VelY());
    }

    public void VelY(float velocity) {
        this.velocity = new Vector2(VelX(), velocity);
    }

    public void VelYAdd(float velocity) {
        this.velocity = new Vector2(VelX(), VelY() + velocity);
    }

    public void correctFacing() {
        float oldFacing = this.facing;
        this.facing = this.PosX() < this.enemy.PosX() ? 1 : -1;

        this.spriteRenderer.flipX = this.facing == -1;
    }

    public virtual float GetEnergy() {
        if (this.ringMode == RingMode.NINTH)
            return this.maxEnergy;

        return this.energy;
    }

    public virtual void AddEnergy(float energy) {
        switch (this.ringMode) {
            case RingMode.THIRD:
                energy = energy > 0 ? energy/1.5f : energy;
                break;
            case RingMode.FOURTH:
                energy = energy > 0 ? energy*1.5f : energy;
                break;
        }

        this.energy = Mathf.Clamp(this.energy + energy, 0, maxEnergy);
    }

    public string GetCurrentAnimationName() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public bool StateAnimationOver() {
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName(this.currentState.animationName)
            && this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            return true;

        return false;
    }

    public void UpdateEffects() {
        this.UpdateFlash();
        this.EXEffect();
    }

    int flashTime;
    Vector4 flashColor;
    public void Flash(Vector4 color, int time) {
        flashColor = color;
        flashTime = time;
    }
    public void UpdateFlash() {
        if (flashTime > 0) {
            this.spriteRenderer.color = Color.Lerp(flashColor, Color.white, Time.deltaTime * 15);
            if (GameController.Instance.pause == 0) {
                flashTime--;
            }
        } else {
            this.spriteRenderer.color = Color.white;
        }
    }
    public void EXEffectStart() {
        this.AddEnergy(-500);
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(100), true);
    }
    public void EXEffect() {
        if (this.currentState.EXFlash && GameController.Instance.Global_Time%4 == 0) {
            this.Flash(new Vector4(2.0f,2.0f,1.0f,1.0f), 2);
        }
    }

    public virtual void HitboxContact(ContactData data) {
        switch (data.MyHitbox.Type)
        {
            case HitboxType.TRIGGER:
                bodyColData.Add(data);
                break;
            case HitboxType.HURT:
                hurtColData.Add(data);
                break;
            case HitboxType.GUARD:
                guardColData.Add(data);
                break;
            case HitboxType.ARMOR:
                armorColData.Add(data);
                break;
            case HitboxType.GRAB:
                grabColData.Add(data);
                break;
            case HitboxType.TECH:
                techColData.Add(data);
                break;
        }
    }

    public virtual bool Block(ContactData hit, GuardType hitGuardType) {
        if (this.ringMode == RingMode.SIXTH) {
            return false;
        }

        GameController.Instance.Pause(hit.Blockpause);
        this.blockstun = hit.Blockstun;
        this.health -= hit.ChipDamage;

        this.SetVelocity(hit.BlockGroundVelocity);

        switch(hitGuardType) {
            case GuardType.MID:
                this.currentState.SwitchState(
                    this.currentState.moveType == MoveType.CROUCH ? states.GuardCrouch()
                    : this.currentState.moveType == MoveType.STAND ? states.GuardStand() : states.GuardAir()
                );
                break;
            case GuardType.HIGH:
                this.currentState.SwitchState(
                    this.currentState.moveType == MoveType.STAND ? states.GuardStand() : states.GuardAir()
                );
                break;
            case GuardType.LOW:
                this.currentState.SwitchState(states.GuardCrouch());
                break;
        }

        this.lastContact = hit;
        this.lastContactState = this.enemy.currentState;

        EffectSpawner.PlayHitEffect(10, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX);
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1), hit.StopSounds);
        return true;
    }

    //Overload this function to have the character do something else when they're hit
    public virtual bool Hit(ContactData hit) {
        //Avoid multiple hits within the same animation keyframe, if a hit has already landed.
        if (this.lastContact.HitFrame == hit.HitFrame && lastContactState == this.enemy.currentState) {
            //Debug.Log(this.lastContact.HitFrame + " " + hit.HitFrame);
            //Debug.Log(lastContactState + " " + this.enemy.currentState);
            return false;
        }

        if (this.currentState.moveType == MoveType.LYING && !hit.DownedHit) {
            return false;
        }

        if (this.currentState.immuneMoveTypes.Contains(this.enemy.currentState.moveType) || this.currentState.immunePriorities.Contains(this.enemy.currentState.attackPriority)) {
            return false;
        }
        
        if (!this.currentState.OnHurt() || !this.enemy.currentState.OnHitEnemy()) {
            return false;
        }

        bool hitDownedEnemy = this.currentState.moveType == MoveType.LYING && this.lastContact.DownedHit;
        this.enemy.comboProcessor.ProcessHit(hit, this.enemy.currentState);
        this.health -= this.enemy.comboProcessor.GetDamage(hitDownedEnemy);
        this.hitstun = this.enemy.comboProcessor.GetHitstun(hitDownedEnemy);
        this.health = Mathf.Max(this.health, 0f);

        this.AddEnergy(hit.GiveEnemyPower);
        this.enemy.AddEnergy(this.enemy.comboProcessor.GetSelfEnergy());

        hit.HitFall = (this.currentState.moveType == MoveType.AIR && hit.FallAir)
            || (this.currentState.moveType != MoveType.AIR && hit.FallGround);

        if (this.health == 0 && hit.AttackPriority < AttackPriority.SPECIAL) {
            if (hit.AttackPriority == AttackPriority.LIGHT) {
                hit.HitGroundVelocity = Vector2.zero;
                hit.HitAirVelocity = Vector2.zero;
                hit.DownedVelocity = Vector2.zero;
                hit.HitFall = false;
                hit.ForceStand = true;
                this.hitstun = 30;
            } else {
                if (hit.HitGroundVelocity.y == 0 && (this.currentState.moveType == MoveType.STAND || this.currentState.moveType == MoveType.CROUCH)
                    || hit.HitAirVelocity.y == 0 && this.currentState.moveType == MoveType.AIR
                    || hit.DownedVelocity.y == 0 && this.currentState.moveType == MoveType.LYING) {
                    hit.HitGroundVelocity = new Vector2(hit.HitGroundVelocity.x,13);
                    hit.HitAirVelocity = new Vector2(hit.HitAirVelocity.x,13);
                    hit.DownedVelocity = new Vector2(hit.DownedVelocity.x,13);
                }
            }
        }

        if (hit.WallBounceTime > 0 && !this.enemy.comboProcessor.AddWallBounce()) {
            hit.WallBounceTime = 0;
        }
        if (hit.Bounce != Vector2.zero && hit.Bounce.y > 0 && !this.enemy.comboProcessor.AddGroundBounce()) {
            hit.Bounce = Vector2.zero;
        }

        if (this.currentState.moveType == MoveType.STAND) {
            hit.HitVelocity = hit.HitGroundVelocity;
            hit.HitVelocityTime = hit.HitGroundVelocityTime;
            if (hit.HitVelocity.y > 0) {
                this.currentState.SwitchState(states.HurtAir());
            } else {
                this.currentState.SwitchState(states.HurtStand());
            }
        } else if (this.currentState.moveType == MoveType.CROUCH) {
            hit.HitVelocity = hit.HitGroundVelocity;
            hit.HitVelocityTime = hit.HitGroundVelocityTime;
            if (hit.HitVelocity.y > 0) {
                this.currentState.SwitchState(states.HurtAir());
            } else {
                this.currentState.SwitchState(states.HurtCrouch());
            }
        } else if (this.currentState.moveType == MoveType.LYING) {
            hit.HitVelocity = hit.DownedVelocity;
            hit.HitVelocityTime = 1;
            if (hit.HitVelocity.y > 0) {
                this.currentState.SwitchState(states.HurtAir());
            } else {
                this.currentState.SwitchState(states.HurtStand());
            }
        } else if (this.currentState.moveType == MoveType.AIR) {
            hit.HitVelocity = hit.HitAirVelocity;
            hit.HitVelocityTime = hit.HitAirVelocityTime;
            this.currentState.SwitchState(states.HurtAir());
        }

        if (hit.ForceStand && this.currentState.moveType != MoveType.AIR) {
            this.currentState.SwitchState(states.HurtStand());
        }
        
        this.SetVelocity(hit.HitVelocity);

        if (hit.FlipEnemy) {
            this.facing *= -1;
        }

        this.lastContact = hit;
        this.lastContactState = this.enemy.currentState;

        this.correctFacing();

        EffectSpawner.PlayHitEffect(hit.fxID, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX);
        if (EffectSpawner.GetSoundEffect(hit.SoundID) != null) {
            GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(hit.SoundID), hit.StopSounds);
        }
        //GameController.Instance.soundHandler.audioSource.PlayOneShot((AudioClip)Resources.Load("Sounds/SFX/Hit/hit-1"));

        GameController.Instance.Pause(hit.Hitpause);
        return true;
    }

    //If true, allows higher priority basic attacks to be chained into lower priority attacks.
    public virtual bool ReverseBeat() {
        return this.energy >= maxEnergy;
    }
}
}