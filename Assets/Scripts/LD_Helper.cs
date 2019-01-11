using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LD_Helper : MonoBehaviour {

    public Transform carpes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Left : " + carpes.position.z);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Right : " + carpes.position.z);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Center : " + carpes.position.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left Jump : " + carpes.position.z);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right Jump : " + carpes.position.z);
        }
    }
}
