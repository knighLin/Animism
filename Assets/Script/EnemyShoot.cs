using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public Rigidbody bullet;
	[SerializeField]
	private float force = 10;
    private Rigidbody newBullet;
   
    private AudioSource audioSource;
    public AudioClip GunBang;
    //public Transform Target;
    /*void Update()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
            Shooting (Target);
		}
	}*/


    void Awake()
    {
       
        audioSource = GetComponent<AudioSource>();
    }

    public void Shooting(Transform Target)
    {
        newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(GunBang);
        newBullet.AddForce((Target.position - transform.position).normalized * force);
        //newBullet.velocity = Vector3.forward * force;

    }


    public void DeleteBullet()
	{
		Destroy (newBullet.gameObject, 3f);
	}
}
