using BlackGardenStudios.HitboxStudioPro;
using UnityEngine;

namespace TeamRitual
{
    public class ClashEffect : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        float scale = 0.3f;
        float updateTicks;
        float maxTime;
        // Start is called before the first frame update
        void Start()
        {
            updateTicks = 0;
            maxTime = 15*10;
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.localScale = new Vector2(scale,scale);

            EffectSpawner.PlayHitEffect(
                20, transform.position, spriteRenderer.sortingOrder + 1, true
            );
        }

        void FixedUpdate()
        {
            updateTicks++;

            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0,1)*360);

            transform.localScale = new Vector2(scale*(1 + 10f*(updateTicks/maxTime)), scale*(1 + 10f*(updateTicks/maxTime)));

            spriteRenderer.color = new Color(1f,1f,1f, 1f - updateTicks/maxTime);

            if (updateTicks > maxTime) {
                Destroy(this);
            }
        }
    }
}
