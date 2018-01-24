using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
    
    public GameObject PlayerPrefab, WolfPrefab, EnemyPrefab;
    public GameObject Wolf;
    public List<int> AnimalState;
    public List<Vector3> AnimalVector3;
    public List<int> EnemyState;
    public List<Vector3> EnemyVector3;
    public string PlayerState;
    public Vector3 PlayerVector3;



    // Use this for initialization
    void Start () {

        SpawnAllObject();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public  void SpawnAllObject()
    {

        SaveData.Data D1 = (SaveData.Data)IOHelper.GetData("C:/Users/user/AppData/LocalLow/Animism/Soul/Save/GameData.sav", typeof(SaveData.Data));
        for (int A =0 ; A<=2; A++) //讀取動物數據
        {
            AnimalState.Add(D1.AnimalState[A]);//讀取動物狀態
            //Debug.Log(D1.AnimalState[A]);
            //Debug.Log("讀" + AnimalState[A]);
            AnimalVector3.Add(D1.AnimalVector3[A]);//讀取動物座標
            if (AnimalState[A]==1)//如果動物活著(AnimalState=1)才生成
                Instantiate(WolfPrefab, AnimalVector3[A], Quaternion.identity);
            if (AnimalState[A] == 2)//如果動物被附身(AnimalState=2)生成後掛在主角身上
            {
                Wolf = Instantiate(WolfPrefab, GameObject.Find("Player").transform.position, Quaternion.identity);
                Wolf.transform.parent = GameObject.Find("Player").transform;
            }
            //Debug.Log(D1.AnimalVector3[A]);
            //Debug.Log("讀" + AnimalVector3[A]);
        }

        for (int E = 0; E <= 2; E++) //讀取敵人數據
        {
            EnemyState.Add(D1.EnemyState[E]);
            EnemyVector3.Add(D1.EnemyVector3[E]);
        }
        
       /* PlayerState = D1.PlayerState;
        PlayerVector3 = D1.PlayerVector3;
        Debug.Log(PlayerState);
        Debug.Log(PlayerVector3);
        Instantiate(PlayerPrefab, PlayerVector3, Quaternion.identity);*/



    }

}
