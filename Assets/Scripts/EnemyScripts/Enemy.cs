﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxEnemyHealth;
    [SerializeField] private float maxMovementSpeed;

    public float enemyHealth { get; private set; }
    public float movementSpeed { get; private set; }

    [SerializeField] private int killReward;     //Money for killing enemy
    public static float damage = 1f;       //Damage enemy does when hitting the endTile


    private List<Status> statuses = new List<Status>();
    public List<Status> Statuses { get { return statuses; } }

    private float timeCheck;

    private GameObject targetTile;
    private bool enemyFinished = false;  //Added to check if enemy has crossed the finish

    private void Awake()
    {
        Counter.enemies.Add(gameObject);
    }

    protected virtual void Start()
    {
        initializeEnemy();
        timeCheck = Time.time;
    }

    protected virtual void Update()
    {
        checkPosition();
        checkStatuses();
        moveEnemy();
    }

    private void initializeEnemy()
    {
        targetTile = MapGenerator.startTile;
        enemyHealth = maxEnemyHealth;
        movementSpeed = maxMovementSpeed;
    }

    public void takeDamage(float amount)
    {
        enemyHealth = enemyHealth - amount;

        if (enemyHealth <= 0) {
            enemyDead();
        }
    }

    public void healDamage(float amount)
    {
        enemyHealth = enemyHealth + amount;

        if (enemyHealth > maxEnemyHealth) {
            enemyHealth = maxEnemyHealth;
        }
    }

    public void overchargeHealth(float amount)
    {
        enemyHealth = enemyHealth + amount;
    }

    public void changeMovementSpeed(float amount)
    {
        movementSpeed = amount;

        if (movementSpeed > maxMovementSpeed) {
            movementSpeed = maxMovementSpeed;
        }
    }

    public void overchargeSpeed(float amount)
    {
        movementSpeed = movementSpeed + amount;
    }

    public void setToNormalSpeed()
    {
        movementSpeed = maxMovementSpeed;
    }

    //Modified to allow the updating of the health/lives bar 
    private void enemyDead()
    {
        if (enemyFinished) {
            HealthBar.lives -= damage;
            enemyFinished = false;
        }
        if (enemyHealth <= 0) {
            MoneyManager.main.addMoney(killReward);
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
        if (targetTile != null && targetTile != MapGenerator.endTile) {
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if (distance < 0.001f) {
                int currIndex = MapGenerator.pathTiles.IndexOf(targetTile);

                targetTile = MapGenerator.pathTiles[currIndex - 1];
            }
        }
        else if (targetTile == MapGenerator.endTile) {
            enemyFinished = true;
            enemyDead();
        }
    }

    // ***** STATUS FUNCTIONS *********************************************************************************************** 

    public void addStatus(Status status)
    {
        statuses.Add(status);
    }

    public void removeStatus(Status status)
    {
        statuses.Remove(status);
    }

    private void checkStatuses() {
        if (statuses.Count > 0) {
            timeCheck = Time.time;
            applyStatus();
        }
    }

    private void applyStatus() {
        List<Status> statusesToRemove = new List<Status>();
        foreach(Status status in statuses){
            switch(status.statusType){
                case StatusType.Frozen:
                case StatusType.Stunned:
                    Debug.Log($"{name} is {status.statusType} for {status.duration} seconds");
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f) {
                        setToNormalSpeed();
                        statusesToRemove.Add(status);
                        Debug.Log($"{name} is no longer {status.statusType}");
                    }
                    else
                        changeMovementSpeed(0f);
                    break;
                case StatusType.Electrocuted:
                case StatusType.Burning:

                    break;
                case StatusType.Overcharged:

                    break;
                case StatusType.Sprinting:

                    break;
            }
            timeCheck = Time.time;
        }
        statuses.RemoveAll(x => statusesToRemove.Contains(x));
        timeCheck = Time.time;
    }
}