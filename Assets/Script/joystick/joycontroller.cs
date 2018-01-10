using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joycontroller: MonoBehaviour
{

    //private Vector3 startPos;
    //private Transform thisTransform;
    //private MeshRenderer mr;
    //public string[] buttonName = new string[1];
    //public GameObject player_ball;
    // public Transform targetball;


    //public GameObject player_cam;

    private float joySen = 0.5f;//敏感度
    private float tiltAroundOldz;


   public static bool joyjump=false;
    public static bool joypossessed = false;
    public static bool joyattack = false;
    public static bool leftpossessed = false;
    public static bool joyfast = false;
    


    // Update is called once per frame
    private void Update()
    {
       // RockerController();
        ButtonController();


        //joy5=btu_left_leftright 分1,-1 沒感壓
        //joy6=btu_left_updown    分1,-1 沒感壓

        //joy7=L2   =joystick button 6 =4th 分1,-1 沒壓-1  
        //joy8=R2   =joystick button 7 =5th 分1,-1 沒壓-1


        //joy9 =正方  =joystick button 0 沒感壓  分0,1
        //joy10=叉叉  =joystick button 1 沒感壓  分0,1
        //joy11=圓形  =joystick button 2 沒感壓  分0,1
        //joy12=三角  =joystick button 3 沒感壓  分0,1
        //joy13=中上那個鍵  =joystick button 13 沒感壓  分0,1

        //joy14=L1  =joystick button 4 感壓 0~1f
        //joy15=R1  =joystick button 5 感壓 0~1f

        //joy16=share  =joystick button 8 感壓 0~1f
        //joy17=options  =joystick button 9 感壓 0~1f
        //joy18=PS4那一顆  =joystick button 12 感壓 0~1f



    }
    //----------------------Rocker controller 搖桿
    public void RockerController()
    {
        /*float joyAbs1 = Mathf.Abs(Input.GetAxis("joy1"));//輸入值取正數
        float joyAbs2 = Mathf.Abs(Input.GetAxis("joy2"));//輸入值取正數

        if (joyAbs1 >= joySen || joyAbs2 >= joySen)
        {

            //左邊搖桿------------------------------------人物走動 joy1,joy2


            // player_ball.transform.position += inputDirection / 5;//球球走路
            //AccumulatePush("x", "joy1", 1);
            //AccumulatePush("z", "joy2", -1);

            Debug.Log("joy1");
            Debug.Log("joy2");


        }
         */
        //右邊搖桿------------------------------------視角旋轉 joy3,joy4
        float joyAbs3 = Mathf.Abs(Input.GetAxis("joy3"));//輸入值取正數
        float joyAbs4 = Mathf.Abs(Input.GetAxis("joy4"));//輸入值取正數

   
        if (joyAbs3 >= joySen || joyAbs4 >= joySen)//輸入值正數 > 敏感度
        {


            Debug.Log("joy3");
            Debug.Log("joy4");

/*
            float tiltAroundZ = -Input.GetAxis("joy3") * 0;
            float tiltAroundX = Input.GetAxis("joy4") * 60;
            Quaternion target = Quaternion.Euler(tiltAroundX, player_cam.transform.rotation.y, player_cam.transform.rotation.z);
            player_cam.transform.rotation = Quaternion.Slerp(player_cam.transform.rotation, target, Time.deltaTime * 3f);


            if (Input.GetAxis("joy3") > 0.5)
            {
                // plyer_cam.gameObject.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
                player_cam.gameObject.transform.Rotate(new Vector3(0f, 5f, 0f));
                //要轉localrotate
            }
            else if (Input.GetAxis("joy3") < 0.5)
            {
                player_cam.gameObject.transform.Rotate(new Vector3(0f, -5f, 0f));
            }
*/


        }
    }
    //-----------------------button controller
    public void ButtonController()
    {


        if (Input.GetAxis("joy5") == 1)
        {
           // 左邊按鈕 左
          

            Debug.Log("joy5");


        }
        if (Input.GetAxis("joy5") == -1)
        {
            //左邊按鈕 右
          

            Debug.Log("joy5");


        }

        if (Input.GetAxis("joy6") == 1)
        {
            //左邊按鈕 上
           // AccumulatePush("z", "joy6", 1);


            Debug.Log("joy6");
        }
        if (Input.GetAxis("joy6") == -1)
        {
            //左邊按鈕 下
            //AccumulatePush("z", "joy6", 1);

            Debug.Log("joy6");

        }
        //L2,R2---------------------

        if (Input.GetButtonUp("joy7"))
        {

            //L2
            // AccumulatePush("x", "joy7", 1);

            // player_cam.transform.rotation = Quaternion.Slerp(player_cam.transform.rotation, Quaternion.identity, Time.deltaTime * 10f);
            
            Debug.Log("joy7");
        }
        if (Input.GetButtonUp("joy8"))
        {

            //R2
            //AccumulatePush("x", "joy8", 1);
            

            Debug.Log("joy8");
        }
        //右邊四按鈕---------------------
        if (Input.GetButtonUp("Fire1") )
        {


            joyattack=true;
            //正方
            // AccumulatePush("z", "joy9", -1);

            //Debug.Log("Fire1");
        }
        else
        {

            joyattack = false;
        }
        if (Input.GetButtonUp("joy10") )
        {

            joyjump = true;
            //叉叉
            

            Debug.Log("joy10");
        }
        else { joyjump = false; }


        if (Input.GetButtonUp("joy11") )
        {

            //圓形
            

            Debug.Log("joy11");
        }
        if (Input.GetButtonUp("joy12") )
        {

            //三角
            

            Debug.Log("joy12");
        }



        //--------------------------------------------------------

        //其他---------------------
        if (Input.GetButtonUp("joy13") )
        {

            //長方形那塊
            // AccumulatePush("z", "joy13", -1);

            leftpossessed = true;

            Debug.Log("joy13");
        }
        else {
            leftpossessed = false;
        }
        if (Input.GetAxis("joy14") >= joySen)
        {

            //L1

            joyfast = true;

            Debug.Log("joy14");

        }
        else
        {
            joyfast = false;

        }
        if (Input.GetButton("joy15") )
        {

            //R1

            joypossessed = true;

            Debug.Log("joy15");
        }
        else { joypossessed = false; }

        if (Input.GetButtonUp("joy17"))
        {

            //OPTION
            // AccumulatePush("z", "joy17", -1);

            Debug.Log("joy17");
        }




        /*
        if (Input.GetAxis("joy16") >= joySen)
        {

            //SHARE
            //AccumulatePush("z", "joy16", -1);

            Debug.Log("joy16");
        }
        if (Input.GetAxis("joy17") >= joySen)
        {

            //OPTION
            // AccumulatePush("z", "joy17", -1);

            Debug.Log("joy17");
        }
        if (Input.GetAxis("joy18") >= joySen)
        {

            //PS4
            // AccumulatePush("z", "joy18", -1);

            Debug.Log("joy18");*/

    }
  /*  public bool joy15()
    {
        bool KeyDown = false;
        if (Input.GetAxis("joy15") >= 0.04f && joypossessed == false)
        {
            //R1
            joypossessed = true;
            KeyDown = true;

            //Debug.Log("joy15");
            
        }
        if (Input.GetAxis("joy15") < 0.04f && Input.GetAxis("joy15") >= -0.04f)
        {
            joypossessed = false;
        }
        return KeyDown;
    }*/

    void AccumulatePush(string xyz, string joybuttonNumber, int PosNeg)
    {

        Vector3 inputDirection = Vector3.zero;

        switch (xyz)
        {
            case "x":
                inputDirection.x = Input.GetAxis(joybuttonNumber) * PosNeg;
                break;
            case "y":
                inputDirection.y = Input.GetAxis(joybuttonNumber) * PosNeg;
                break;
            case "z":
                inputDirection.z = Input.GetAxis(joybuttonNumber) * PosNeg;
                break;
        }

        // thisTransform.position = startPos + inputDirection;//虛擬手把
        //player_ball.transform.position += inputDirection / 5;//球球走路

    }

}


