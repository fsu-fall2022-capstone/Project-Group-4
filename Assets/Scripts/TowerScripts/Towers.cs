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
    [SerializeField] protected BarrelRotation barrelRotation;
    protected List<BoonType> boons = new List<BoonType>();

    private float nextTimeToShoot;

    public GameObject currentTarget;

    public Transform barrel;
    public GameObject projectile;

    private void Start()
    {
        nextTimeToShoot = Time.time;
    }

    private void FixedUpdate()
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
        GameObject newBullet = Instantiate(projectile, barrel.position, barrelRotation.pivot);
        Bullet currentBullet = newBullet.GetComponent<Bullet>();
        currentBullet.Damage = getDamage();
        currentBullet.Target = currentTarget;
    }

    public void addBoon(BoonType boon)
    {
        Debug.Log("Boon Added To Tower!");
        boons.Add(boon);
        switch (boon)
        {
            case BoonType.Power:
                damage = damage * 2;
                break;
            case BoonType.Swiftness:
                timeBtwShots = timeBtwShots / 2;
                break;
            case BoonType.Farsight:
                range = range * 2;
                break;
        }
    }

    public void removeBoon(BoonType boon)
    {
        Debug.Log("Boon Removed From Tower");
        switch (boon)
        {
            case BoonType.Power:
                damage = damage / 2;
                break;
            case BoonType.Swiftness:
                timeBtwShots = timeBtwShots * 2;
                break;
            case BoonType.Farsight:
                range = range / 2;
                break;
        }
        boons.Remove(boon);
    }

    public float getDamage() { return damage; }

    public int getCost() { return towerCost; }

    public string getName() { return towerName; }
}
