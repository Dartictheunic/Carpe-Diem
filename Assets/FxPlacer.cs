using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPlacer : MonoBehaviour {

    public Transform linkedCarpe;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(linkedCarpe.position.x, transform.position.y, transform.position.z);
	}
}
