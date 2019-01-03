using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public float updateInterval = 0.5f;
    public float accum = 0f;
    public float frames = 0f;
    public float timeleft;

    Text text;
    void Start()
    {
        timeleft = updateInterval;
        text = GetComponent<Text>();
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            if (accum / frames < 30)
            {
                text.color = Color.red;
            }

            if (accum / frames > 30 && accum / frames < 60)
            {
                text.color = Color.yellow;
            }

            if (accum / frames > 60)
            {
                text.color = Color.cyan;
            }

            text.text = (accum / frames).ToString("f1");
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}