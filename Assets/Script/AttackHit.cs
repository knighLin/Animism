using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour {

    private EnemyHealth enemyHealth;
    private TypeValue typeValue;

    void Awake()
    {
        typeValue = GameObject.FindWithTag("Player").GetComponent<TypeValue>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        if (other.tag == "Enemy")
        {
            enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth.currentHealth > 0)
            {//當Enemy的還有血量時
                var damage = typeValue.PlayerAtk * Random.Range(0.9f, 1.1f);
                damage = Mathf.Round(damage);

                enemyHealth.Hurt(damage);

            }
        }
    }


}
