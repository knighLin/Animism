using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeletData : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DeletSaveData()
    {
            File.Delete(Application.persistentDataPath + @"\Save\" + this.name + ".sav");
    }
}
