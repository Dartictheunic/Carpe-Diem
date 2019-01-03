using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    [Tooltip("De combien augmente le score quand on le passe")]
    public int ObstacleValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Carpe>() != null)
        {
            collision.gameObject.GetComponent<Carpe>().Hurt();
            Destroy(this.GetComponent<Collider>());
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Carpe>() != null)
        {
            other.gameObject.GetComponent<Carpe>().carpeManager.IncreaseScore(ObstacleValue);
        }
    }
}
