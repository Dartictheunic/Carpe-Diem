using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class VolumeSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().volume = Gamemanager.gamemanager.volume;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
