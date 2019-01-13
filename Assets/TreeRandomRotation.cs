using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TreeRandomRotation : MonoBehaviour {

    private void Start()
    {
        transform.Rotate(new Vector3(Random.Range(0f, 360f),0 , 0));
    }

}
