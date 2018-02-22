using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class DataName : MonoBehaviour {
    public GameObject Delet;
    public FileInfo DataInfo;
    public Text TextName;
    public string WhitchData;
    // Use this for initialization
    void Start () {
        SetDataName();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetDataName()
    {
        WhitchData = transform.parent.name;
        if (File.Exists(Application.persistentDataPath + @"\Save\" + WhitchData + ".sav"))
        {
            TextName.fontSize = 41;
            DataInfo = new FileInfo(Application.persistentDataPath + @"\Save\" + WhitchData + ".sav");
            Debug.Log(DataInfo.LastWriteTime);
            TextName.text = DataInfo.LastWriteTime.ToString();
            if (Delet!=null)
            Delet.SetActive(true);
        }
        else
        {
            TextName.fontSize = 51;
            TextName.text = "空的存檔";
            if (Delet != null)
                Delet.SetActive(false);
        }
    }
}
