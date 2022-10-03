using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthEffect : MonoBehaviour
{
    public GameObject characterObj;
    float playerHealthValue;
    public Image HealthBar;
    Animator CharactAnim;

    [Space]
    [Range(0,100f)]
    public float playerHealth;


    [Space]
    public float BasicAttackDamage;
    public float CrouchAttackDamage;
    public float HeavyAttackDamage;
    public float SpecialAttackDamage;

    [Space]
    public float blockAttackDamage;

    bool isBlocking;

    [Space]
    public float BlockingDamageOffset;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100f;


    }

    // Update is called once per frame
    void Update()
    {

        HealthBar.fillAmount = playerHealth * .01f;;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        print(col.gameObject.tag);

        if (col.gameObject.tag == "Player")
        {
            if(col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                print("Do Nothing");

            }
            else if(col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Basic_Attack"))
            {
                print("Basic Attack");
                Basic_Attack();

            }
            else if(col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Crouch_Attack"))
            {
                print("Crouch Attack");
                Crouch_Attack();
            }
        }
    }
    void Basic_Attack()
    {
        float offset = !isBlocking ? 0 : BasicAttackDamage/BlockingDamageOffset; 
        playerHealth -= offset;
        // CharactAnim.SetTrigger("HitLight")
    }

    void Crouch_Attack()
    {
        float offset = !isBlocking ? 0 : CrouchAttackDamage/BlockingDamageOffset; 
        playerHealth -= offset;
        // CharactAnim.SetTrigger("HitCrouch")

    }

    void Medium_Attack()
    {

    }

    void Heavy_Attack()
    {

    }

    void SetBlocking()
    {
          isBlocking=!isBlocking;
        
    }
}
