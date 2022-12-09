using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAbility : Ability
{
    protected float range;
    public void ConstructAbility(AbilityType abilityType, float range, float duration, float cooldown)
    {
        base.ConstructAbility(abilityType, duration, cooldown);
        this.range = range;
    }

    public void castHealing(Vector3 position)
    {
        Debug.Log($"Casting Healing! It will be cast at {position} with range {range}");
        IEnumerator coroutine = IAbilityCastHealing(position);
        StartCoroutine(coroutine);
    }

    private IEnumerator IAbilityCastHealing(Vector3 position)
    {
        List<GameObject> enemiesInRange = getEnemiesInRange(position, range);
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                Enemy script = enemy.GetComponent<Enemy>();
                script.healDamage((script.getMaxHealth() - script.enemyHealth) / 4);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}