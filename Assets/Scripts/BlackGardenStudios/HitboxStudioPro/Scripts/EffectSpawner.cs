using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlackGardenStudios.HitboxStudioPro
{
    public class EffectSpawner : MonoBehaviour
    {
        AttackFX[] m_Effects;
        SoundFX[] m_SoundEffects;
        Dictionary<int, AttackFX> m_EffectDict = new Dictionary<int, AttackFX>();
        Dictionary<int, SoundFX> m_SoundEffectDict = new Dictionary<int, SoundFX>();
        static private EffectSpawner instance
        {
            get
            {
                if (m_Instance == null)
                {
                    new GameObject("Effect Spawner", typeof(EffectSpawner));
                }

                return m_Instance;
            }
            set
            {
                m_Instance = value;
            }
        }

        static private EffectSpawner m_Instance;

        static public GameObject PlayHitEffect(int uid, Vector3 point, int order, bool flipx)
        {
            return instance._PlayHitEffect(uid, point, order, flipx, new Color32(255,255,255,1));
        }

        static public GameObject PlayHitEffect(int uid, Vector3 point, int order, bool flipx, Color32 color)
        {
            return instance._PlayHitEffect(uid, point, order, flipx, color);
        }

        public GameObject _PlayHitEffect(int uid, Vector3 point, int order, bool flipx, Color32 color)
        {
            AttackFX pool;

            if (m_EffectDict.TryGetValue(uid, out pool) && pool.Effects != null && pool.Effects.Length > 0)
            {
                GameObject effect = pool.Effects[Random.Range(0, pool.Effects.Length)];
                var go = Instantiate(effect, point, Quaternion.identity);
                var renderer = go.GetComponent<SpriteRenderer>();

                renderer.flipX = flipx;
                renderer.sortingOrder = order;
                renderer.color = color;
                StartCoroutine(DestroyEffect(go));
                return effect;
            }

            return null;
        }

        static public AudioClip GetSoundEffect(int uid)
        {
            return instance._GetSoundEffect(uid);
        }

        public AudioClip _GetSoundEffect(int uid)
        {
            SoundFX pool;

            if (m_SoundEffectDict.TryGetValue(uid, out pool) && pool.Effects != null && pool.Effects.Length > 0)
            {
                AudioClip effect = pool.Effects[Random.Range(0, pool.Effects.Length)];
                return effect;
            }
            return null;
        }

        private WaitForSeconds m_Wait = new WaitForSeconds(1f);

        private IEnumerator DestroyEffect(GameObject go)
        {
            yield return m_Wait;
            Destroy(go);
        }

        void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                m_Effects = Resources.LoadAll<AttackFX>("Effects/");
                m_SoundEffects = Resources.LoadAll<SoundFX>("Sounds/");

                for (int i = 0; i < m_Effects.Length; i++)
                    m_EffectDict.Add(m_Effects[i].uniqueID, m_Effects[i]);

                for (int i = 0; i < m_SoundEffects.Length; i++)
                    m_SoundEffectDict.Add(m_SoundEffects[i].uniqueID, m_SoundEffects[i]);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        static public AttackFX[] GetPools()
        {
            var list = Resources.LoadAll<AttackFX>("Effects/").ToList();
            list.Sort((AttackFX a, AttackFX b) => b.uniqueID - a.uniqueID);
            return list.ToArray();
        }

        static public SoundFX[] GetSoundPools()
        {
            var list = Resources.LoadAll<SoundFX>("Sounds/").ToList();
            list.Sort((SoundFX a, SoundFX b) => b.uniqueID - a.uniqueID);
            return list.ToArray();
        }

        void OnDestroy()
        {
            if (m_Instance == this)
            {
                m_Instance = null;
            }
        }
    }
}