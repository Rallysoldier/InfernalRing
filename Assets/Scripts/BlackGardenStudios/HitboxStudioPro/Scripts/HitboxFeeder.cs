using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class HitboxFeeder : MonoBehaviour
    {
        public ICharacter Owner { get; private set; }
        public BoxCollider2D Collider { get; private set; }
        private HitboxManager m_Manager;

        private int m_hits = 1;
        private float m_Damage = 1f;
        private int m_FXUID = 0;
        /**New fields**/
        private int m_hitpause;
        private int m_blockpause;
        private float m_chipDamage;
        private GuardType m_guardType;
        private AttackPriority m_attackPriority;
        private int m_hitstun;
        private int m_blockstun;
        private Vector2 m_hitGroundVelocity;
        private int m_hitGroundVelocityTime;
        private Vector2 m_hitAirVelocity;
        private int m_hitAirVelocityTime;
        private Vector2 m_blockGroundVelocity;
        private int m_blockGroundVelocityTime;
        private Vector2 m_blockAirVelocity;
        private int m_blockAirVelocityTime;
        private int m_frame;
        /****/
        private bool m_DidHit = false;

        public int Id { get; private set; }
        public HitboxType Type { get; private set; }

        void Awake()
        {
            Collider = GetComponent<BoxCollider2D>();
            m_Manager = GetComponentInParent<HitboxManager>();
            Owner = GetComponentInParent<ICharacter>();
            gameObject.tag = transform.parent.tag;

            Collider.enabled = false;
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            Collider = GetComponent<BoxCollider2D>();
            m_Manager = GetComponentInParent<HitboxManager>();
            Owner = GetComponentInParent<ICharacter>();
        }
#endif
        public void Feed(Vector2 boxSize, Vector2 boxOffset, int ID, HitboxType type,
            float damage, float chipDamage, GuardType guardType, AttackPriority attackPriority, int hitstun, int blockstun,
            Vector2 hitGroundVelocity, int hitGroundVelocityTime, Vector2 hitAirVelocity, int hitAirVelocityTime,
            Vector2 blockGroundVelocity, int blockHroundVelocityTime, Vector2 blockAirVelocity, int blockAirVelocityTime,
            int frame, int hitpause, int blockpause, int fxUID, int hits)
        {
            Type = type;
            m_Damage = damage;
            m_FXUID = fxUID;
            /**Feed new fields**/
            m_hitpause = hitpause;
            m_blockpause = blockpause;
            m_chipDamage = chipDamage;
            m_guardType = guardType;
            m_attackPriority = attackPriority;
            m_hitstun = hitstun;
            m_blockstun = blockstun;
            m_hitGroundVelocity = hitGroundVelocity;
            m_hitGroundVelocityTime = hitGroundVelocityTime;
            m_hitAirVelocity = hitAirVelocity;
            m_hitAirVelocityTime = hitAirVelocityTime;
            m_blockGroundVelocity = blockGroundVelocity;
            m_blockGroundVelocityTime = blockHroundVelocityTime;
            m_blockAirVelocity = blockAirVelocity;
            m_blockAirVelocityTime = blockAirVelocityTime;
            m_frame = frame;
            /****/
            Collider.size = boxSize;
            Collider.offset = boxOffset;
            Collider.isTrigger = true;
            Id = ID;
            m_DidHit = false;
            m_hits = hits;

            Collider.enabled = true;
        }

        public void UpdatePoiseDamage(float damage) {  }
        public void UpdateAttackDamage(float damage) { m_Damage = damage; }

        public void Disable()
        {
            if (Collider != null)
                Collider.enabled = false;
        }

        private bool ReportHit(int target)
        {
            return m_Manager.ReportHit(Id, target);
        }

        private bool PeekHit(int target)
        {
            return m_Manager.PeekReport(Id, target);
        }

        private HitboxFeeder GetFeederFromCollision(Collider2D collision)
        {
            var feeder = collision.GetComponent<HitboxFeeder>();

            if (feeder == null) return null;
            //if this hitbox already hit someone this frame they need to wait a frame.
            if (feeder.m_DidHit == true) return null;
            //Check if pair passes matrix
            var test = HitboxCollisionMatrix.TestPair(Type, feeder.Type);
            //Since both objects will perform a collision test, only invoke an event if we are receiving.
            if (test != HitboxCollisionMatrix.EVENT.RECV && test != HitboxCollisionMatrix.EVENT.BOTH) return null;

            return feeder;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var feeder = GetFeederFromCollision(collision);

            if (feeder != null)
                m_Manager.AddContact(this, feeder);
        }

        /// <summary>
        /// Solve a contact between this feeder and param feeder then fire any applicable events.
        /// </summary>
        public void HandleContact(HitboxFeeder feeder)
        {
            //Lets ask the manager if we should report this.
            if (feeder.ReportHit(m_Manager.UID) == false)
                //Hit wasn't reported, this animation must have already hit us in a previous frame.
                return;
            //Consume the other hurtboxes hit for this frame.
            feeder.m_DidHit = true;
            //Proceed to generate contact data and pass event to the owner.
            var collision = feeder.Collider;

            //Estimate approximately where the intersection took place.
            var contactPoint = Collider.bounds.ClosestPoint(collision.bounds.center);
            var startY = Mathf.Min(collision.bounds.center.y + collision.bounds.extents.y, Collider.bounds.center.y + (Collider.bounds.extents.y / 2f));
            var endY = Mathf.Max(collision.bounds.center.y - collision.bounds.extents.y, Collider.bounds.center.y - (Collider.bounds.extents.y / 2f));

            contactPoint.y = Mathf.Lerp(startY, endY, Random.Range(0f, 1f));

            //Calculate force, velocity, direction, and damage.
            Owner.HitboxContact(
                new ContactData
                {
                    MyHitbox = this,
                    TheirHitbox = feeder,
                    AttackHits = feeder.m_hits,
                    Damage = feeder.m_Damage,
                    Point = contactPoint,
                    /**Setting new fields**/
                    ChipDamage = feeder.m_chipDamage,
                    GuardType = feeder.m_guardType,
                    AttackPriority = feeder.m_attackPriority,
                    Hitpause = feeder.m_hitpause,
                    Blockpause = feeder.m_blockpause,
                    Hitstun = feeder.m_hitstun,
                    Blockstun = feeder.m_blockstun,
                    HitGroundVelocity = feeder.m_hitGroundVelocity,
                    HitGroundVelocityTime = feeder.m_hitGroundVelocityTime,
                    HitAirVelocity = feeder.m_hitAirVelocity,
                    HitAirVelocityTime = feeder.m_hitAirVelocityTime,
                    BlockGroundVelocity = feeder.m_blockGroundVelocity,
                    BlockGroundVelocityTime = feeder.m_blockGroundVelocityTime,
                    //BlockAirVelocity = feeder.m_blockAirVelocity,
                    //BlockAirVelocityTime = feeder.m_blockAirVelocityTime,
                    Frame = m_frame,
                    /****/
                    fxID = feeder.m_FXUID
                }
            );
        }
    }
}