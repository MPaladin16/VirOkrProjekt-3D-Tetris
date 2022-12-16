using TMPro;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        float min = Mathf.FloorToInt(currentTime / 60);
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", min, currentTime);
    }
}
