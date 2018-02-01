using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSaveData : MonoBehaviour {

    public string SelectedData;
	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }
	
	// Update is called once per frame
	void Update () {

	}
    public void DestroyThis()
    {
        Destroy(transform.gameObject);	
    }
}
