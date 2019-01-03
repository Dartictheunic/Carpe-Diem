using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    [Header("Variables à link")]
    public CarpeManager carpeManager;
    public Animator contourAnimator;

    int i;
    Animator anim;
    Text text;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        text = GetComponent<Text>();
        anim.speed = carpeManager.bpm / 60;
        contourAnimator.speed = carpeManager.bpm / 60;
        i = 4;
        text.text = i.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Note()
    {
        i--;
        if (i == 0)
        {
            text.text = "Go !";
            carpeManager.LaunchGame();
        }

        else if (i > 0)
        {
            text.text = i.ToString();
        }
    }
}
