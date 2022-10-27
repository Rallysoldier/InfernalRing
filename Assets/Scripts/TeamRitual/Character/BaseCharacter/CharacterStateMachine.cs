using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BlackGardenStudios.HitboxStudioPro;
using TeamRitual.Core;
using TeamRitual.Input;

namespace TeamRitual.Character {
public class CharacterStateMachine : ScriptableObject
{
    public string characterName;

    //Constant variables
    public const int MAX_HEALTH = 1000;
    public const int MAX_ENERGY = 1000;
    public const float CONST_GRAVITY = 0.85f;
    public Vector2 velocityWalkForward = new Vector2(2,0);
    public Vector2 velocityWalkBack = new Vector2(-2,0);
    public Vector2 velocityRunForward = new Vector2(15,0);
    public Vector2 velocityRunBack = new Vector2(-15,0);
    public Vector2 velocityJumpNeutral = new Vector2(0,17);
    public Vector2 velocityJumpForward = new Vector2(6.5f,17f);
    public Vector2 velocityJumpBack = new Vector2(-6.5f,17f);

    //input variables
    public InputHandler inputHandler;
    public string inputStr = "";
    public bool changedInput;
    
    public SoundHandler soundHandler;
    public CharacterStateMachine enemy;

    //Animation Variables
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    //Physics/Motion Variables
    public Rigidbody2D body;
    public Vector2 velocity = new Vector2(0,0);
    public float standingFriction = 0.05f;
    public float crouchingFriction = 0.15f;
    public float gravity = 0.85f;
    public int facing;

    //Hit and health variables
    public int health;
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
        this.health = MAX_HEALTH;
    }

    void Update() {
    }

    public virtual ContactSummary UpdateStates() {
        if (this.hitstun > 0) {
            this.hitstun--;
        }
        if (this.blockstun > 0) {
            this.blockstun--;
        }

        if (this.currentState.stateType != StateType.HURT) {
            if (this.health == 0) {
                this.currentState.SwitchState(this.states.HurtAir());
            }
        }
        if (this.currentState.stateType != StateType.ATTACK) {
            this.attackCancels.Clear();
        }

        this.comboProcessor.Update();

        this.UpdateStatePhysics();

        this.ApplyHitVelocities();

        this.ChangeStateOnInput();

        this.contactSummary.SetData(bodyColData,hurtColData,guardColData,armorColData,grabColData,techColData);
        this.ClearContactData();
        return contactSummary;
    }

    public void ApplyVelocity() {
        float velX = this.VelX()/500f;
        float resultPosX = this.PosX() + velX;

        if (resultPosX >= GameController.Instance.StageMaxBound() || resultPosX <= GameController.Instance.StageMinBound()
        || Mathf.Abs(resultPosX - this.enemy.PosX()) > 18.5f) {
            velX = 0;
        }
        this.SetPos(this.PosX() + velX, this.PosY() + this.VelY()/500f);
    }

    public void UpdateStatePhysics() {
        if (this.currentState.stateType == StateType.IDLE) {
            this.gravity = CONST_GRAVITY;
        }

        this.body.gravityScale = 0.0f;
        switch (this.currentState.physicsType)
        {
            default:
                break;
            case PhysicsType.STAND:
                this.VelXDirect(VelX()*this.standingFriction);
                this.VelY(0);
                this.body.position = new Vector2(this.PosX(),0);
                break;
            case PhysicsType.CROUCH:
                this.VelXDirect(VelX()*this.crouchingFriction);
                this.VelY(0);
                this.body.position = new Vector2(this.PosX(),0);
                break;
            case PhysicsType.AIR:
                if (this.currentState.stateType == StateType.ATTACK && this.currentState.attackPriority < AttackPriority.SPECIAL
                    && this.currentState.moveHit > 0 && this.enemy.currentState.moveType == MoveType.AIR) {
                    VelYAdd(-this.gravity/3);
                } else {
                    VelYAdd(-this.gravity);
                }
                break;
            case PhysicsType.CUSTOM:
                break;
        }
    }

    public void ApplyHitVelocities() {
        if (this.currentState.stateType == StateType.HURT && this.lastContact.HitVelocityTime > 0) {
            this.lastContact.HitVelocityTime--;
            this.SetVelocity(this.lastContact.HitVelocity);
        } else if (this.lastContact.BlockGroundVelocityTime > 0) {
            this.lastContact.BlockGroundVelocityTime--;
            this.SetVelocity(this.lastContact.BlockGroundVelocity);
        }
    }

