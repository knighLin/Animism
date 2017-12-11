using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttack : MonoBehaviour {

    private RaycastHit hit;//點擊的物件
    public LayerMask WeaponLayer;//可當武器的階層
    public Transform WeaponPoint;
    private bool HasWeapon = false;
    private Rigidbody ThrowWeapon;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;//最多填充時間

    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;   

	void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {

        Attack();
        GetWeapon();
		
	}

    void Attack()
    {
        if(HasWeapon)
        {
            if(Input.GetMouseButtonDown(0))
            {
                ThrowWeapon.transform.parent = null;
                ThrowWeapon.AddForce(transform.forward *500);
                //HasWeapon = false;
            }
            else if (Input.GetMouseButton(0) )
            {//按住而且還未發射過
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;//發射力道累加

            }
            else if (Input.GetMouseButtonUp(0) )
            {//放開而且還未發射過
                Throw();

            }
            else if (m_CurrentLaunchForce >= m_MaxLaunchForce )
            {//當發射力道大於最大值，且還未發射子彈，強制發射
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Throw();

            }
        }
    }

    void GetWeapon()
    {
        if (Input.GetMouseButtonDown(1) && PossessedSystem.AttachedBody == this.gameObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 10, WeaponLayer);
            if (hit.collider.CompareTag("Weapon"))
            {
                hit.transform.position = WeaponPoint.position;
                hit.transform.parent = WeaponPoint;
                ThrowWeapon = hit.rigidbody;
                //Debug.Log(ThrowWeapon);
                HasWeapon = true;
            }
        }
    }

    private void Throw()
    {
        // Instantiate and launch the shell.
        HasWeapon = false;//已丟

        ThrowWeapon.velocity = m_CurrentLaunchForce * WeaponPoint.forward;//剛體的速度 ＝ 力道＊子彈Ｚ軸的位置（砲口），力道越大發射越快

        m_CurrentLaunchForce = m_MinLaunchForce;//發射後回歸最小值
    }

}
