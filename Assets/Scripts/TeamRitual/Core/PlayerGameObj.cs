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
    
    public CharacterStateMachine stateMachine;
    public InputHandler inputHandler = new InputHandler();
    public int wins = 0;

    [SerializeField]
    protected SpritePalette m_ActivePalette;
    [SerializeField]
    protected SpritePaletteGroup m_PaletteGroup;

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
        soundHandler = new SoundHandler(GetComponent<AudioSource>());
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
    }

    public void SetPalette(int paletteNumber) {
        m_ActivePalette = m_PaletteGroup.Palettes[paletteNumber-1];
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

            if (Mathf.Abs(this.stateMachine.VelX()) > Mathf.Abs(collidingSM.VelX()) &&
                Mathf.Sign(this.stateMachine.VelX()) == Mathf.Sign(collidingSM.PosX()-this.stateMachine.PosX())) {
                collidingSM.VelXDirect(this.stateMachine.VelX());
            }
        }
    }
}
}