using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemy : Enemy
{
    public AbilityType abilityType;
    protected Ability specialAbility;
    [SerializeField] protected GameObject abilityPrefab;
    [SerializeField] protected float maxDuration;
    [SerializeField] protected float maxCooldown;
    [SerializeField] protected int abilityCount;
    [SerializeField] protected float abilityRange;
    [SerializeField] protected bool randomizeAbility = false;

    protected override void Start()
    {
        base.Start();
        if (randomizeAbility)
            abilityType = (AbilityType)UnityEngine.Random.Range(1, Enum.GetNames(typeof(AbilityType)).Length);
        initializeAbility();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        checkSpecialAbility();
    }

    private void initializeAbility()
    {
        switch (abilityType)
        {
            case AbilityType.DeadSpawn:
            case AbilityType.Spawn:
                specialAbility = gameObject.AddComponent(typeof(SpawnerAbility)) as SpawnerAbility;
                (specialAbility as SpawnerAbility).ConstructAbility(abilityType, abilityPrefab, abilityCount, maxDuration, maxCooldown);
                break;
            case AbilityType.Shield:
            case AbilityType.Overcharge:
            case AbilityType.Sprint:
                specialAbility = gameObject.AddComponent(typeof(StatusCasterAbility)) as StatusCasterAbility;
                (specialAbility as StatusCasterAbility).ConstructAbility(abilityType, abilityRange, maxDuration, maxCooldown);
                break;
            case AbilityType.Heal:
                specialAbility = gameObject.AddComponent(typeof(HealerAbility)) as HealerAbility;
                (specialAbility as HealerAbility).ConstructAbility(abilityType, abilityRange, maxDuration, maxCooldown);
                break;
            default:
                Debug.Log($"Ability not implemented! {abilityType}");
                break;
        }
        specialAbility.startAbility(); // to prevent instant use of ability on spawn
    }

    protected override void enemyDead()
    {
        if (enemyFinished)
        {
            HealthBar.lives -= damage;
            enemyFinished = false;
        }
        if (enemyHealth <= 0)
        {
            if (abilityType == AbilityType.DeadSpawn)
            {
                (specialAbility as SpawnerAbility).spawnEnemies(gameObject.transform.position, targetTile, pathID);
            }

            MoneyManager.main.addMoney(killReward);
        }

        Counter.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void checkSpecialAbility()
    {
        if (specialAbility.isReady())
        {
            useSpecialAbility();
            specialAbility.startAbility();
        }
        else
        {
            specialAbility.updateAbility(Time.deltaTime);
        }
    }

    public void useSpecialAbility()
    {
        switch (abilityType)
        {
            case AbilityType.Spawn:
                (specialAbility as SpawnerAbility).spawnEnemies(gameObject.transform.position, targetTile, pathID);
                break;
            case AbilityType.Shield:
            case AbilityType.Overcharge:
            case AbilityType.Sprint:
                (specialAbility as StatusCasterAbility).castStatus(gameObject.transform.position);
                break;
            case AbilityType.DeadSpawn:
                break; // handled in enemyDead()
            case AbilityType.Heal:
                (specialAbility as HealerAbility).castHealing(gameObject.transform.position);
                break;
            default:
                Debug.Log($"Ability Can't be used/not implemented! {abilityType}");
                break;
        }
    }
}
