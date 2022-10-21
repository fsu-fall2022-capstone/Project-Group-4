/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names may have been changed and some parts of the code may
    have been modified to fit our game scheme, these sections will be marked with 
    comments. 

    //GetCost and GetName made by Alex Martinez
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTowers : Towers
{
    public string element;
    public float effectDuration;

    protected override void shoot()
    {
        GameObject newProjectile = Instantiate(base.projectile, barrel.position, pivot.rotation);
        ElementalShot currentProjectile = newProjectile.GetComponent<ElementalShot>();
        currentProjectile.Damage = base.getDamage();
        currentProjectile.Target = currentTarget;
        currentProjectile.Element = element;
        currentProjectile.EffectDuration = effectDuration;
        //      base.shoot();
    }
}

public enum Elements
{
    Ice, Fire, Water, Lightning
}
