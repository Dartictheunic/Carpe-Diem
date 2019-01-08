using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FantomLib;

public class BPM_Vibration : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#if !UNITY_EDITOR

    public void MakeVibrationOnBPM()
    {
        AndroidPlugin.StartVibrator(30);
       
    }
#endif

}
