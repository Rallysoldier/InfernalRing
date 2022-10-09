using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamRitual
{
    public class BlockEffect : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        float scale = 0.6f;
        float maxTime = 12;
        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.localScale = new Vector2(scale,scale);
        }

        float updateTicks;
        void Update()
        {
            updateTicks++;

            transform.localScale = new Vector2(scale*(1 + updateTicks/10), scale*(1 + updateTicks/10));

            if (updateTicks > maxTime/3) {
                spriteRenderer.color = new Color(1f,1f,1f, 1f - updateTicks/maxTime);
            }

            if (updateTicks > maxTime) {
                Destroy(this);
            }
        }
    }
}
