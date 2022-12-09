using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxEnemyHealth;
    [SerializeField] protected float maxMovementSpeed;

    [field: SerializeField] public float enemyHealth { get; private set; }
    [field: SerializeField] public float movementSpeed { get; private set; }

    [SerializeField] protected int killReward;     //Money for killing enemy
    public static float damage = 1f;       //Damage enemy does when hitting the endTile
    public static int toMainMenu;

    protected List<Status> statuses = new List<Status>();
    public List<Status> Statuses { get { return statuses; } }
    protected bool allowDamage = true;     //Added to check if enemy can take damage
    protected bool overcharged = false;   //Added to check if enemy is overcharged
    protected List<BoonType> boons = new List<BoonType>();
    [SerializeField] protected GameObject lightningBolt;

    protected float timeCheck;

    protected GameObject targetTile;
    protected int pathID = 0;
    protected bool waitForTarget = true;
    protected bool enemyFinished = false;  //Added to check if enemy has crossed the finish

    public Vector2 direction;
    public Vector3 currScale;

    private void Awake()
    {
        Counter.enemies.Add(gameObject);
    }

    protected virtual void Start()
    {
        enemyHealth = maxEnemyHealth;
        movementSpeed = maxMovementSpeed;
        timeCheck = Time.time;
        toMainMenu = 0;
    }

    protected virtual void FixedUpdate()
    {
        if (!waitForTarget)
        {
            StartCoroutine(EnemyLogic());
        }
    }

    protected IEnumerator EnemyLogic()
    {
        checkPosition();
        StartCoroutine(checkStatuses());
        moveEnemy();

        currScale = gameObject.transform.localScale;
        if (direction.x < 0)
        {
            if (currScale.y > 0)
                currScale.y *= -1;
            gameObject.transform.localScale = currScale;
        }
        else
        {
            if (currScale.y < 0)
                currScale.y *= -1;
            gameObject.transform.localScale = currScale;
        }

        yield return null;
    }

    public void initializeEnemy(GameObject _targetTile, int _pathID)
    {
        targetTile = _targetTile;
        pathID = _pathID;
        waitForTarget = false;
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

    public void levelUpMaxHealth(float amount)
    {
        maxEnemyHealth += ((maxEnemyHealth / 10) * amount);
    }

    public void overchargeHealth(float amount)
    {
        if (!overcharged)
        {
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
        if (!overcharged)
        {
            movementSpeed = movementSpeed + amount;
            overcharged = true;
        }
    }

    public void setToNormalSpeed()
    {
        movementSpeed = maxMovementSpeed;
    }

    public float getMaxHealth()
    {
        return maxEnemyHealth;
    }

    public float getMaxSpeed()
    {
        return maxMovementSpeed;
    }

    //Modified to allow the updating of the health/lives bar 
    protected virtual void enemyDead()
    {
        if (enemyFinished)
        {
            HealthBar.lives -= damage;
            enemyFinished = false;
        }
        if (enemyHealth <= 0)
            MoneyManager.main.addMoney(killReward);

        Counter.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    protected void moveEnemy()
    {
        //Time.deltaTime is zero when new game is set so enemy speed is zero, need enemy speed to be positive
        if (Time.deltaTime == 0 || toMainMenu == 1)
        {
            //Determine the movement direction and flip enemy sprite to face that way - Nathan Granger
            direction = targetTile.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (movementSpeed * 25) * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * 1f);
            toMainMenu = 0;
        }
        else
        {
            //Determine the movement direction and flip enemy sprite to face that way - Nathan Granger
            direction = targetTile.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (movementSpeed * 25) * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * Time.deltaTime);
        }

    }

    //Modified to check if an enemy has hit the endTile, then update player health
    public void checkPosition()
    {
        if (targetTile != null && targetTile != MapGenerator.endTile)
        {
            if (Vector3.Distance(transform.position, targetTile.transform.position) < 0.001f)
            {
                try
                {
                    int currIndex = MapGenerator.pathTiles[pathID].IndexOf(targetTile);

                    targetTile = MapGenerator.pathTiles[pathID][currIndex - 1];

                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = targetTile.GetComponent<SpriteRenderer>().sortingOrder;
                }
                catch
                {
                    Debug.Log($"Error: Enemy has no path {pathID} for spawnTile {MapGenerator.spawnTiles[pathID].transform.position}");
                    enemyDead();
                }

            }
        }
        else if (targetTile == MapGenerator.endTile)
        {
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

    private IEnumerator checkStatuses()
    {
        if (statuses.Count > 0)
            applyStatus();
        yield return null;
    }

    protected void applyStatus()
    {
        List<Status> statusesToRemove = new List<Status>();
        foreach (Status status in statuses)
        {
            switch (status.statusType)
            {
                case StatusType.Frozen:
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f)
                    {
                        setToNormalSpeed();
                        statusesToRemove.Add(status);
                    }
                    else
                        changeMovementSpeed(maxMovementSpeed / 2);
                    break;
                case StatusType.Stunned:
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f)
                    {
                        setToNormalSpeed();
                        statusesToRemove.Add(status);
                    }
                    else
                        changeMovementSpeed(0);
                    break;
                case StatusType.Electrocuted:
                    takeDamage(status.damage);
                    foreach (GameObject enemy in Counter.enemies)
                    {
                        if (enemy.GetComponent<Enemy>() != this && enemy != null)
                        {
                            float distance = (transform.position - enemy.transform.position).magnitude;
                            // status.duration determines range of chaining
                            if (distance < 1)
                            {
                                enemy.GetComponent<Enemy>().addStatus(new Status(StatusType.Electrocuted, status.duration, status.damage / 2));
                            }
                        }
                    }
                    statusesToRemove.Add(status);
                    break;
                case StatusType.Burning:
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f)
                    {
                        statusesToRemove.Add(status);
                    }
                    else
                        Debug.Log("Burn Damage Applied");
                    takeDamage(status.damage);
                    break;
                case StatusType.Overcharged:
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f)
                    {
                        overcharged = false;
                        if (enemyHealth > maxEnemyHealth)
                            enemyHealth = maxEnemyHealth;
                    }
                    else
                        overchargeHealth(enemyHealth * 0.25f);
                    break;
                case StatusType.Sprinting:
                    status.updateDuration(Time.time - timeCheck);
                    if (status.duration <= 0f)
                    {
                        setToNormalSpeed();
                        overcharged = false;
                    }
                    else
                        overchargeSpeed(maxMovementSpeed * 0.25f);
                    break;
                case StatusType.Shielded:
                    status.updateDuration(Time.time - timeCheck);
                    allowDamage = (status.duration <= 0f) ? true : false;
                    break;
            }
            timeCheck = Time.time;
        }
        statuses.RemoveAll(x => statusesToRemove.Contains(x));
        timeCheck = Time.time;
    }

    // ***** BOON FUNCTIONS *********************************************************************************************** 

    public void addBoon(BoonType boon)
    {
        boons.Add(boon);
        switch (boon)
        {
            case BoonType.Fortune:
                killReward += 15;
                break;
        }
    }

    public void removeBoon(BoonType boon)
    {
        switch (boon)
        {
            case BoonType.Fortune:
                killReward -= 15;
                break;
        }
        boons.Remove(boon);
    }
}
