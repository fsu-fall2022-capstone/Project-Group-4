using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTowers : Towers
{
    public ElementType element;
    public float effectDuration;

    protected override void shoot()
    {
        GameObject newProjectile = Instantiate(base.projectile, barrel.position, base.barrelRotation.pivot);
        ElementalShot currentProjectile = newProjectile.GetComponent<ElementalShot>();
        currentProjectile.Damage = base.getDamage();
        currentProjectile.Target = currentTarget;
        currentProjectile.Element = element;
        currentProjectile.EffectDuration = effectDuration;
    }
}