using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalShot : Bullet
{
    public ElementType Element;
    public float EffectDuration;

    protected override void OnBulletCollisionEffect() {
        Debug.Log("Target Hit!");
        Enemy enemyScript = Target.GetComponent<Enemy>();
        switch (Element)
        {
            case ElementType.Ice:
                enemyScript.addStatus(new Status(StatusType.Frozen, EffectDuration));
                break;
            case ElementType.Fire:
                enemyScript.addStatus(new Status(StatusType.Burning, EffectDuration));
                break;
            case ElementType.Water:
                //enemyScript.addStatus(new Status(StatusType.Frozen, EffectDuration));
                break;
            case ElementType.Lightning:
                enemyScript.addStatus(new Status(StatusType.Electrocuted, EffectDuration));
                break;
            default:
                Debug.Log($"Element not implemented! {Element}");
                break;
        }
        enemyScript.takeDamage(Damage);
    }
}
