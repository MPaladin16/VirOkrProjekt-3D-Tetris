using TMPro;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawners;

    public TextMeshProUGUI timeText;

    private float currentTime;
    private bool start;

    // Start is called before the first frame update
    void Start()
    {
        start = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            currentTime += Time.deltaTime;
            float min = Mathf.FloorToInt(currentTime / 60);
            timeText.text = "Time: " + string.Format("{0:00}:{1:00}", min, currentTime);
        }
    }

    public void StartGame()
    {
        currentTime = 0f;

        foreach(var spawner in spawners)
        {
            spawner.GetComponent<GenerateTetris>().startSpawning();
        }

        start = true;
        
    }
}
