using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBullet : Bullet
{
    public float radius;
    private Vector3 apexPoint;
    [SerializeField] private float parabolicHeight = 3.0f;
    private float t = 0.0f; // interpolation value for parabolic motion in Vector3.Lerp

    protected override void Start()
    {
        base.Start();
        apexPoint = transform.position + (base.Target.transform.position - transform.position) / 2 
                + new Vector3(0, parabolicHeight, 0);
    }

    protected override void FixedUpdate()
    {
        // B(t) = (1-t)[(1-t)Po + tP1] + t[(1-t)P1 + tP2], 0 <= 1 <= 1
        // https://en.wikipedia.org/wiki/B%C3%A9zier_curve#Quadratic_B%C3%A9zier_curves
        if (t < 1.0f) {
            t += Time.deltaTime;
            gameObject.transform.position = (1-t)*((1-t)*transform.position + t*apexPoint) + 
                    t*((1-t)*apexPoint + t*base.Target.transform.position);
        }

        Vector3 relative = base.Target.transform.position - transform.position;
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
            if(enemy != null) {
                if (enemy.gameObject.tag == "Enemy")
                {
                    enemy.GetComponent<Enemy>().takeDamage(base.Damage);
                }
            }
        }
    }
}