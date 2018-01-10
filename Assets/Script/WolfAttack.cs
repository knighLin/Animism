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

    int AttackRender()
    {
        int AttackCount = Random.Range(0, 3);
        return AttackCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (PossessedSystem.PossessedCol.enabled == false)
        {
            if ((Input.GetMouseButtonDown(0)|| Input.GetButtonDown("Fire1")) && PossessedSystem.AttachedBody == this.gameObject && PossessedSystem.OnPossessed == true)
            {
                audioSource.PlayOneShot(attack);
                Anim.SetTrigger("Attack");
                Anim.SetInteger("Render",AttackRender());
            }

            if ((Input.GetMouseButtonDown(1) || Input.GetButtonDown("joy12")) && PossessedSystem.WolfCount >= 1 && PossessedSystem.OnPossessed == true)
            {
                
                Vector3 MovePoint = new Vector3(Random.Range(this.transform.position.x - 2, this.transform.position.x + 2), this.transform.position.y, Random.Range(this.transform.position.z - 2, this.transform.position.x + 2));
                Vector3 MovePoint2 = new Vector3(Random.Range(this.transform.position.x - 2, this.transform.position.x + 2), this.transform.position.y, Random.Range(this.transform.position.z - 2, this.transform.position.x + 2));
                Instantiate(WolfGuards, MovePoint, Quaternion.identity);
                Instantiate(WolfGuards, MovePoint2, Quaternion.identity);
              
                PossessedSystem.WolfCount = 0;
            }
        }
    }

   


}
