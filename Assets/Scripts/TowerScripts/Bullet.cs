/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names may have been changed and some parts of the code may
    have been modified to fit our game scheme, these sections will be marked with 
    comments. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public GameObject Target;

    protected void Start()
    {
        Destroy(gameObject, 10f);
    }

    protected void FixedUpdate()
    {
        transform.position += transform.right * 0.25f;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected!");
        if (Target != null)
        {
            if (collision.gameObject == Target)
            {
                OnBulletCollisionEffect();
            }
        }
        Destroy(gameObject);
    }

    protected virtual void OnBulletCollisionEffect()
    {
        Debug.Log("Target Hit!");
        Target.GetComponent<Enemy>().takeDamage(Damage);
    }
}
