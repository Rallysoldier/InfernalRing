using System;
using System.Collections.Generic;
using UnityEngine;
using BlackGardenStudios.HitboxStudioPro;
using TeamRitual.Input;
using TeamRitual.Character;

namespace TeamRitual.Core {
public class PlayerGameObj : MonoBehaviour, ICharacter
{
    [SerializeField]
    public string characterName;
    public SoundHandler soundHandler;
    
    public PlayerGameObj enemy;
    public CharacterStateMachine stateMachine;
    public InputHandler inputHandler;
    public int wins = 0;

    [SerializeField]
    protected SpritePalette m_ActivePalette;
    [SerializeField]
    protected SpritePaletteGroup m_PaletteGroup;
    public int paletteNumber;

    public bool paletteSelected;
    public bool modeSelected;

    protected bool LockFlip { get; set; }

    protected Transform m_Transform;
    public Transform Transform { get { return m_Transform; } }
    public Animator m_Animator;
    public Rigidbody2D m_RigidBody;
    public Collider2D m_Collider;

    public Rigidbody2D RigidBody { get { return m_RigidBody; } }
    protected SpriteRenderer m_Renderer;

    protected Color m_DefaultColor;

    private HitboxManager m_HitboxManager;

    void Awake() {
        soundHandler = new SoundHandler(GetComponents<AudioSource>());
        m_Transform = transform;
        m_Transform.localScale = new Vector2(4.0f,4.0f);
        m_Renderer = GetComponent<SpriteRenderer>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_HitboxManager = GetComponent<HitboxManager>();
        m_Collider = GetComponent<Collider2D>();
        m_DefaultColor = m_Renderer.color;
    }

