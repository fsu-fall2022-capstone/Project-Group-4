using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBullet : Bullet
{
    public float radius;
    private Vector3 apexPoint;
    private Vector3 finalPoint;
    [SerializeField] private float parabolicHeight = 3.0f;
    private float t = 0.0f; // interpolation value for parabolic motion in Vector3.Lerp

    protected override void Start()
    {
        base.Start();
        finalPoint = base.Target.transform.position;
        apexPoint = transform.position + (finalPoint - transform.position) / 2
                + new Vector3(0, parabolicHeight, 0);
    }

    protected override void FixedUpdate()
    {

        if (base.Target != null)
        {
            if (t < 1.0f)
            {
                calcBoziersCurve();
            }

            updateRotation();
            finalPoint = base.Target.transform.position;
        }
        else
        {
            if (t < 1.0f)
            {
                calcBoziersCurve();
            }
            else
            {
                OnBulletCollisionEffect();
                Destroy(gameObject);
            }

            updateRotation();
        }
    }

    // B(t) = (1-t)[(1-t)Po + tP1] + t[(1-t)P1 + tP2], 0 <= t <= 1
    // https://en.wikipedia.org/wiki/B%C3%A9zier_curve#Quadratic_B%C3%A9zier_curves
    private void calcBoziersCurve()
    {
        t += Time.deltaTime;
        gameObject.transform.position = (1 - t) * ((1 - t) * transform.position + t * apexPoint) +
                t * ((1 - t) * apexPoint + t * finalPoint);
    }

    private void updateRotation()
    {
        Vector3 relative = finalPoint - transform.position;
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        Vector3 newRotation = new Vector3(0, 0, angle);
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    protected override void OnBulletCollisionEffect()
    {
        Debug.Log("Target Hit!");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy.gameObject.tag == "Enemy")
                {
                    enemy.GetComponent<Enemy>().takeDamage(base.Damage);
                }
            }
        }
    }
}