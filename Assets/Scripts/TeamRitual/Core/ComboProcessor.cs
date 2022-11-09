using BlackGardenStudios.HitboxStudioPro;
using System.Collections.Generic;
using TeamRitual.Character;
using UnityEngine;

namespace TeamRitual.Core {
public class ComboProcessor {
    private CharacterStateMachine character;

    private const float MIN_DAMAGE_SCALING = 0.05f;
    private Dictionary<AttackPriority,float> starterScalings = new Dictionary<AttackPriority,float>();
    private float[] stepDeductions = new float[] {
        -0.05f, -0.05f, -0.05f, -0.05f, -0.05f, -0.05f, -0.05f, 0, 0, 0, -0.025f, 0, 0, -0.025f, 0, 0, -0.025f, 0
    };

    private int comboTime = 0;
    private List<ContactData> comboHits = new List<ContactData>();
    private float currentScaling = 1.0f;
    private int scalingStep = 0;
    private int maxGroundBounce = 2;
    private int groundBounce = 0;
    private int maxWallBounce = 2;
    private int wallBounce = 0;

    private CharacterState recentHitState = null;

    public ComboProcessor(CharacterStateMachine character) {
        this.character = character;

        starterScalings[AttackPriority.NONE] = 0.5f;
        starterScalings[AttackPriority.THROW] = 0.6f;
        starterScalings[AttackPriority.LIGHT] = 0.7f;
        starterScalings[AttackPriority.MEDIUM] = 0.9f;
        starterScalings[AttackPriority.HEAVY] = 1.0f;
        starterScalings[AttackPriority.SPECIAL] = 0.8f;
        starterScalings[AttackPriority.SUPER] = 1.0f;
    }

    public void Update() {
        if (character.enemy.currentState.stateType != StateType.HURT) {
            if (this.comboTime != 0) {
                Clear();
            }
        } else {
            this.comboTime++;
        }
    }

    private void Clear() {
        this.comboTime = 0;
        this.comboHits.Clear();
        this.currentScaling = 1.0f;
        this.scalingStep = 0;
        this.recentHitState = null;
        this.groundBounce = 0;
        this.wallBounce = 0;
    }

    public void ProcessHit(ContactData hit, CharacterState state) {
        float damageScaling = 1.0f;
        int hitstunDecay = 0;

        //change current scaling
        if (this.comboHits.Count == 0) {
            this.currentScaling = starterScalings[hit.AttackPriority];
        } else {
            //Update damage scaling
            if (this.scalingStep > 0) {
                this.currentScaling = this.currentScaling + this.stepDeductions[this.scalingStep - 1];
                this.currentScaling = Mathf.Max(this.currentScaling, MIN_DAMAGE_SCALING);
            }
            damageScaling = this.currentScaling;

            //Update hitstun scaling
            hitstunDecay = (int) Mathf.Floor(comboTime/60f);
        }

        //Update scaling step on the first hit of the attack
        if (this.recentHitState != null && this.recentHitState.GetType() != state.GetType()) {
            this.scalingStep = (int) Mathf.Max(0, this.scalingStep + state.scalingStep);
        }
        
        //change damage and downedDamage based on scaling
        hit.Damage = hit.Damage * damageScaling;
        hit.DownedDamage = hit.DownedDamage * damageScaling;

        hit.GiveSelfPower = hit.GiveSelfPower * damageScaling;

        //change hitstun and downedHitstun based on scaling
        hit.Hitstun = Mathf.Max(hit.Hitstun - hitstunDecay, 0);
        hit.DownedHitstun = Mathf.Max(hit.DownedHitstun - hitstunDecay, 0);

        this.recentHitState = state;
        this.comboHits.Add(hit);
    }

    public int GetDamage(bool downedHit) {
        if (this.comboHits.Count == 0) {
            return 0;
        }
        ContactData lastHit = this.comboHits[this.comboHits.Count - 1];
        return downedHit ? (int) lastHit.DownedDamage : (int) lastHit.Damage;
    }

    public int GetHitstun(bool downedHit) {
        if (this.comboHits.Count == 0) {
            return 0;
        }
        ContactData lastHit = this.comboHits[this.comboHits.Count - 1];
        return downedHit ? lastHit.DownedHitstun : lastHit.Hitstun;
    }

    public float GetSelfEnergy() {
        if (this.comboHits.Count == 0) {
            return 0;
        }
        return this.comboHits[this.comboHits.Count - 1].GiveSelfPower;
    }

    public bool AddWallBounce() {
        if (wallBounce < maxWallBounce) {
            wallBounce++;
            return true;
        } else {
            return false;
        }
    }

    public bool AddGroundBounce() {
        if (groundBounce < maxGroundBounce) {
            groundBounce++;
            return true;
        } else {
            return false;
        }
    }
}
}