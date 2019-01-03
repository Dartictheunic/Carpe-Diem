using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateScore(int score)
    {
        text.text = "Score: " + score.ToString();
    }

    public void UpdateMultiplier(int multiplier)
    {
        text.text = "x" + multiplier.ToString();
    }

    public void UpdateColor(Color newColor)
    {
        text.color = newColor;
    }
}
