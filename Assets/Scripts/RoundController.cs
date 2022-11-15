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

    public GameObject basicEnemy;

    public float timeBtwWaves;
    public float timeBeforeRoundStarts;
    public float timeVar;

    public bool isRoundGoing;
    public bool isIntermission;
    public bool isStartOfRound;

    public int round;

    private void Start()
    {
        if (main == null) main = this;
        isRoundGoing = false;
        isIntermission = false;
        isStartOfRound = true;

        timeVar = 0f + timeBeforeRoundStarts;

        round = 1;
    }

    private void spawnEnemies()
    {
        StartCoroutine("ISpawnEnemies");
    }

    IEnumerator ISpawnEnemies()
    {
        for (int i = 0; i < round; i++)
        {
            for(int j = 0; j < MapGenerator.spawnTiles.Count; j++)
            {
                GameObject newEnemy = Instantiate(basicEnemy, MapGenerator.spawnTiles[j].transform.position, Quaternion.identity);
                Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                enemyScript.setPathID(j);
                enemyScript.initializeTarget(MapGenerator.spawnTiles[j]);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
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
}