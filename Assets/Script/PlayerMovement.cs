using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    //use other class
    private TypeValue value;
    // private AnimEvents animEvent;
    //方向位置
    private Transform m_Cam;//參考場景中地主相機位置
    private Vector3 m_CamForward;//相機當前面向的位置
    private Vector3 moveDirection;//移動位置
    [SerializeField] float m_MovingTurnSpeed = 360;//移動時轉向的速度
    [SerializeField] float m_StationaryTurnSpeed = 180;//站立時轉向速度
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;//重力乘數(落下速度)
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //特定於樣本資產中的字符，需要修改才能與他人合作
    [SerializeField] float m_AnimSpeedMultiplier = 1f;//動畫播放速度的乘數
    [SerializeField] float m_GroundCheckDistance = 0.2f;//地面距離檢查

    Rigidbody m_Rigidbody;
    public static Animator m_Animator;
    private float _Speed;//移動速度的乘數
    bool m_Jump;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;//地面距離檢查的起源值
    const float k_Half = 0.5f;
    float m_TurnAmount;//轉向值
    float m_ForwardAmount;//前進值
    Vector3 m_GroundNormal;//地面法向量
    //float m_CapsuleHeight;//膠囊高度
    //Vector3 m_CapsuleCenter;//膠囊中心
    //CapsuleCollider m_Capsule;


    void Awake()
    {
        value = GetComponent<TypeValue>();
        //animEvent = GameObject.FindWithTag("Human").GetComponent<AnimEvents>();
        //animEvent.Jump = Jump;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GameObject.FindWithTag("Human").GetComponent<Animator>();

        //m_Capsule = GameObject.FindWithTag("Human").GetComponent<CapsuleCollider>();
         //m_CapsuleHeight = m_Capsule.height;
         //m_CapsuleCenter = m_Capsule.center;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;//保存一下地面检查值 
    }
    //public void Jump(){
    //    m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, value.JumpPower, m_Rigidbody.velocity.z);//保存x、z轴速度，并给以y轴向上的速度  
    //}
    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }


    }
    private void Update()
    {
        //if (!m_Jump)//不在跳跃状态下，如果读到跳跃则更新变量  
        //{
        //    m_Jump = Input.GetKeyDown(KeyCode.Space);
        //}
    }

    //固定更新與物理同步調用
    private void FixedUpdate()
    {
        //讀取輸入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //計算移動方向傳遞給角色
        if (m_Cam != null)
        {
            //計算相機相對方向移動：
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            moveDirection = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            //在沒有主相機的情況下，我們使用世界相對的方向
            moveDirection = v * Vector3.forward + h * Vector3.right;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {//當按下shift，有跑步動作
            _Speed = value.RunSpeed;
        }
        else
        {
            _Speed = value.MoveSpeed;
        }

        //將所有參數傳遞給角色控制腳本
        PlayerMove(moveDirection, m_Jump);
        m_Jump = false;
    }

    // 移动！ 
    void PlayerMove(Vector3 move, bool jump)
    {
        // 将一个世界坐标的输入转换为本地相关的转向和前进速度，这需要考虑到角色头部的方向
        if (move.magnitude > 1f)
        {
            move.Normalize();//向量大于1，则变为单位向量
        }
        move = transform.InverseTransformDirection(move);//转换为本地坐标
        CheckGroundStatus();//判断当前地面的状态
        move = Vector3.ProjectOnPlane(move, m_GroundNormal); // 根据地面的法向量，产生一个对应平面的速度方向
        m_TurnAmount = Mathf.Atan2(move.x, move.z);//产生一个方位角，即与z轴的夹角，用于人物转向
        m_ForwardAmount = move.z;//人物前进的数值

        ApplyExtraTurnRotation();//应用附加转弯！//問題點

        // 控制和速度处理，在地上和空中是不一样的
        if (m_IsGrounded)
        {
            m_Rigidbody.velocity = transform.forward * move.z * _Speed;
            // 确定当前是否能跳  ：
            if (Input.GetKeyDown(KeyCode.Space))
            { // jump!
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, value.JumpPower, m_Rigidbody.velocity.z);//保存x、z轴速度，并给以y轴向上的速度  
                //m_CapsuleHeight = m_Animator.GetFloat("ColliderHeight");
                //m_Capsule.height = m_CapsuleHeight;
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
            }
        }
        else
        {//乘數增加重力：
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);
            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;//上升的时候不判断是否在地面上   
        }

        // 将输入和其他状态传递给动画组件  
        UpdateAnimator(move);

    }

    // 更新动画组件 
    void UpdateAnimator(Vector3 move)
    {
        //更新動畫參數
        m_Animator.SetFloat("Speed", m_Rigidbody.velocity.magnitude, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            //m_Animator.SetTrigger("Jump");
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // 计算哪只脚是在后面的，所以可以判断跳跃动画中哪只脚先离开地面  
        // 这里的代码依赖于特殊的跑步循环，假设某只脚会在未来的0到0.5秒内超越另一只脚  获取当前是在哪个脚，Repeat相当于取模
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // 这边的方法允许我们在inspector视图中调整动画的速率，他会因为根运动影响移动的速度  
        //if (m_IsGrounded && move.magnitude > 0)
        //{
        //    m_Animator.speed = m_AnimSpeedMultiplier;
        //}
        //else
        //{// 在空中的时候不用
        //    m_Animator.speed = 1;
        //}
    }


    // 应用附加转弯
    void ApplyExtraTurnRotation()
    {
        // 帮助角色快速转向，这是动画中根旋转的附加项
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);//問題點
        //根据移动的速度计算出当前转向的速度  
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    // 判断当前地面的状态 
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // 在场景中显示地面检查线，从脚上0.1米处往下射m_GroundCheckDistance的距离，预制体默认是0.3  
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif

        // 0.1f是從主角內部開始射線的小偏移
        //也很好地註意到，樣本資產中的變換位置是主角的基礎
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {//射到了，保存法向量，改变变量，将动画的applyRootMotion置为true，true的含义是应用骨骼节点的位移，就是说动画的运动会对实际角色坐标产生影响，用于精确的播放动画  
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            //m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            //m_Animator.applyRootMotion = false;
        }
    }

}


