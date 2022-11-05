using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemy : Enemy {
    public AbilityType abilityType;
    protected Ability specialAbility;
    [SerializeField] protected GameObject abilityPrefab;
    [SerializeField] protected float maxDuration;
    [SerializeField] protected float maxCooldown;
    [SerializeField] protected int abilityCount;
    [SerializeField] protected float abilityRange;

    protected override void Start() {
        base.Start();
        initializeAbility();
    }

    protected override void Update() {
        base.Update();
        checkSpecialAbility();
    }

    private void initializeAbility() {
        switch (abilityType) {
            case AbilityType.Spawn:
                specialAbility = gameObject.AddComponent(typeof(SpawnerAbility)) as SpawnerAbility;
                (specialAbility as SpawnerAbility).ConstructAbility(abilityPrefab, abilityCount, maxDuration, maxCooldown);
                break;
            case AbilityType.Shield:
            case AbilityType.Overcharge:
            case AbilityType.Sprint:
                specialAbility = gameObject.AddComponent(typeof(StatusCasterAbility)) as StatusCasterAbility;
                (specialAbility as StatusCasterAbility).ConstructAbility(abilityType, abilityRange, maxDuration, maxCooldown);
                break;
            default:
                Debug.Log($"Ability not implemented! {abilityType}");
                break;
        }
        specialAbility.startAbility(); // to prevent instant use of ability on spawn
    }

    private void checkSpecialAbility() {
        if (specialAbility.isReady()) {
            useSpecialAbility();
            specialAbility.startAbility();
            Debug.Log("Special Ability Used!");
        } else {
            specialAbility.updateAbility(Time.deltaTime);
        }
    }

    public void useSpecialAbility() {
        switch (abilityType) {
            case AbilityType.Spawn:
                (specialAbility as SpawnerAbility).spawnEnemies(gameObject.transform.position, targetTile);
                break;
            case AbilityType.Shield:
            case AbilityType.Overcharge:
            case AbilityType.Sprint:
                (specialAbility as StatusCasterAbility).castStatus(gameObject.transform.position);
                break;
            default:
                Debug.Log($"Ability not implemented! {abilityType}");
                break;
        }
    }
}