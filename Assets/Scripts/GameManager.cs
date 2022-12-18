using TMPro;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{

    [Header("Score needed for each level")]
    public int lvl_2 = 300;
    public int lvl_3 = 800;
    public int lvl_4 = 2000;
    public int lvl_5 = 3000;

    [Header("Spawner settings")]
    [Tooltip("Spawn tetris piece every x seconds")]
    [SerializeField] float seconds;

    [Tooltip("Start spawning pieces after startOffset seconds")]
    [SerializeField] float startOffset;

    public static Action<bool> onGameStarted;
    public static Action<bool> onGameStopped;

    public GameObject[] spawners;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lvlText;

    private float currentTime;
    private bool start;

    private int score;
    private int level;

    private int nextLvl;

    private int[] levelTresholds;

    // Start is called before the first frame update
    void Start()
    {
        start = false;

        levelTresholds = new int[4];
        levelTresholds[0] = lvl_2;
        levelTresholds[1] = lvl_3;
        levelTresholds[2] = lvl_4;
        levelTresholds[3] = lvl_5;

        //called by button, uncomment for testing
        //StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            currentTime += Time.deltaTime;
            float min = Mathf.FloorToInt(currentTime / 60);
            float sec = Mathf.FloorToInt(currentTime % 60);
            timeText.text = "Time: " + string.Format("{0:00}:{1:00}", min, sec);

            scoreText.text = "Score: " + score;
            lvlText.text = "Level: " + level;

        }

    }

    public void StartGame()
    {
        currentTime = 0f;
        score = 0;

        level = 1;
        nextLvl = levelTresholds[level - 1];

        //pokreni spawnere
        spawners[0].GetComponent<GenerateTetris>().startSpawning(0, seconds);
        spawners[1].GetComponent<GenerateTetris>().startSpawning(startOffset, seconds);

        //pokreni timer
        start = true;
        onGameStarted?.Invoke(start);
    }

    //poziva se kada se brise red u tetris mrezi
    public void UpdateScore(int amount = 100)
    {
        //lvl multiplier
        amount *= level;
        score += amount;

        //lvl 5 == max lvl 
        if (level < 5 && score >= nextLvl)
        {
            level++;

            if(level < 5)
            {
                nextLvl = levelTresholds[level - 1];
            }

            //stop the spawners
            spawners[0].GetComponent<GenerateTetris>().stopSpawning();
            spawners[1].GetComponent<GenerateTetris>().stopSpawning();

            //reduce time gap by half -- ovo bumo prilagodili
            seconds /= 1.3f;
            startOffset /= 1.3f;

            //start the spawners again (wait for the previous piece to fall)
            spawners[0].GetComponent<GenerateTetris>().startSpawning(startOffset, seconds);
            spawners[1].GetComponent<GenerateTetris>().startSpawning(seconds, seconds);
        }
    }

    public void StopGame()
    {
        start = false;
        onGameStopped?.Invoke(start);

        spawners[0].GetComponent<GenerateTetris>().stopSpawning();
        spawners[1].GetComponent<GenerateTetris>().stopSpawning();
    }
}
