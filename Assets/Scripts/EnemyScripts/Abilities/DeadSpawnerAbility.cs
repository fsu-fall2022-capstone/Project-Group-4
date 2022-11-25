using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSpawnerAbility : Ability {
    protected GameObject enemy;
    public int count { get; private set; }
    public void ConstructAbility(GameObject enemy, int count, float duration, float cooldown) {
        base.ConstructAbility(AbilityType.Spawn, duration, cooldown);
        this.enemy = enemy;
        this.count = count;
    }

    public void spawnEnemies(Vector3 position, GameObject target) {
        
        Debug.Log($"Spawning Enemies! {count} will spawn at {position} to target {target.transform.position}" );
        IEnumerator coroutine = IAbilitySpawnEnemies(position, target);
        StartCoroutine(coroutine);
        
    }

    private IEnumerator IAbilitySpawnEnemies(Vector3 position, GameObject target) {
        for (int i = 0; i < count; i++) {
            GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().initializeTarget(target);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
