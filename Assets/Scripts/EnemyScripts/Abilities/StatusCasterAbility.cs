using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCasterAbility : Ability
{
    protected StatusType type;
    protected float range;
    public void ConstructAbility(AbilityType abilityType, float range, float duration, float cooldown)
    {
        base.ConstructAbility(abilityType, duration, cooldown);
        this.type = constructStatusTypeFromAbilityType(abilityType);
        this.range = range;
    }

    protected StatusType constructStatusTypeFromAbilityType(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.Overcharge:
                return StatusType.Overcharged;
            case AbilityType.Sprint:
                return StatusType.Sprinting;
            case AbilityType.Shield:
                return StatusType.Shielded;
            default:
                Debug.Log($"AbilityType {type} does not have a corresponding StatusType!");
                return StatusType.None;
        }
    }

    public void castStatus(Vector3 position)
    {
        Debug.Log($"Casting Status! {type} will be cast at {position} with range {range}");
        IEnumerator coroutine = IAbilityCastStatus(position);
        StartCoroutine(coroutine);
    }

    private IEnumerator IAbilityCastStatus(Vector3 position)
    {
        List<GameObject> enemiesInRange = getEnemiesInRange(position, range);
        foreach (GameObject enemy in enemiesInRange)
        {
            if(enemy != null) {
                enemy.GetComponent<Enemy>().addStatus(new Status(type, duration));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}