    public virtual void ChangeStateOnInput() {
        if (this.currentState.inputChangeState) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (inputStr.EndsWith("F,F") && this.changedInput) {
                    this.currentState.SwitchState(states.RunForward());
                } else if (inputStr.EndsWith("B,B") && this.changedInput) {
                    this.currentState.SwitchState(states.RunBack());
                } else if ((this.changedInput && (inputStr.EndsWith("D") || inputStr.EndsWith("D,F") || inputStr.EndsWith("F,D") || inputStr.EndsWith("D,B")  || inputStr.EndsWith("B,D")))
                     || inputHandler.held("D")) {
                    this.currentState.SwitchState(states.CrouchTransition());
                } else if ((this.changedInput && (inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U")))
                     || inputHandler.held("U")) {
                    this.currentState.SwitchState(states.JumpStart());
                } else if (!(this.currentState is CommonStateRunForward) && ((inputStr.EndsWith("F") && this.changedInput) || inputHandler.held(inputHandler.ForwardInput(this)))) {
                    this.currentState.SwitchState(states.WalkForward());
                } else if (!(this.currentState is CommonStateRunBack) && (((inputStr.EndsWith("B") && this.changedInput) || inputHandler.held(inputHandler.BackInput(this))))) {
                    this.currentState.SwitchState(states.WalkBackward());
                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                
            } else if (this.currentState.moveType == MoveType.AIR) {
                
            }
        }
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

    public void SetPosX(float x) {
        this.SetPos(x, this.PosY());
    }
    public float PosX() {
        return this.body.position.x;
    }

    public void SetPosY(float y) {
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
        return this.energy;
    }

    public virtual void AddEnergy(float energy) {
        this.energy = this.energy + energy >= MAX_ENERGY ? MAX_ENERGY : this.energy + energy;
    }

    public string GetCurrentAnimationName() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
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
        GameController.Instance.Pause(hit.Blockpause);
        this.blockstun = hit.Blockstun;
        this.health -= (int)hit.ChipDamage;

        this.SetVelocity(hit.BlockGroundVelocity);

        switch(hitGuardType) {
            case GuardType.MID:
                this.currentState.SwitchState(
                    this.currentState.moveType == MoveType.CROUCH ? states.GuardCrouch() : states.GuardStand()
                );
                break;
            case GuardType.HIGH:
                this.currentState.SwitchState(states.GuardStand());
                break;
            case GuardType.LOW:
                this.currentState.SwitchState(states.GuardCrouch());
                break;
        }

        this.lastContact = hit;
        this.lastContactState = this.enemy.currentState;
        
        EffectSpawner.PlayHitEffect(0, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX);
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(0), hit.StopSounds);
        return true;
    }

    //Overload this function to have the character do something else when they're hit
    public virtual bool Hit(ContactData hit) {
        if (this.health == 0f) {
            return false;
        }

        //Avoid multiple hits within the same animation keyframe, if a hit has already landed.
        if (this.lastContact.HitFrame == hit.HitFrame && lastContactState == this.enemy.currentState) {
            //Debug.Log(this.lastContact.HitFrame + " " + hit.HitFrame);
            //Debug.Log(lastContactState + " " + this.enemy.currentState);
            return false;
        }

        if (this.currentState.moveType == MoveType.LYING && !this.lastContact.DownedHit) {
            return false;
        }

        if (this.currentState.moveType == MoveType.STAND) {
            hit.HitVelocity = hit.HitGroundVelocity;
            hit.HitVelocityTime = hit.HitGroundVelocityTime;
            if (hit.HitVelocity.y > 0 || this.health == 0) {
                this.currentState.SwitchState(states.HurtAir());
            } else {
                this.currentState.SwitchState(states.HurtStand());
            }
        } else if (this.currentState.moveType == MoveType.CROUCH) {
            hit.HitVelocity = hit.HitGroundVelocity;
            hit.HitVelocityTime = hit.HitGroundVelocityTime;
            if (hit.HitVelocity.y > 0 || this.health == 0) {
                this.currentState.SwitchState(states.HurtAir());
            } else {
                this.currentState.SwitchState(states.HurtCrouch());
            }
        } else if (this.currentState.moveType == MoveType.LYING) {
            hit.HitVelocity = hit.DownedVelocity;
            hit.HitVelocityTime = 1;
            if (hit.HitVelocity.y > 0 || this.health == 0) {
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

        bool hitDownedEnemy = this.currentState.moveType == MoveType.LYING && this.lastContact.DownedHit;
        this.enemy.comboProcessor.ProcessHit(hit, this.enemy.currentState);
        this.health -= this.enemy.comboProcessor.GetDamage(hitDownedEnemy);
        this.hitstun = this.enemy.comboProcessor.GetHitstun(hitDownedEnemy);
        this.health = (int) Mathf.Max(this.health, 0f);

        this.AddEnergy(hit.GiveEnemyPower);
        this.enemy.AddEnergy(hit.GiveSelfPower);

        hit.HitFall = (this.currentState.moveType == MoveType.AIR && hit.FallAir)
            || (this.currentState.moveType != MoveType.AIR && hit.FallGround);

        this.lastContact = hit;
        this.lastContactState = this.enemy.currentState;

        EffectSpawner.PlayHitEffect(hit.fxID, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX);
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(hit.SoundID), hit.StopSounds);
        //GameController.Instance.soundHandler.audioSource.PlayOneShot((AudioClip)Resources.Load("Sounds/SFX/Hit/hit-1"));

        GameController.Instance.Pause(hit.Hitpause);
        return true;
    }

    //If true, allows higher priority basic attacks to be chained into lower priority attacks.
    public virtual bool ReverseBeat() {
        return false;
    }
}
}