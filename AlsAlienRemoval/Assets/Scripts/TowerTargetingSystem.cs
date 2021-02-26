using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using System;

public class TowerTargetingSystem : MonoBehaviour
{
    public Transform firePosition;
    public Transform towerPosition;
    public int upgradeCost;
    public float fireRate;
    public float lineWidth;
    public float lineDuration;
    public float towerDamage;
    public float towerSpeedDecreasePercent;
    public float towerSlowDuration;
    public float rangeMin;
    public float rangeMax;
    private float fireTimer;
    private bool hasPlaced;
    private int towerType;
    private int timesUpgraded;
    public float splashMinRange;
    public float splashMaxRange;
    public TMP_Text TargetText;
    private int targetType;
    public Projectile projectile;
    public Laser laser;
    private AudioSource fireSoundSource;

    void placed(bool placed)
    {
        hasPlaced = true;
    }

    void setTowerType(int type)
    {
        towerType = type;

        if(type == 1)
        {
            targetType = 1;
            TargetText.text = "Closest";
        }
        else if(type == 2)
        {
            targetType = 2;
            TargetText.text = "Weakest";
        }
        else if (type == 3)
        {
            targetType = 4;
            TargetText.text = "Fastest";
        }
        else if (type == 4)
        {
            targetType = 3;
            TargetText.text = "Strongest";
        }
        else if(type == 5)
        {
            targetType = 1;
            TargetText.text = "Closest";
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = towerPosition.position;

        // calculate squared distances
        float min = rangeMin * rangeMin;
        float max = rangeMax * rangeMax;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance >= min && curDistance <= max)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private GameObject FindWeakestEnemy()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject weakest = null;
        float minHealth = Mathf.Infinity;
        Vector3 position = towerPosition.position;

        // calculate squared distances
        float min = rangeMin * rangeMin;
        float max = rangeMax * rangeMax;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            Enemy enemy = go.GetComponent<Enemy>();
            float health = enemy.health;
            float curDistance = diff.sqrMagnitude;
            if (health < minHealth && curDistance >= min && curDistance <= max)
            {
                weakest = go;
                minHealth = health;
            }
        }
        return weakest;
    }

    private GameObject FindStrongestEnemy()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject strongest = null;
        float maxHealth = 0;
        Vector3 position = towerPosition.position;

        // calculate squared distances
        float min = rangeMin * rangeMin;
        float max = rangeMax * rangeMax;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            Enemy enemy = go.GetComponent<Enemy>();
            float health = enemy.health;
            float curDistance = diff.sqrMagnitude;
            if (health > maxHealth && curDistance >= min && curDistance <= max)
            {
                strongest = go;
                maxHealth = health;
            }
        }
        return strongest;
    }

    private GameObject FindFastestEnemy()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject fastest = null;
        float maxSpeed = 0;
        Vector3 position = towerPosition.position;

        // calculate squared distances
        float min = rangeMin * rangeMin;
        float max = rangeMax * rangeMax;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            Enemy enemy = go.GetComponent<Enemy>();
            float speed = enemy.speed;
            float curDistance = diff.sqrMagnitude;
            if (speed > maxSpeed && curDistance >= min && curDistance <= max)
            {
                fastest = go;
                maxSpeed = speed;
            }
        }
        return fastest;
    }

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = fireRate;
        hasPlaced = false;

        fireSoundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPlaced)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                GameObject target = null;
                if (targetType == 1)
                {
                    target = FindClosestEnemy();
                }
                else if (targetType == 2)
                {
                    target = FindWeakestEnemy();
                }
                else if (targetType == 3)
                {
                    target = FindStrongestEnemy();
                }
                else if (targetType == 4)
                {
                    target = FindFastestEnemy();
                }

                if (towerType == 1)
                {
                    if (target != null)
                    {
                        Laser drawLaser = Instantiate(laser);
                        drawLaser.SendMessage("setStartPosition", firePosition.position);
                        drawLaser.SendMessage("setEndPosition", target.transform.position);
                        drawLaser.SendMessage("setColor", Color.red);
                        drawLaser.SendMessage("setWidth", lineWidth);
                        drawLaser.SendMessage("setDuration", lineDuration);
                        drawLaser.SendMessage("draw");
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }
                else if(towerType == 2)
                {
                    if (target != null)
                    {
                        Laser drawLaser = Instantiate(laser);
                        drawLaser.SendMessage("setStartPosition", firePosition.position);
                        drawLaser.SendMessage("setEndPosition", target.transform.position);
                        drawLaser.SendMessage("setColor", Color.magenta);
                        drawLaser.SendMessage("setWidth", lineWidth);
                        drawLaser.SendMessage("setDuration", lineDuration);
                        drawLaser.SendMessage("draw");
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }
                else if (towerType == 3)
                {
                    if (target != null)
                    {
                        Laser drawLaser = Instantiate(laser);
                        drawLaser.SendMessage("setStartPosition", firePosition.position);
                        drawLaser.SendMessage("setEndPosition", target.transform.position);
                        drawLaser.SendMessage("setColor", Color.cyan);
                        drawLaser.SendMessage("setWidth", lineWidth);
                        drawLaser.SendMessage("setDuration", lineDuration);
                        drawLaser.SendMessage("draw");
                        target.SendMessage("setSlowDuration", towerSlowDuration);
                        target.SendMessage("Slow", towerSpeedDecreasePercent);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }
                else if (towerType == 4)
                {
                    if (target != null)
                    {
                        Laser drawLaser = Instantiate(laser);
                        drawLaser.SendMessage("setStartPosition", firePosition.position);
                        drawLaser.SendMessage("setEndPosition", target.transform.position);
                        drawLaser.SendMessage("setColor", Color.yellow);
                        drawLaser.SendMessage("setWidth", lineWidth);
                        drawLaser.SendMessage("setDuration", lineDuration);
                        drawLaser.SendMessage("draw");
                        target.SendMessage("setLineWidth", lineWidth);
                        target.SendMessage("setLineDuration", lineDuration);
                        target.SendMessage("setTowerDamge", towerDamage);
                        target.SendMessage("setMinRange", splashMinRange);
                        target.SendMessage("setMaxRange", splashMaxRange);
                        target.SendMessage("Splash");
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }
                else if (towerType == 5)
                {
                    if(target != null)
                    {
                        firePosition.right = target.transform.position - firePosition.position;
                        Instantiate(projectile, firePosition.position, firePosition.rotation);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }
            }

        }
    }

    public void upgradeTower()
    {
        if (Currency.amount >= upgradeCost)
        {
            if (timesUpgraded < 3)
            {
                if (towerType == 1)
                {
                    towerDamage = towerDamage + 5;
                }
                if (towerType == 2)
                {
                    towerDamage = towerDamage + 3;
                }
                if (towerType == 3)
                {
                    towerSlowDuration = towerSlowDuration + (float)0.125;
                }
                if (towerType == 4)
                {
                    towerDamage = towerDamage + 10;
                }
                timesUpgraded = timesUpgraded + 1;
                upgradeCost = upgradeCost * 2;
            }
        }
       
    }

    public void changeTargetingUp()
    {
        targetType++;
        if(targetType > 4)
        {
            targetType = 1;
        }

        if(targetType == 1)
        {
            TargetText.text = "Closest";
        }
        else if(targetType == 2)
        {
            TargetText.text = "Weakest";
        }
       else if(targetType == 3)
        {
            TargetText.text = "Strongest";
        }
        else if(targetType == 4)
        {
            TargetText.text = "Fastest";
        }
    }

    public void changeTargetingDown()
    {
        targetType--;
        if (targetType < 1)
        {
            targetType = 4;
        }

        if (targetType == 1)
        {
            TargetText.text = "Closest";
        }
        else if (targetType == 2)
        {
            TargetText.text = "Weakest";
        }
        else if (targetType == 3)
        {
            TargetText.text = "Strongest";
        }
        else if (targetType == 4)
        {
            TargetText.text = "Fastest";
        }
    }
}
