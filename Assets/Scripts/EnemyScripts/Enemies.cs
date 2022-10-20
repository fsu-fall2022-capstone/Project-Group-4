﻿/*
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

public class Enemies : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private string status = null;
    
    private float statusDuration;
    public float timeCheck;

    private int killReward;     //Money for killing enemy
    public static float damage;       //Damage enemy does when hitting the endTile

    private GameObject targetTile;
    private bool enemyFinished = false;  //Added to check if enemy has crossed the finish

    private void Awake()
    {
        Counter.enemies.Add(gameObject);
    }

    private void Start()
    {
        initailizeEnemy();
        timeCheck = Time.time;
    }

    private void initailizeEnemy()
    {
        targetTile = MapGenerator.startTile;
    }

    public void takeDamage(float amount)
    {
        enemyHealth = enemyHealth - amount;

        if (enemyHealth <= 0)
        {
            enemyDead();
        }
    }

    //Modified to allow the updating of the health/lives bar 
    private void enemyDead()
    {
        if (enemyFinished)
        {
            damage = 1f;
            HealthBar.lives -= damage;
            enemyFinished = false;
        }

        Counter.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void moveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * Time.deltaTime);
    }

    //Modified to check if an enemy has hit the endTile, then update player health
    public void checkPosition()
    {
        if (targetTile != null && targetTile != MapGenerator.endTile)
        {
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if (distance < 0.001f)
            {
                int currIndex = MapGenerator.pathTiles.IndexOf(targetTile);

                targetTile = MapGenerator.pathTiles[currIndex - 1];
            }
        }
        else if (targetTile == MapGenerator.endTile)
        {
            enemyFinished = true;
            enemyDead();
        }
    }

    private void checkStatus()
    {
        if (status != null)
        {
            switch (status)
            {
                case "Frozen":
                    statusDuration -= Time.time - timeCheck;
                    if (statusDuration <= 0f)
                    {
                        movementSpeed = 0.5f;
                        statusDuration = 0f;
                        status = null;
                    }
                    else
                        movementSpeed = 0f;
                    break;
            }
            timeCheck = Time.time;
        }
    }
    
    public void setStatus(string Status, float duration)
    {
        status = Status;
        statusDuration = duration;
        timeCheck = Time.time;
    }

    private void Update()
    {
        checkStatus();
        checkPosition();
        moveEnemy();
        takeDamage(0);
    }
}