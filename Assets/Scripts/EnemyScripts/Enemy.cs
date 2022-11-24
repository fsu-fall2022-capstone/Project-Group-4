﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxEnemyHealth;
    [SerializeField] protected float maxMovementSpeed;

    public float enemyHealth { get; private set; }
    public float movementSpeed { get; private set; }

    [SerializeField] protected int killReward;     //Money for killing enemy
    public static float damage = 1f;       //Damage enemy does when hitting the endTile
    public static int toMainMenu;

    protected List<Status> statuses = new List<Status>();
    public List<Status> Statuses { get { return statuses; } }
    protected bool allowDamage = true;     //Added to check if enemy can take damage
    protected bool overcharged = false;   //Added to check if enemy is overcharged

    protected float timeCheck;

    protected GameObject targetTile;
    protected int pathID = 0;
    [SerializeField] protected bool waitForTarget = true;
    protected bool enemyFinished = false;  //Added to check if enemy has crossed the finish

    private void Awake()
    {
        Counter.enemies.Add(gameObject);
    }

    protected virtual void Start()
    {
        initializeEnemy();
        killReward = (int)maxEnemyHealth;
        timeCheck = Time.time;
        toMainMenu = 0;
    }

    protected virtual void FixedUpdate()
    {
        if(!waitForTarget) {
            checkPosition();
            checkStatuses();
            moveEnemy();
        }
    }

    private void initializeEnemy()
    {
        if (!waitForTarget)
            targetTile = MapGenerator.spawnTiles[pathID];
        enemyHealth = maxEnemyHealth;
        movementSpeed = maxMovementSpeed;
    }

    public void initializeTarget(GameObject _targetTile)
    {
        targetTile = _targetTile;
        waitForTarget = false;
    }

    public void setPathID(int _pathID)
    {
        pathID = _pathID;
        Debug.Log("PathID: " + pathID);
    }

    public void takeDamage(float amount)
    {
        if (allowDamage)
            enemyHealth = enemyHealth - amount;

        if (enemyHealth <= 0)
            enemyDead();
    }

    public void healDamage(float amount)
    {
        enemyHealth = enemyHealth + amount;

        if (enemyHealth > maxEnemyHealth)
            enemyHealth = maxEnemyHealth;
    }

    public void overchargeHealth(float amount)
    {
        if (!overcharged) {
            enemyHealth = enemyHealth + amount;
            overcharged = true;
        }
    }

    public void changeMovementSpeed(float amount)
    {
        movementSpeed = amount;

        if (movementSpeed > maxMovementSpeed)
            movementSpeed = maxMovementSpeed;
    }

    public void overchargeSpeed(float amount)
    {
        if (!overcharged) {
            movementSpeed = movementSpeed + amount;
            overcharged = true;
        }
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
        if (enemyHealth <= 0)
            MoneyManager.main.addMoney(killReward);

        Counter.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void moveEnemy()
    {
        //Time.deltaTime is zero when new game is set so enemy speed is zero, need enemy speed to be positive
        if (Time.deltaTime == 0 || toMainMenu == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * 1f);
            toMainMenu = 0;
        }
        else 
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * Time.deltaTime);
        }
        
    }

    //Modified to check if an enemy has hit the endTile, then update player health
    public void checkPosition()
    {
        if (targetTile != null && targetTile != MapGenerator.endTile) {
            if (Vector3.Distance(transform.position, targetTile.transform.position) < 0.001f) {
                try {
                    int currIndex = MapGenerator.pathTiles[pathID].IndexOf(targetTile);

                    targetTile = MapGenerator.pathTiles[pathID][currIndex - 1];

                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = targetTile.GetComponent<SpriteRenderer>().sortingOrder;
                } catch {
                    Debug.Log($"Error: Enemy has no path {pathID} for spawnTile {MapGenerator.spawnTiles[pathID].transform.position}");
                    enemyDead();
                }

            }
        }
        else if (targetTile == MapGenerator.endTile) {
            enemyFinished = true;
            enemyDead();
        }
    }

    public static void resetForMainMenu()
    {
        toMainMenu = 1;
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
        if (statuses.Count > 0)
            applyStatus();
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
                    Debug.Log($"{name} is {status.statusType} for {status.duration} seconds");
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f) {
                        overcharged = false;
                        if(enemyHealth > maxEnemyHealth)
                            enemyHealth = maxEnemyHealth;
                        Debug.Log($"{name} is no longer {status.statusType}");
                    }
                    else
                        overchargeHealth(maxEnemyHealth * 0.25f);
                    break;
                case StatusType.Sprinting:
                    Debug.Log($"{name} is {status.statusType} for {status.duration} seconds");
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f) {
                        setToNormalSpeed();
                        overcharged = false;
                        Debug.Log($"{name} is no longer {status.statusType}");
                    }
                    else
                        overchargeSpeed(maxMovementSpeed * 0.25f);
                    break;
                case StatusType.Shielded:
                    Debug.Log($"{name} is {status.statusType} for {status.duration} seconds");
                    status.updateDuration(Time.time - timeCheck);
                    allowDamage = (status.duration <= 0f) ? true : false;
                    break;
            }
            timeCheck = Time.time;
        }
        statuses.RemoveAll(x => statusesToRemove.Contains(x));
        timeCheck = Time.time;
    }
}