    void Update() {
        this.inputHandler.releasedKeys.Clear();
        this.inputHandler.heldKeys.Clear();

        foreach (KeyValuePair<string,KeyCode> kvp in this.inputHandler.inputMapping) {
            if (UnityEngine.Input.GetKeyDown(kvp.Value)) {
                this.inputHandler.receiveInput(kvp.Key);
            }
            if (UnityEngine.Input.GetKeyUp(kvp.Value)) {
                this.inputHandler.addReleasedKey(kvp.Key);
            }
            if (UnityEngine.Input.GetKey(kvp.Value)) {
                this.inputHandler.addHeldKey(kvp.Key);
            }
        }

        if (GameController.Instance.gcStateMachine.currentState is GCStateFight && GameController.Instance.pause == 0) {
            this.stateMachine.ChangeStateOnInput();
        }

        string input = this.inputHandler.command;
        if (!this.paletteSelected) {
            if (input.EndsWith("F")) {
                this.inputHandler.command += ",";
                this.stateMachine.Flash(new Vector4(20f,20f,20f,1f),15);
                this.paletteForward(1);
            } else if (input.EndsWith("B")) {
                this.inputHandler.command += ",";
                this.stateMachine.Flash(new Vector4(20f,20f,20f,1f),15);
                this.paletteBack(1);
            } else if (input.EndsWith("L") || input.EndsWith("M") || input.EndsWith("H")) {
                GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1001), true);
                this.stateMachine.Flash(new Vector4(20f,20f,20f,1f),30);
                this.inputHandler.command += ",";
                this.paletteSelected = true;
            }
        } else if (!this.modeSelected) {
            if (input.EndsWith("F")) {
                this.inputHandler.command += ",";
                this.modeForward();
            } else if (input.EndsWith("B")) {
                this.inputHandler.command += ",";
                this.modeBack();
            } else if (input.EndsWith("L") || input.EndsWith("M") || input.EndsWith("H")) {
                GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1011), true);
                EffectSpawner.PlayHitEffect(
                    1011, new Vector2(this.stateMachine.PosX(),this.stateMachine.height/2f), m_Renderer.sortingOrder + 1, true,
                    GameController.Instance.GetRingColor(this.stateMachine.GetRingMode())
                );
                this.stateMachine.Flash(new Vector4(20f,1f,1f,1f),30);
                this.inputHandler.command += ",";
                this.modeSelected = true;
            }
        }
    }

    public void modeForward() {
        int modeNum = (int) this.stateMachine.GetRingMode() + 1;
        RingMode newMode = RingMode.FIRST;

        foreach (int mn in Enum.GetValues(typeof(RingMode))) {
            if (mn == modeNum) {
                newMode = (RingMode) mn;
                break;
            }
        }

        this.stateMachine.SetRingMode(newMode);
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1010), true);
        EffectSpawner.PlayHitEffect(
            1010, new Vector2(this.stateMachine.PosX(),this.stateMachine.height/2f), m_Renderer.sortingOrder - 1, true,
            GameController.Instance.GetRingColor(this.stateMachine.GetRingMode())
        );
    }

    public void modeBack() {
        int modeNum = (int) this.stateMachine.GetRingMode() - 1;
        RingMode newMode = RingMode.NINTH;

        foreach (int mn in Enum.GetValues(typeof(RingMode))) {
            if (mn == modeNum) {
                newMode = (RingMode) mn;
                break;
            }
        }

        this.stateMachine.SetRingMode(newMode);
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1010), true);
        EffectSpawner.PlayHitEffect(
            1010, new Vector2(this.stateMachine.PosX(),this.stateMachine.height/2f), m_Renderer.sortingOrder - 1, true,
            GameController.Instance.GetRingColor(this.stateMachine.GetRingMode())
        );
    }

    public void paletteForward(int amount) {
        int paletteNumber = (this.paletteNumber + amount)%m_PaletteGroup.Palettes.Length;
        if (paletteNumber == this.enemy.paletteNumber || paletteNumber == 0) {
            this.paletteNumber += 1;
            this.paletteForward(1);
            return;
        }
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1000), true);
        this.SetPalette(paletteNumber);
    }

    public void paletteBack(int amount) {
        int paletteNumber = this.paletteNumber - amount;
        if (paletteNumber <= 0) {
            paletteNumber = m_PaletteGroup.Palettes.Length - paletteNumber;
        }

        if (paletteNumber == this.enemy.paletteNumber) {
            this.paletteNumber = this.enemy.paletteNumber;
            this.paletteBack(1);
            return;
        }
        GameController.Instance.soundHandler.PlaySound(EffectSpawner.GetSoundEffect(1000), true);
        this.SetPalette(paletteNumber);
    }

    public void SetPalette(int paletteNumber) {
        m_ActivePalette = m_PaletteGroup.Palettes[paletteNumber-1];
        this.paletteNumber = paletteNumber;
        SetPalette(m_ActivePalette);
    }

    public void SetPalette(SpritePalette palette)
    {
        m_ActivePalette = palette;

        if (palette != null)
        {
            var block = new MaterialPropertyBlock();

            if (m_Renderer == null) m_Renderer = GetComponent<SpriteRenderer>();
            m_Renderer.GetPropertyBlock(block);
            block.SetTexture("_SwapTex", palette.Texture);
            m_Renderer.SetPropertyBlock(block);
        }
    }

    public int NextPaletteIndex(int exceptPal) {
        for (int p = 0; p < this.m_PaletteGroup.Palettes.Length; p++) {
            if (p+1 != exceptPal) {
                Debug.Log(p+1);
                return p+1;
            }
        }

        return 0;
    }

    public SpritePalette ActivePalette { get { return m_ActivePalette; } }
    public SpritePaletteGroup PaletteGroup { get { return m_PaletteGroup; } }

    public bool FlipX
    {
        get { return m_Renderer.flipX; }
        protected set { if(LockFlip == false) m_Renderer.flipX = value; }
    }

    public virtual void HitboxContact(ContactData data)
    {
        this.stateMachine.HitboxContact(data);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("StagePrefab")) {
            float nextPos = this.stateMachine.PosX() + this.stateMachine.VelX();
            float colX = (collision.bounds.max.x + collision.bounds.min.x)/2f < 1 ? collision.bounds.max.x - 1.5f : collision.bounds.min.x + 1.5f;

            if (Mathf.Sign(colX) == -1 && nextPos < colX && this.stateMachine.VelX() < 1
            || Mathf.Sign(colX) == 1 && nextPos > colX && this.stateMachine.VelX() > 1) {
                this.stateMachine.VelX(0);
            }
        } else if (collision.gameObject.GetComponent<PlayerGameObj>() != null) {
            CharacterStateMachine collidingSM = collision.gameObject.GetComponent<PlayerGameObj>().stateMachine;

            if (collidingSM.health > 0 && Mathf.Abs(this.stateMachine.VelX()) > Mathf.Abs(collidingSM.VelX()) &&
                Mathf.Sign(this.stateMachine.VelX()) == Mathf.Sign(collidingSM.PosX()-this.stateMachine.PosX())) {
                collidingSM.VelXDirect(this.stateMachine.VelX());
            }
        }
    }
}
}