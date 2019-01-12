using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    [Tooltip("De combien augmente le score quand on le passe")]
    public int ObstacleValue;


    bool hasBeenTriggered;
    float timeInTrigger;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0, (Mathf.Sin(Time.time*3)/75) * Time.deltaTime, 0);
	}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Carpe>() != null && !hasBeenTriggered)
        {
            collision.gameObject.GetComponent<Carpe>().Hurt();
            Destroy(this.GetComponent<Collider>());
            hasBeenTriggered = true;
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Carpe>() != null && !hasBeenTriggered)
        {
            timeInTrigger += Time.deltaTime;
            if (timeInTrigger > .25f)
            {
                other.gameObject.GetComponent<Carpe>().carpeManager.IncreaseScore(1);
                timeInTrigger = 0;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Carpe>() != null && !hasBeenTriggered)
        {
            other.gameObject.GetComponent<Carpe>().carpeManager.ObstaclePassed(ObstacleValue);
        }
    }
}
