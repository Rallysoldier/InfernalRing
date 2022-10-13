using System.Collections.Generic;
using UnityEngine;
using BlackGardenStudios.HitboxStudioPro;
using TeamRitual.Core;
using TeamRitual.Input;

namespace TeamRitual.Character {
public class CharacterStateMachine : ScriptableObject
{
    public string characterName;
    //Misc Variables
    public InputHandler inputHandler;
    public CharacterStateMachine enemy;

    //Animation Variables
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    //Physics/Motion Variables
    public Rigidbody2D body;
    public int facing;
    public Vector2 hitVelocity;
    public int hitVelocityTime;
    public Vector2 blockVelocity;
    public int blockVelocityTime;

    public const int MAX_HEALTH = 1000;
    public int health;
    public int hitstun;
    public int blockstun;
    public int cancelPriority = 0;

    public Vector2 velocityWalkForward = new Vector2(2,0);
    public Vector2 velocityWalkBack = new Vector2(-2,0);
    public Vector2 velocityRunForward = new Vector2(15,0);
    public Vector2 velocityRunBack = new Vector2(-15,0);
    public Vector2 velocityJumpNeutral = new Vector2(0,17);
    public Vector2 velocityJumpForward = new Vector2(6.5f,17f);
    public Vector2 velocityJumpBack = new Vector2(-6.5f,17f);
    public float standingFriction = 0.05f;
    public float crouchingFriction = 0.15f;
    public float gravity = 0.85f;

    //state variables
    public CharacterState currentState;
    public CharacterStateFactory states;
    public string inputStr = "";
    string prevInputStr = "";
    public bool changedInput;

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

    public CharacterStateMachine() {
        this.states = new CharacterStateFactory(this);
        this.contactSummary = new ContactSummary(this);
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
                this.SetVelocity(-8,9);
                this.currentState.SwitchState(this.states.HurtAir());
            }
        }

        body.gravityScale = 0.0f;
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
                    && this.currentState.moveHit > 0) {
                    VelYAdd(-this.gravity/3);
                } else {
                    VelYAdd(-this.gravity);
                }
                break;
            case PhysicsType.CUSTOM:
                break;
        }

        if (this.hitVelocityTime > 0) {
            this.hitVelocityTime--;
            this.SetVelocity(this.hitVelocity);
        }

        if (this.blockVelocityTime > 0) {
            this.blockVelocityTime--;
            this.SetVelocity(this.blockVelocity);
        }

        this.changeStateOnInput();

        this.changedInput = false;
        this.prevInputStr = inputStr;

        contactSummary.SetData(bodyColData,hurtColData,guardColData,armorColData,grabColData,techColData);
        this.bodyColData.Clear();
        this.hurtColData.Clear();
        this.guardColData.Clear();
        this.armorColData.Clear();
        this.grabColData.Clear();
        this.techColData.Clear();
        return contactSummary;
    }

    public void updateInputHandler() {
        //Gets new character input based on the direction they're facing.
        //Inverts F and B inputs if the character is facing the -x direction (facing == -1)
        this.inputStr = this.inputHandler.getCharacterInput(this);
        if (inputStr != prevInputStr) {
            changedInput = true;
        }
        prevInputStr = inputStr;
        this.inputHandler.updateBufferTime();
    }

    public virtual void changeStateOnInput() {
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

    public float PosX() {
        return this.body.position.x;
    }

    public float PosY() {
        return this.body.position.y;
    }

    public float VelX() {
        return this.body.velocity.x;
    }

    public float VelY() {
        return this.body.velocity.y;
    }

    //Always use this to set velocity. Changes velocity based on the "facing" variable.
    public void SetVelocity(float velx, float vely) {
        body.velocity = new Vector2(velx*facing,vely);
    }

    public void SetVelocity(Vector2 velocity) {
        body.velocity = new Vector2(velocity.x*facing,velocity.y);
    }

    public void VelX(float velocity) {
        this.SetVelocity(velocity,VelY());
    }

    public void VelXDirect(float velocity) {
        body.velocity = new Vector2(velocity,VelY());
    }

    public void VelY(float velocity) {
        body.velocity = new Vector2(VelX(),velocity);
    }

    public void VelYAdd(float velocity) {
        body.velocity = new Vector2(VelX(),VelY() + velocity);
    }

    public void correctFacing() {
        float oldFacing = this.facing;
        this.facing = this.PosX() < this.enemy.PosX() ? 1 : -1;

        this.spriteRenderer.flipX = this.facing == -1;
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

        this.blockVelocity = hit.BlockGroundVelocity;
        this.SetVelocity(hit.BlockGroundVelocity);
        this.blockVelocityTime = hit.BlockGroundVelocityTime;

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
        
        EffectSpawner.PlayHitEffect(0, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX);
        return true;
    }

    //Overload this function to have the character do something else when they're hit
    public virtual bool Hit(ContactData hit) {
        if (this.health == 0f) {
            return false;
        }

        this.health -= (int) hit.Damage;
        this.health = (int) Mathf.Max(this.health,0f);

        GameController.Instance.Pause(hit.Hitpause);
        this.hitstun = hit.Hitstun;

        switch (this.currentState.moveType) {
            case MoveType.STAND:
                this.hitVelocity = hit.HitGroundVelocity;
                this.SetVelocity(hit.HitGroundVelocity);
                this.hitVelocityTime = hit.HitGroundVelocityTime;
                if (this.hitVelocity.y > 0 || this.health == 0) {
                    this.currentState.SwitchState(states.HurtAir());
                } else {
                    this.currentState.SwitchState(states.HurtStand());
                }
                break;
            case MoveType.CROUCH:
                this.hitVelocity = hit.HitGroundVelocity;
                this.SetVelocity(hit.HitGroundVelocity);
                this.hitVelocityTime = hit.HitGroundVelocityTime;
                if (this.hitVelocity.y > 0 || this.health == 0) {
                    this.currentState.SwitchState(states.HurtAir());
                } else {
                    this.currentState.SwitchState(states.HurtStand());
                }
                break;
            case MoveType.AIR:
                this.hitVelocity = hit.HitAirVelocity;
                this.SetVelocity(hit.HitAirVelocity);
                this.hitVelocityTime = hit.HitAirVelocityTime;
                this.currentState.SwitchState(states.HurtAir());
                break;
        }

        switch (hit.AttackPriority) {
            case AttackPriority.LIGHT:
                EffectSpawner.PlayHitEffect(
                    100, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX
                );
                break;
            case AttackPriority.MEDIUM:
                EffectSpawner.PlayHitEffect(
                    200, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX
                );
                break;
            case AttackPriority.HEAVY:
                EffectSpawner.PlayHitEffect(
                    300, hit.Point, spriteRenderer.sortingOrder + 1, !hit.TheirHitbox.Owner.FlipX
                );
                break;
        }
        
        return true;
    }
}
}