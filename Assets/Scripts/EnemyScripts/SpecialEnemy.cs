using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemy : Enemy {
    public AbilityType abilityType;
    private Ability specialAbility;
    [SerializeField] private float maxDuration;
    [SerializeField] private float maxCooldown;

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
                (specialAbility as SpawnerAbility).ConstructAbility(gameObject, 3, maxDuration, maxCooldown);
                break;
            default:
                Debug.Log($"Ability not implemented! {abilityType}");
                break;
        }
    }

    private void checkSpecialAbility() {
        if (specialAbility.isReady()) {
            specialAbility.startAbility();
            useSpecialAbility();
            Debug.Log("Special Ability Used!");
        }
    }

    public void useSpecialAbility() {
        switch (abilityType) {
            case AbilityType.Spawn:
                (specialAbility as SpawnerAbility).spawnEnemies(gameObject.transform.position, targetTile);
                break;
            default:
                Debug.Log($"Ability not implemented! {abilityType}");
                break;
        }
    }
}