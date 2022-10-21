using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalShot : Bullet
{
    public string Element;
    public float EffectDuration;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected!");
        Enemies enemyScript = Target.GetComponent<Enemies>();
        switch (Element)
        {
            case "Ice":
                enemyScript.setStatus("Frozen", EffectDuration);
                break;
        }
        enemyScript.takeDamage(Damage);
        Destroy(gameObject);
    }
}