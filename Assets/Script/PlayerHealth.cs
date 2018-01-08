
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {

    private PlayerMovement playerMovement;//角色的移動
   // private PossessedSystem possessedSystem;
	public float MaxHealth = 100; //最大HP
    [SerializeField]
	public static float currentHealth; //當前HP
	
	private Animator animator;
	private bool isDead;//是否死亡
    [SerializeField]
    private HPUI HP;


    //audio
    private AudioSource audioSource;
    public AudioClip hurt;

    void Awake()
	{
        playerMovement = GetComponent<PlayerMovement>();
       // possessedSystem = GetComponent<PossessedSystem>();
		currentHealth = MaxHealth;//開始時，當前ＨＰ回最大ＨＰ
        audioSource = GetComponent<AudioSource>();
        animator = GameObject.FindWithTag ("Human").GetComponent<Animator> ();
    }
    void Start()
    {
       // HP = GameObject.FindGameObjectWithTag("HP").GetComponent<HPUI>();
       // HP.SetHumanHP(currentHealth);
    }

    public void Hurt(float Amount)
	{
       if(PossessedSystem.OnPossessed)//如果附身，扣動物血量
        {
            PossessedSystem.AttachedBody.GetComponent<AnimalHealth>().currentHealth -= Amount;
        }
        else
        {
            currentHealth -= Amount;//扣血
            audioSource.PlayOneShot(hurt);
        }
		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
       
       // HP.SetHumanHP(currentHealth);

        animator.SetTrigger("Hurt");
        animator.SetInteger("Render",HurtRender());
	}

    int HurtRender()
    {
        int HurtCount = Random.Range(0, 2);
        return HurtCount;
    }

	void Death()
	{
		isDead = true;
		playerMovement.enabled = false;
        animator.SetBool("Die",isDead);
		Destroy(gameObject,4f);
	}

}
