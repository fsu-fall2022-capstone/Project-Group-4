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
        specialAbility = new Ability(abilityType, maxDuration, maxCooldown);
    }

    protected override void Update() {
        base.Update();
        checkSpecialAbility();
    }

    private void checkSpecialAbility() {
        if (specialAbility.isReady()) {
            useSpecialAbility();
            specialAbility.startAbility();
        }
    }

    public void useSpecialAbility() {

    }
}