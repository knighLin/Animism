using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : MonoBehaviour
{

    public Rigidbody WolfGuards;//召喚狼
    private Animator Anim;

    // Use this for initialization
    void Awake()
    {
        Anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PossessedSystem.PossessedCol.enabled)
        {
            if (Input.GetMouseButtonDown(0) && PossessedSystem.AttachedBody == this.gameObject)
            {
                Anim.SetTrigger("Attack");
            }

            if (Input.GetMouseButtonDown(1) && PossessedSystem.WolfCount >= 3)
            {
                Vector3 InstantiatePoint = new Vector3(UnityEngine.Random.Range(PossessedSystem.AttachedBody.transform.position.x - 1.5f, transform.position.x + 1.5f),
                                                       PossessedSystem.AttachedBody.transform.position.y,
                                                       UnityEngine.Random.Range(PossessedSystem.AttachedBody.transform.position.z - 1.5f, PossessedSystem.AttachedBody.transform.position.z + 1.5f));

                Instantiate(WolfGuards, InstantiatePoint, Quaternion.identity);

                Vector3 InstantiatePoint2 = new Vector3(UnityEngine.Random.Range(PossessedSystem.AttachedBody.transform.position.x - 1.5f, transform.position.x + 1.5f),
                                                       PossessedSystem.AttachedBody.transform.position.y,
                                                       UnityEngine.Random.Range(PossessedSystem.AttachedBody.transform.position.z - 1.5f, PossessedSystem.AttachedBody.transform.position.z + 1.5f));

                Instantiate(WolfGuards, InstantiatePoint2, Quaternion.identity);
                PossessedSystem.WolfCount = 0;
            }
        }


    }

}
