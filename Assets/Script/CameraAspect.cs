using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspect : MonoBehaviour {
    public float baseWidth = 1920;
    public float baseHeight = 1080;
    public float baseOrthographicSize = 5;

    void Awake()
    {

        float newOrthographicSize = (float)Screen.height / (float)Screen.width * this.baseWidth / this.baseHeight * this.baseOrthographicSize;
        Camera.main.orthographicSize = Mathf.Max(newOrthographicSize, this.baseOrthographicSize);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
