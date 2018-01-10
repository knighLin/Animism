using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalAtk : MonoBehaviour
{
	//call other class
	private PlayerHealth PlayerHealth;

	private TypeValue value;

	//private GameObject gameManager;
	private GameObject gameManager;
	private GameObject Player;//Find Atk Target

	//Animator
	public Animator animator;
	public float timeBetweenAttacks = 1f;//敵人攻擊的時間間距
	public int EnemyAtk = 10;//敵人攻擊力
	//bool playerInRange;//主角是否在範圍裡
	//private float timer;//計數到下一次攻擊。
	public Collider weaponCollider;

	void Awake ()
	{
		//set class var
		Player = GameObject.Find ("Player");
		gameManager = GameObject.Find ("GameManager");
        PlayerHealth = Player.GetComponent <PlayerHealth> ();
		
		value = gameManager.GetComponent<TypeValue> ();

		//set Animator
		animator = GetComponent<Animator> ();

		//set WeaponCollider
		weaponCollider.enabled = false;
		Physics.IgnoreCollision (GetComponent<Collider>(),weaponCollider);//讓兩個物體不會產生碰撞
	}

	void OnTriggerEnter (Collider other)//當進入範圍的物件為主角///只後要做範圍的距離判斷，改變敵人攻擊方式
	{
		//animator.SetTrigger ("hit");//EnemyBeAtk
		Debug.Log ("EnemyDamage");

		if (other.gameObject == Player) {
			/*Debug.Log ("Player");
			//playerInRange = true;
			timer += Time.deltaTime;
			//Debug.Log (timer);
			if (timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0) {//主角進入範圍，敵人還有血量
				Debug.Log("AAA");
				Attack ();
				timer = 0;
			}*/
			Attack ();
			Debug.Log ("PlayerBeatk");
		}

	}


	public void Attack ()
	{
		if (PlayerHealth.currentHealth > 0) {//當主角的還有血量時
            
            var damage = (EnemyAtk - value.PlayerDef) * Random.Range(0.9f, 1.1f);
            damage = Mathf.Round(damage);
            PlayerHealth.Hurt (damage);//敵人的攻擊扣掉主角的防禦，然後＊隨機小數點，就是主角要被扣掉的血

            

        }

	}
	public void OnAttackTrigger()//避免走路時碰到武器，觸發事件，所以只有攻擊時，才開啟觸發
	{
		weaponCollider.enabled = true;
	}

	public void OnDisableAttackTrigger()
	{
		weaponCollider.enabled = false;

}




}