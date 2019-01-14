using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class VolumeSetter : MonoBehaviour {
    public float multiplier = 1f;
	// Use this for initialization
	void Start () {
        if (Gamemanager.gamemanager != null)
        GetComponent<AudioSource>().volume = (Gamemanager.gamemanager.volume * multiplier);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
