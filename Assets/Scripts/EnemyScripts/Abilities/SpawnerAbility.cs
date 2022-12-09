using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAbility : Ability
{
    protected GameObject enemy;
    public byte count { get; private set; }
    public void ConstructAbility(AbilityType abilityType, GameObject enemy, int count, float duration, float cooldown)
    {
        base.ConstructAbility(abilityType, duration, cooldown);
        this.enemy = enemy;
        this.count = (count > 255) ? (byte)255 : (byte)count;
    }

    public void spawnEnemies(Vector3 position, GameObject target, int pathID)
    {
        Debug.Log($"Spawning Enemies! {count} will spawn at {position} to target {target.transform.position}");
        IEnumerator coroutine = IAbilitySpawnEnemies(position, target, pathID);
        StartCoroutine(coroutine);
    }

    private IEnumerator IAbilitySpawnEnemies(Vector3 position, GameObject target, int pathID)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().initializeEnemy(target, pathID);
            yield return new WaitForSeconds(0.3f);
        }
    }
}