using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private TypeValue typeValue;

    private Animator Anim;
    private AudioSource audioSource;
    public AudioClip attack;
    [SerializeField] private Rigidbody WolfGuards;//召喚狼
    [SerializeField] private Transform GuardPoint1;
    [SerializeField] private Transform GuardPoint2;

    // Use this for initialization
    void Awake()
    {
        Anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        typeValue = GameObject.FindWithTag("Player").GetComponent<TypeValue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PossessedSystem.PossessedCol.enabled == false)
        {
            if (Input.GetMouseButtonDown(0) && PossessedSystem.AttachedBody == this.gameObject)
            {
                audioSource.PlayOneShot(attack);
                Anim.SetTrigger("Attack");
            }

            if (Input.GetMouseButtonDown(1) && PossessedSystem.WolfCount >= 1)
            {
               
                Instantiate(WolfGuards, GuardPoint1.position, Quaternion.identity);
                Instantiate(WolfGuards, GuardPoint2.position, Quaternion.identity);
                PossessedSystem.WolfCount = 0;
            }
        }


    }
    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Enemy")
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
            // Debug.Log("Fuck");
            if (enemyHealth.currentHealth > 0)
            {//當Enemy的還有血量時
                var damage = typeValue.PlayerAtk * Random.Range(0.9f, 1.1f);
                damage = Mathf.Round(damage);
                enemyHealth.Hurt(damage);
            }
        }
    }

}
