using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {
    public AbilityType abilityType { get; private set; }
    public float cooldown { get; private set; }
    public float duration { get; private set; }
    protected float maxDuration;
    protected float maxCooldown;

    protected virtual void ConstructAbility(AbilityType abilityType, float duration, float cooldown) {
        this.abilityType = abilityType;
        this.maxDuration = duration;
        this.maxCooldown = cooldown;
        this.duration = 0;
        this.cooldown = 0;
    }

    public void startAbility() {
        duration = maxDuration;
        cooldown = maxCooldown;
    }

    public void updateAbility(float time) {
        if (duration > 0) {
            duration -= time;
        } else if (cooldown > 0) {
            cooldown -= time;
        }
    }

    public bool isRunning() {
        return duration > 0;
    }

    public bool isReady() {
        return cooldown <= 0;
    }

    // command functions to directly change the timers
    public void updateCooldown(float time) {
        cooldown -= time;
    }

    public void updateDuration(float time) {
        duration -= time;
    }
}

public class SpawnerAbility : Ability {
    protected GameObject enemy;
    public int count { get; private set; }
    public void ConstructAbility(GameObject enemy, int count, float duration, float cooldown) {
        base.ConstructAbility(AbilityType.Spawn, duration, cooldown);
        this.enemy = enemy;
        this.count = count;
    }

    public void spawnEnemies(Vector3 position, GameObject target) {
        if(isReady()) {
            Debug.Log($"Spawning Enemies! {count} will spawn at {position} to target {target.transform.position}" );
            IEnumerator coroutine = IAbilitySpawnEnemies(position, target);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator IAbilitySpawnEnemies(Vector3 position, GameObject target) {
        for (int i = 0; i < count; i++) {
            GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().initializeTarget(target);
            yield return new WaitForSeconds(0.1f);
        }
    }
}