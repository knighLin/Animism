using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfGuardsAttack : MonoBehaviour {

    private EnemyHealth enemyHealth;
    private float Atk = 10;


    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")
        {
            enemyHealth = other.transform.GetComponent<EnemyHealth>();
            var damage = Atk * Random.Range(0.9f, 1.1f);
            damage = Mathf.Round(damage);
            enemyHealth.Hurt(damage);
        }

    }
}
