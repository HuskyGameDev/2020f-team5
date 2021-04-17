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
    public float cascadeDecrease;
    public int cascadeTimes;
    public float cascadeRangeMin;
    public float cascadeRangeMax;
    public TMP_Text TargetText;
    public TMP_Text upgradeText;
    public TMP_Text sellText;
    private int targetType;
    public Projectile projectile;
    public Laser laser;
    public float firedamage;
    private AudioSource fireSoundSource;
    public Explosion explosion;
    private bool changeTarget;
    private Vector3 oldposition;
    public GameObject explosionTarget;
    public GameObject explosiveFirePoint;
    public float explosiveFirePointDuration;


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
            upgradeText.text = "Upgrade:\n+Damage\n$" + upgradeCost;
            sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
        }
        else if(type == 2)
        {
            targetType = 2;
            TargetText.text = "Weakest";
            upgradeText.text = "Upgrade:\n+Damage\n$" + upgradeCost;
            sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
        }
        else if (type == 3)
        {
            targetType = 4;
            TargetText.text = "Fastest";
            upgradeText.text = "Upgrade:\n+Duration\n$" + upgradeCost;
            sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
        }
        else if (type == 4)
        {
            targetType = 3;
            TargetText.text = "Strongest";
            upgradeText.text = "Upgrade:\n+Damage\n$" + upgradeCost;
            sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
        }
        else if(type == 5)
        {
            targetType = 1;
            TargetText.text = "Closest";
            upgradeText.text = "Upgrade:\n+Damage\n$" + upgradeCost;
            sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
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

        changeTarget = false;
        oldposition = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPlaced)
        {
            if(changeTarget)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
                explosionTarget.transform.position = new Vector2(worldPoint2d.x, worldPoint2d.y);

                if (Input.GetMouseButtonDown(0))
                {
                    changeTarget = false;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    explosionTarget.transform.position = oldposition;
                    changeTarget = false;
                }
            }

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

                // earth tower
                if (towerType == 1)
                {
                    if (target != null)
                    {
                        Laser drawLaser = Instantiate(laser);
                        drawLaser.SendMessage("setStartPosition", firePosition.position);
                        drawLaser.SendMessage("setEndPosition", target.transform.position);
                        drawLaser.SendMessage("setColor", Color.green);
                        drawLaser.SendMessage("setWidth", lineWidth);
                        drawLaser.SendMessage("setDuration", lineDuration);
                        drawLaser.SendMessage("draw");
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }

                // rapid tower
                else if(towerType == 2)
                {
                    if (target != null)
                    {
                        int cascadeCount = 0;
                        Laser drawLaser = Instantiate(laser);
                        drawLaser.SendMessage("setStartPosition", firePosition.position);
                        drawLaser.SendMessage("setEndPosition", target.transform.position);
                        drawLaser.SendMessage("setColor", Color.yellow);
                        drawLaser.SendMessage("setWidth", lineWidth);
                        drawLaser.SendMessage("setDuration", lineDuration);
                        drawLaser.SendMessage("draw");
                        if (timesUpgraded >= 3)
                        {
                            target.SendMessage("setCascadeDecrease", cascadeDecrease);
                            target.SendMessage("setCascadeTimes", cascadeTimes);
                            target.SendMessage("setLineWidth", lineWidth);
                            target.SendMessage("setLineDuration", lineDuration);
                            target.SendMessage("setTowerDamge", towerDamage);
                            target.SendMessage("setMinRangeCascade", cascadeRangeMin);
                            target.SendMessage("setMaxRangeCascade", cascadeRangeMax);
                            target.SendMessage("setCascadeCount", cascadeCount);
                            target.SendMessage("Cascade");
                        }
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }

                // slow tower
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
                        if (timesUpgraded >= 3)
                        {
                            target.SendMessage("setLineWidth", lineWidth);
                            target.SendMessage("setLineDuration", lineDuration);
                            target.SendMessage("setMinRange", splashMinRange);
                            target.SendMessage("setMaxRange", splashMaxRange);
                            target.SendMessage("setSlowDuration", towerSlowDuration);
                            target.SendMessage("setDecrease", towerSpeedDecreasePercent);
                            target.SendMessage("SplashIce");
                        }
                        target.SendMessage("Slow", towerSpeedDecreasePercent);
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }

                // splash tower
                else if (towerType == 4)
                {
                    if (target != null)
                    {
                        if (timesUpgraded <= 2)
                        {
                            Laser drawLaser = Instantiate(laser);
                            drawLaser.SendMessage("setStartPosition", firePosition.position);
                            drawLaser.SendMessage("setEndPosition", target.transform.position);
                            drawLaser.SendMessage("setColor", Color.red);
                            drawLaser.SendMessage("setWidth", lineWidth);
                            drawLaser.SendMessage("setDuration", lineDuration);
                            drawLaser.SendMessage("draw");
                            target.SendMessage("setLineWidth", lineWidth);
                            target.SendMessage("setLineDuration", lineDuration);
                            target.SendMessage("setMinRange", splashMinRange);
                            target.SendMessage("setMaxRange", splashMaxRange);
                            if (timesUpgraded == 2)
                            {
                                target.SendMessage("setFireDamage", firedamage);
                                target.SendMessage("SplashFire");
                                target.SendMessage("SetOnFire");
                                fireTimer = 0;
                            }
                            else if (timesUpgraded < 2)
                            {
                                target.SendMessage("setTowerDamge", towerDamage);
                                target.SendMessage("Splash");
                                target.SendMessage("Hit", towerDamage);
                                fireTimer = 0;
                            }
                            fireSoundSource.Play();
                        }
                    }
                    if (timesUpgraded >= 3 && ifEnemyExists())
                    {
                        if (changeTarget)
                        {
                            StartCoroutine(ExplsoiveFirePoint());
                            Instantiate(explosion, oldposition, Quaternion.Euler(0, 0, 0));
                            fireTimer = 0;
                        }
                        else
                        {
                            StartCoroutine(ExplsoiveFirePoint());
                            Instantiate(explosion, explosionTarget.transform.position, Quaternion.Euler(0, 0, 0));
                            fireTimer = 0;
                        }
                    }
                }

                // projectile tower
                else if (towerType == 5)
                {
                    if(target != null)
                    {
                        firePosition.right = target.transform.position - firePosition.position;
                        Instantiate(projectile, firePosition.position, firePosition.rotation);
                        if(timesUpgraded >= 3)
                        {
                            Instantiate(projectile, firePosition.position, firePosition.rotation*Quaternion.Euler(0,0,22.5f));
                            Instantiate(projectile, firePosition.position, firePosition.rotation*Quaternion.Euler(0,0,-22.5f));
                        }
                        fireTimer = 0;
                        fireSoundSource.Play();
                    }
                }
            }

        }
    }

    private bool ifEnemyExists()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        bool flag = false;
        foreach(GameObject go in gos)
        {
            flag = true;
        }
        return flag;
    }

    public void upgradeTower()
    {
        if (Currency.getCurrency() >= upgradeCost)
        {
            if (timesUpgraded < 3)
            {
                if (towerType == 1)
                {
                    if (timesUpgraded == 2)
                    {
                        towerDamage = towerDamage + 20;
                    }
                    else
                    {
                        towerDamage = towerDamage + 5;
                    }
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
                if(towerType == 5)
                {
                    towerDamage = towerDamage + 5;
                }
                timesUpgraded = timesUpgraded + 1;
                upgradeCost = (int)(upgradeCost * 1.2);
                if (timesUpgraded < 3)
                {
                    if(timesUpgraded == 2 && towerType == 5)
                    {
                        upgradeText.text = "Upgrade:\n+Damage\n+Triple Shot\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else if(timesUpgraded == 2 && towerType == 2)
                    {
                        upgradeText.text = "Upgrade:\n+Damage\n+Cascade\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else if(timesUpgraded == 2 && towerType == 3)
                    {
                        upgradeText.text = "Upgrade:\n+Duration\n+Splash\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else if (timesUpgraded == 2 && towerType == 1)
                    {
                        upgradeText.text = "Upgrade:\n+Huge Damage\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else if (timesUpgraded == 2 && towerType == 4)
                    {
                        upgradeText.text = "Upgrade:\n+Damage\n+Explosive\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else if (timesUpgraded == 1 && towerType == 4)
                    {
                        upgradeText.text = "Upgrade:\n+Damage\n+ Fire Damage\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else if(towerType == 3)
                    {
                        upgradeText.text = "Upgrade:\n+Duration\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                    else
                    {
                        upgradeText.text = "Upgrade:\n+Damage\n$" + upgradeCost;
                        sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                    }
                }
                else
                {
                    if(towerType == 4 && timesUpgraded >= 3)
                    {
                        this.SendMessage("setTarget");
                    }
                    upgradeText.text = "No More Upgrades";
                    sellText.text = "Sell Amount:\n$" + (upgradeCost - (upgradeCost / 2));
                }
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

    public void changeExplosionTarget()
    {
        changeTarget = true;
        oldposition = explosionTarget.transform.position;
    }

    IEnumerator ExplsoiveFirePoint()
    {
        explosiveFirePoint.SetActive(true);

        yield return new WaitForSeconds(explosiveFirePointDuration);

        explosiveFirePoint.SetActive(false);
    }
}
