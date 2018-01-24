using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{
    private PossessedSystem PossessedSystem;
    private PlayerManager PlayerManager;
    public Data D = new Data();//創一個新的Data物件 用來儲存各種資料
    public string filename;


    public class Data
    {
        public List<int> AnimalState;
        public List<Vector3> AnimalVector3;
        public List<int> EnemyState;
        public List<Vector3> EnemyVector3;
        public string PlayerState;
        public Vector3 PlayerVector3;
    }

    void Start()
    {
        PossessedSystem = GameObject.Find("Player").GetComponent<PossessedSystem>();
        PlayerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //定义存档路径
            string dirpath = Application.persistentDataPath + "/Save";
            //创建存档文件夹
            IOHelper.CreateDirectory(dirpath);
            //定义存档文件路径
            filename = dirpath + "/GameData.sav";
            //儲存檔案
            SaveDataValue();

            //保存数据
            IOHelper.SetData(filename, D);
            Debug.Log("存檔路徑為"+ filename );
        }
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

    public void SaveDataValue()
    {

        D.AnimalState = new List<int>{ 1 , 1 , 1 } ;
        D.AnimalVector3 = new List<Vector3> { new Vector3(232,23,46), new Vector3(232, 23, 43), new Vector3(232, 23, 40) };
        D.EnemyState = new List<int> { 1, 1, 1 };
        D.EnemyVector3 = new List<Vector3> { new Vector3(226, 23, 46), new Vector3(226, 23, 43), new Vector3(226, 23, 40) };
        D.PlayerState = PlayerManager.NowType;
        D.PlayerVector3 = GameObject.Find("Player").transform.position;

    }


}