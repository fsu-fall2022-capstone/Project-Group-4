/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names may have been changed and some parts of the code may
    have been modified to fit our game scheme, these sections will be marked with 
    comments. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public static RoundController main;

    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject bossPrefab;

    public float timeBtwWaves;
    public float timeBeforeRoundStarts;
    public float timeVar;

    public bool isRoundGoing;
    public bool isIntermission;
    public bool isStartOfRound;

    public int round;

    private byte minionValue = 1;
    private byte specialValue = 3;
    private byte tankValue = 5;
    private byte spawnerValue = 7;

    private void Start()
    {
        if (main == null) main = this;
        isRoundGoing = false;
        isIntermission = false;
        isStartOfRound = true;

        timeVar = 0f + timeBeforeRoundStarts;

        Debug.Log($"RoundController reports timescale as: {Time.timeScale}");
        if (Time.timeScale == 0) TimeHandler.StartGameTime();

        round = 1;
    }

    private void FixedUpdate()
    {
        if (isStartOfRound)
        {
            if (Time.time >= timeVar)
            {
                isStartOfRound = false;
                isRoundGoing = true;

                spawnEnemies();
                return;
            }
        }
        else if (isIntermission)
        {
            if (Time.time >= timeVar)
            {
                isIntermission = false;
                isRoundGoing = true;

                MapGenerator.main.randomExpand(); // this needs to be replaced by buttons
                spawnEnemies();
            }
        }
        else if (isRoundGoing)
        {
            if (!(Counter.enemies.Count > 0))
            {
                isIntermission = true;
                isRoundGoing = false;

                timeVar = Time.time + timeBtwWaves;
                round += 1;
            }
        }
    }

    private List<GameObject> getEnemySpawnOrder()
    {
        List<GameObject> enemies = new List<GameObject>();
        int points = round;
        byte minionCount = 0;

        if (points < 5)
        {
            for (int i = 0; i < points; i++)
            {
                enemies.Add(enemyPrefabs[0]);
            }
        }
        else
        {
            while (points > 0)
            {
                if (points >= spawnerValue && minionCount >= (3 + (byte)(round / 5)))
                {
                    GameObject enemy = enemyPrefabs[1];
                    enemies.Add(enemy);
                    points -= spawnerValue;
                    spawnerValue += 7;
                }
                else if (points >= tankValue && minionCount >= (3 + (byte)(round / 5)))
                {   // should be tank type
                    GameObject enemy = enemyPrefabs[2];
                    enemies.Add(enemy);
                    points -= tankValue;
                }
                else if (points >= specialValue && minionCount >= (3 + (byte)(round / 5)))
                {   // should be spawner type
                    GameObject enemy = enemyPrefabs[UnityEngine.Random.Range(3, enemyPrefabs.Length)];
                    enemies.Add(enemy);
                    points -= specialValue;
                }
                else if (points >= minionValue)
                {   // should be easiest enemy to beat
                    GameObject enemy = enemyPrefabs[0];
                    enemies.Add(enemy);
                    points -= minionValue;
                    minionCount++;
                }
            }
        }

        return enemies;
    }

    private void spawnEnemies()
    {
        StartCoroutine("ISpawnEnemies");
    }

    IEnumerator ISpawnEnemies()
    {
        List<GameObject> enemies = getEnemySpawnOrder();

        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject selectedEnemy = enemies[i];

            for (int j = 0; j < MapGenerator.spawnTiles.Count; j++)
            {
                GameObject newEnemy = Instantiate(selectedEnemy,
                    MapGenerator.spawnTiles[j].transform.position, Quaternion.identity);
                Enemy script = newEnemy.GetComponent<Enemy>();
                script.levelUpMaxHealth(round / 5);
                script.initializeEnemy(MapGenerator.spawnTiles[j], j);
            }
            yield return new WaitForSeconds(1f);
        }

        if (round % 10 == 0)
        {
            // time to spawn boss prefab
            GameObject boss = Instantiate(bossPrefab,
                MapGenerator.spawnTiles[0].transform.position, Quaternion.identity);
            Enemy bossEnemy = boss.GetComponent<Enemy>();
            bossEnemy.levelUpMaxHealth(round / 10);
            bossEnemy.initializeEnemy(MapGenerator.spawnTiles[0], 0);
        }
    }
}