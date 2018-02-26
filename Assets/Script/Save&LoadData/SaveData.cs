using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{
    public GameObject[] Wolf, Enemy;
    private PossessedSystem PossessedSystem;
    public string filename;

    public class Data
    {
        public List<int> AnimalState;
        public List<Vector3> AnimalVector3;
        public List<Quaternion> AnimalQuaternion;
        public List<int> EnemyState;
        public List<Vector3> EnemyVector3;
        public List<Quaternion> EnemyQuaternion;
        public string PlayerState;
        public Vector3 PlayerVector3;
        public Quaternion PlayerQuaternion;
    }

    void Start()
    {
        PossessedSystem = GameObject.Find("Pine").GetComponent<PossessedSystem>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //读取数据
            Data D1 = (Data)IOHelper.GetData(filename, typeof(Data));

            Debug.Log(D1.AnimalState);
            Debug.Log(D1.AnimalVector3);
            Debug.Log(D1.EnemyState);
            Debug.Log(D1.EnemyVector3);
            Debug.Log(D1.PlayerState);
            Debug.Log(D1.PlayerVector3);

        }
    }

    public void SaveDataValue(string SaveDataName)//按鈕給字串
    {
        Data Save = new Data();
        //定义存档路径
        string dirpath = Application.persistentDataPath + @"\Save\";
        //创建存档文件夹
        IOHelper.CreateDirectory(dirpath);
        //定义存档文件路径
        filename = dirpath+ SaveDataName + ".sav";
        //儲存檔案
        if (GameObject.FindGameObjectsWithTag("WolfMaster")[0] != null)//抓現在所有狼
            Wolf = GameObject.FindGameObjectsWithTag("WolfMaster");
        if (GameObject.FindGameObjectsWithTag("Enemy1")[0] != null) //抓現在所有敵人
            Enemy = GameObject.FindGameObjectsWithTag("Enemy1");
        Save.AnimalState = new List<int> { };                     //創新的List用來存數值
        Save.AnimalVector3 = new List<Vector3> { };               //創新的List用來存數值
        Save.AnimalQuaternion = new List<Quaternion> { };         //創新的List用來存數值
        Save.EnemyState = new List<int> { };                      //創新的List用來存數值
        Save.EnemyVector3 = new List<Vector3> { };                //創新的List用來存數值
        Save.EnemyQuaternion = new List<Quaternion> { };          //創新的List用來存數值
        for (int A = 0; A < Wolf.Length; A++)                     //抓動物
        {
            Save.AnimalState.Add(1);                              //抓動物狀態
            Save.AnimalVector3.Add(Wolf[A].transform.position);   //抓動物座標
            Save.AnimalQuaternion.Add(Wolf[A].transform.rotation);//抓動物旋轉角度
            Debug.Log("保存了第" + (A + 1) + "隻狼," + "狀態為" + Save.AnimalState[A] + "座標為" + Save.AnimalVector3[A]);
        }
        for (int E = 0; E < Enemy.Length; E++)                    //抓敵人
        {
            Save.EnemyState.Add(1);                               //抓敵人狀態
            Save.EnemyVector3.Add(Enemy[E].transform.position);   //抓敵人座標
            Save.EnemyQuaternion.Add(Enemy[E].transform.rotation);//抓敵人旋轉角度
            Debug.Log("保存了第" + (E + 1) + "個敵人," + "狀態為" + Save.EnemyState[E] + "座標為" + Save.EnemyVector3[E]);
        }
        Save.PlayerState = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().NowType;
        Save.PlayerVector3 = GameObject.Find("Pine").transform.position;
        Save.PlayerQuaternion = GameObject.Find("Pine").transform.rotation;
        Debug.Log("保存了派恩的位置,座標為" + Save.PlayerVector3);
        //保存数据
        IOHelper.SetData(filename, Save);
        Debug.Log("保存了" + SaveDataName);
        Debug.Log("存檔路徑為" + filename);
        }

}
