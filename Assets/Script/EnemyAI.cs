using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    //使用其他腳本函數
    //private PlayerHealth playerHealth;
    private EnemyShoot enemyShoot;
    //狀態變數設定
    public enum EnemyState
    {
        Enemy_Idle = 0,
        Enemy_Walk,
        Enemy_Rotation,
        Enemy_Catch,
        Enemy_NormalAttack,
        Enemy_Shooting

    }
    EnemyState enemyState;
    private float EnemyThinkTime = 3f;//思考時間
    private float timer = 0;//變換狀態時間
    private int NewState;//思考後的新狀態
    private bool isThink;
    private NavMeshAgent nav;

    //判斷主角
    public Transform Target;
    private float EnemyToPlayerDis;//主角跟敵人的距離

    //巡邏範圍計算變數
    private Vector3 MovePoint;//要移動的位置
    private Vector3 OriginPoint;//敵人最初位置
    private float PatorlRadius = 15f;//巡邏半徑

    //動作宣告
    private Animator animator;
    private float animSpeed;

    

    void Awake()
    {
        enemyShoot = GetComponentInChildren<EnemyShoot>();
        //playerHealth = GameObject.Find ("Player").GetComponent<PlayerHealth> ();
        enemyState = EnemyState.Enemy_Idle;//敵人最初狀態
        isThink = true;
        nav = GetComponent<NavMeshAgent>();
        OriginPoint = transform.position;//敵人最初的位置
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        ThinkState();
        TargetInRange();
        if (nav.remainingDistance < nav.stoppingDistance) //如果移動位置小於停止位置，不跑步
         {
            
            nav.isStopped = true;
             // isThink = true;//走到巡邏點後再開始思考
         }
         else
         {
             //isThink = false;
            nav.isStopped = false;
             animSpeed = nav.desiredVelocity.sqrMagnitude;
            animator.SetFloat("Speed", animSpeed);
         }

    }

    void ThinkState()
    {
        if (isThink == true)
        {
            if (Time.time - timer > EnemyThinkTime)
            {
                NewState = Random.Range(0, 3);
                timer = Time.time;
                switch (NewState)
                {
                    case 0:
                        SetEnemyState(EnemyState.Enemy_Idle);
                        break;
                    case 1:
                        SetEnemyState(EnemyState.Enemy_Walk);
                        break;
                    case 2:
                        SetEnemyState(EnemyState.Enemy_Rotation);
                        break;
                }
            }

        }

    }

    Vector3 PatrolPoint()//巡邏半徑內，隨機選一個點
    {
        MovePoint = new Vector3(Random.Range(OriginPoint.x - PatorlRadius, OriginPoint.x + PatorlRadius), transform.position.y, Random.Range(OriginPoint.z - PatorlRadius, OriginPoint.z + PatorlRadius));
        // Debug.Log("NewPoint" + MovePoint);
        return MovePoint;

    }

    private void TargetInRange()
    {
        EnemyToPlayerDis = Vector3.Distance(transform.position, Target.position);//去判斷跟主角的範圍
        if (EnemyToPlayerDis <= 10 && PlayerHealth.isDead == false)
        {
            isThink = false;//Stop think
          
            if (EnemyToPlayerDis <= 8 && EnemyToPlayerDis >= 3)//小於8大於3的距離射擊
            {
                SetEnemyState(EnemyState.Enemy_Shooting);
            }
            else if (EnemyToPlayerDis <= 3)//距離小於3  普通攻擊
            {
                SetEnemyState(EnemyState.Enemy_NormalAttack);//距離在10內追擊主角
            }
            else
            {
                SetEnemyState(EnemyState.Enemy_Catch);
            }
        }
        else//當距離大於10，敵人重新思考
        {
            isThink = true;
           // Debug.Log("player out of range");
            if (enemyState == EnemyState.Enemy_Catch || enemyState == EnemyState.Enemy_Shooting || enemyState == EnemyState.Enemy_NormalAttack)
            {
                SetEnemyState(EnemyState.Enemy_Idle);
            }
        }
    }

    void SetEnemyState(EnemyState State)
    {
        enemyState = State;//如果不是就變成思考後的新狀態
        switch (State)
        {
            case EnemyState.Enemy_Idle:
                nav.isStopped = true;
                //Debug.Log("Idle");
                break;
            case EnemyState.Enemy_Rotation:
                //Debug.Log("Rotation");
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, ((int)Random.Range(1, 4) * 90), 0), Time.deltaTime * 8f);
                nav.isStopped = true;
                break;
            case EnemyState.Enemy_Walk:
               // Debug.Log("Walk");
                //isThink = false;
                nav.SetDestination(PatrolPoint());

                nav.isStopped = false;
                break;
            case EnemyState.Enemy_Catch:
                //Debug.Log("Catch");
                nav.isStopped = false;
                transform.LookAt(Target);
             
                nav.SetDestination(Target.position);
                break;
            case EnemyState.Enemy_NormalAttack:
               // Debug.Log("Attack");
                nav.isStopped = true;
                transform.LookAt(Target.position);
                //Start attack animation
                break;
            case EnemyState.Enemy_Shooting:
               // Debug.Log("Shoot");
                transform.LookAt(Target.position);
                //start shoot animation
                if (Time.time - timer > 1f)
                {
                    animator.SetTrigger("Shoot");
                    enemyShoot.Shooting(Target);
                    enemyShoot.DeleteBullet();
                    timer = Time.time;
                }

                break;
        }
    }
}
    /* void Update()
     {
         if (isThink == true)//每3秒變換一次狀態
         {
             if (Time.time - timer >= EnemyThinkTime)
             {
                 ThinkState = Random.Range(0, 3);
                 timer = Time.time;
                 ThinkChangeState();
             }

         }

         TargetInRange();
         //
         if (Vector3.Distance(transform.position, PatrolPoint()) <= nav.stoppingDistance) //如果移動位置小於停止位置，不跑步
         {
             //animSpeed = 0f;
             isThink = true;//走到巡邏點後再開始思考
             nav.isStopped = true;
         }
         else
         {
             Debug.Log(transform.position);
             Debug.Log(isThink);
             animSpeed = nav.desiredVelocity.sqrMagnitude;
         }
         animator.SetFloat("Speed", animSpeed);

     }

     Vector3 PatrolPoint()//巡邏半徑內，隨機選一個點
     {
         MovePoint = new Vector3(Random.Range(OriginPoint.x - PatorlRadius, OriginPoint.x + PatorlRadius), transform.position.y, Random.Range(OriginPoint.z - PatorlRadius, OriginPoint.z + PatorlRadius));
         Debug.Log("NewPoint" + MovePoint);
         return MovePoint;
     }

     void OnCollisionEnter(Collision other)
     {
         if (other.collider.tag != "Floor")
         {
             animSpeed = 0f;
             SetEnemyState(EnemyState.Enemy_Idle);
         }
     }

     void ThinkChangeState()
     {
         switch (ThinkState)
         {
             case 0:
                 SetEnemyState(EnemyState.Enemy_Idle);
                 break;
             case 1:
                 SetEnemyState(EnemyState.Enemy_Walk);
                 break;
             case 2:
                 SetEnemyState(EnemyState.Enemy_Rotation);
                 break;
         }

     }

     void TargetInRange()
     {

         if (playerHealth.currentHealth > 0)
         {
             EnemyToPlayerDis = Vector3.Distance(transform.position, Target.position);

             if (EnemyToPlayerDis <= 12)//敵人跟主角距離
             {
                 transform.LookAt(Target);
                 isThink = false;
                 if (EnemyToPlayerDis <= 9 && EnemyToPlayerDis >= 3)//小於9大於3的距離射擊
                 {
                     SetEnemyState(EnemyState.Enemy_Shooting);
                 }
                 else if (EnemyToPlayerDis <= 3)//距離小於3	普通攻擊
                 {
                     SetEnemyState(EnemyState.Enemy_NormalAttack);
                 }
                 else//距離在12內追擊主角
                 {
                     SetEnemyState(EnemyState.Enemy_Catch);
                 }
             }
             else//當距離大於12，敵人重新思考
             {
                 isThink = true;//主角不在範圍，敵人思考下一步動作
                 if (enemyState == EnemyState.Enemy_Catch || enemyState == EnemyState.Enemy_Shooting || enemyState == EnemyState.Enemy_NormalAttack)
                 {
                     SetEnemyState(EnemyState.Enemy_Idle);
                 }
             }
         }

     }


     void SetEnemyState(EnemyState State)
     {
         if (enemyState == State) //當目前狀態等於新狀態跳出
         {
             return;
         }
         enemyState = State;//如果不是就變成思考後的新狀態

         switch (enemyState)
         {
             case EnemyState.Enemy_Idle:
                 Debug.Log("Idle");
                 nav.isStopped = true;
                 break;

             case EnemyState.Enemy_Walk:
                 Debug.Log("Walk");
                 isThink = false;// 暫停思考
                 nav.SetDestination(PatrolPoint());
                 nav.stoppingDistance = 0;
                 nav.isStopped = false;

                 break;

             case EnemyState.Enemy_Rotation:
                 Debug.Log("Rotation");
                 transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, ((int)Random.Range(1, 4) * 90), 0), Time.deltaTime * 5);
                 nav.isStopped = true;
                 break;
             case EnemyState.Enemy_Catch:
                 Debug.Log("Catch");
                 nav.SetDestination(Target.position);
                 nav.stoppingDistance = 1;
                 //animSpeed = m_rigidbody.velocity.magnitude;
                 //animator.SetFloat ("Speed",animSpeed);

                 break;

             case EnemyState.Enemy_NormalAttack:
                 Debug.Log("Attack");
                 animator.SetTrigger("Attack");
                 break;

             case EnemyState.Enemy_Shooting:
                 Debug.Log("Shoot");
                 animator.SetTrigger("Shoot");
                 enemyShoot.shot(Target);
                 enemyShoot.deleteBullet();

                 break;
         }

     }*/



