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

public class Towers : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] protected float damage;
    [SerializeField] private float timeBtwShots;     //Time in between shots (in seconds)
    [SerializeField] protected int towerCost;       //Saves Tower Cost
    [SerializeField] protected string towerName;    //Saves Tower Name

    private float nextTimeToShoot;

    public GameObject currentTarget;

    public Transform pivot;
    public Transform barrel;
    public GameObject projectile;


    private void Start()
    {
        nextTimeToShoot = Time.time;
    }

    private void updateClosestEnemy()
    {
        GameObject currClosestEnemy = null;

        float distance = Mathf.Infinity;

        foreach (GameObject enemy in Counter.enemies)
        {
            float _distance = (transform.position - enemy.transform.position).magnitude;

            if (_distance < distance)
            {
                distance = _distance;
                currClosestEnemy = enemy;
            }
        }

        if (distance <= range)
        {
            currentTarget = currClosestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    protected virtual void shoot()
    {
        Enemies enemyScript = currentTarget.GetComponent<Enemies>();

        enemyScript.takeDamage(damage);
    }

    public float getDamage() { return damage;}

    public int getCost() { return towerCost; }

    public string getName() { return towerName; }

    private void Update()
    {
        updateClosestEnemy();
        
        if (Time.time >= nextTimeToShoot)
        {
            if (currentTarget != null)
            {
                shoot();
                nextTimeToShoot = Time.time + timeBtwShots;
            }
        }
    }
}
