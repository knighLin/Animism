using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfGuardsAi : MonoBehaviour
{

    private EnemyHealth enemyHealth;
    private GameObject Target;
    private NavMeshAgent Nav;
    private Animator Anim;

    void Awake()
    {
        Target = GameObject.FindWithTag("Player");
        Nav = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        StartCoroutine("DistoryTime");
    }

    private void Update()
    {
        if (Target == null)
        {
            return;
        }
        Nav.SetDestination(Target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Find Enemy");
            enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth.currentHealth > 0)
            {//當Enemy的還有血量時

                transform.LookAt(other.transform);
                Anim.SetTrigger("Attack");//之後要改誠動畫
            }

        }
    }

    IEnumerator DistoryTime()//兩分鐘後消失
    {
        yield return new WaitForSeconds(120);
        Destroy(gameObject);
    }

}
