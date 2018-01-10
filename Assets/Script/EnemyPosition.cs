using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour {

    private float FowardBack, LeftRight;

   
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AttackPosition()
    {
        FowardBack = Vector3.Dot( transform.position, Camera.main.transform.forward);
        LeftRight = Vector3.Cross(transform.position, Camera.main.transform.forward).y;
        Debug.Log("F"+FowardBack);
        Debug.Log("L"+LeftRight);
        //if (FowardBack>0)
        //{
        //    if (LeftRight > 0)
        //    {

        //    }
        //    else (LeftRight < 0)
        //    {

        //    }
        //}
        //else (FowardBack < 0)
        //{
        //    if (LeftRight > 0)
        //    {

        //    }
        //    else (LeftRight < 0)
        //    {

        //    }
        //}

    }
}
