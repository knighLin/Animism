
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {
    
    private HPcontroller HPcontroller;
    private PlayerMovement playerMovement;//角色的移動
    private RagdollBehavior ragdollBehavior;
   // private PossessedSystem possessedSystem;
	public float MaxHealth = 100; //最大HP
	public  float currentHealth; //當前HP
    private Animator animator;
	public static bool isDead;//是否死亡

    private float timer;//開啟ragdoll的時間
    public CapsuleCollider m_collider;

    //audio
    private AudioSource audioSource;
    public AudioClip hurt;

    void Awake()
	{

        playerMovement = GetComponent<PlayerMovement>();
        ragdollBehavior = GetComponent<RagdollBehavior>();
        // possessedSystem = GetComponent<PossessedSystem>();
        currentHealth = MaxHealth;//開始時，當前ＨＰ回最大ＨＰ
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator> ();

    }
    void Start()
    {
       
       // HPcontroller = GameObject.Find("GameManager").GetComponent<HPcontroller>();
    }

    void Update()
    {
        if (currentHealth <= 0 && isDead == false)
        {
            Death();
        }

    }

    public void Hurt(float Amount)
	{
        //if (PossessedSystem.OnPossessed)//如果附身，扣動物血量
        //{
        //    PossessedSystem.AttachedBody.GetComponent<AnimalHealth>().currentHealth -= Amount;
        //    HPcontroller.Blink = true;
        //    HPcontroller.CharacterHpControll();
        //}
        //else
        //{

            currentHealth -= Amount;//扣血
        StartCoroutine(HurtAnimation());
           // HPcontroller.CharacterHpControll();
            //HPcontroller.Blink = true;
            audioSource.PlayOneShot(hurt);

        //      }
        

        //animator.SetTrigger("Hurt");
        //animator.SetInteger("Render",HurtRender());
    }

    

    IEnumerator HurtAnimation()
    {
        animator.enabled = false;
        ragdollBehavior.ToggleRagdoll(true);
        yield return new WaitForSeconds(0.5f);
        animator.enabled = true;
        ragdollBehavior.ToggleRagdoll(false);
        StopCoroutine(HurtAnimation());
    }
    void Death()
	{
       // m_collider.enabled = false;
        ragdollBehavior.ToggleRagdoll(true);
        animator.enabled = false;
        playerMovement.enabled = false;
      //  enabled = false;
        isDead = true;
        //animator.SetBool("Die",isDead);
		//Destroy(gameObject,4f);
	}

}
