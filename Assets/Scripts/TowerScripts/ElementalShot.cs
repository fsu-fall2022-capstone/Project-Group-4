using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalShot : Bullet
{
    public ElementType Element;
    public float EffectDuration;
    public bool upgraded = false;

    protected override void OnBulletCollisionEffect()
    {
        Enemy enemyScript = Target.GetComponent<Enemy>();
        switch (Element)
        {
            case ElementType.Ice:
                enemyScript.addStatus(new Status(StatusType.Frozen, EffectDuration));
                if (upgraded)
                    enemyScript.addStatus(new Status(StatusType.Burning, EffectDuration, 2));
                enemyScript.takeDamage(Damage);
                break;
            case ElementType.Fire:
                enemyScript.addStatus(new Status(StatusType.Burning, EffectDuration, 2));
                if (upgraded)
                {
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(Target.transform.position, 2);
                    foreach (Collider2D enemy in enemies)
                    {
                        if (enemy != null)
                        {
                            if (enemy.gameObject.tag == "Enemy")
                            {
                                enemy.GetComponent<Enemy>().addStatus(new Status(StatusType.Burning, EffectDuration, 2));
                            }
                        }
                    }
                }
                enemyScript.takeDamage(Damage);
                break;
            case ElementType.Lightning:
                enemyScript.addStatus(new Status(StatusType.Stunned, EffectDuration));
                if (!upgraded)
                {
                    enemyScript.takeDamage(Damage);
                }
                else
                {
                    enemyScript.addStatus(new Status(StatusType.Electrocuted, EffectDuration, Damage));
                }
                break;
            default:
                Debug.Log($"Element not implemented! {Element}");
                break;
        }
    }
}
