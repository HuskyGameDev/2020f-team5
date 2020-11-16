using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TowerTargetingSystem : MonoBehaviour
{
    public Transform firePosition;
    public Transform towerPosition;
    public int upgradeCost;
    public float fireRate;
    public float lineWidth;
    public float lineDuration;
    public float towerDamage;
    public float towerSpeedDecrease;
    public float towerSlowDuration;
    public float rangeMin;
    public float rangeMax;
    private float fireTimer;
    public GameObject laser;
    private bool hasPlaced;
    private int towerType;
    private int timesUpgraded;
    public float splashMinRange;
    public float splashMaxRange;

    void placed(bool placed)
    {
        hasPlaced = true;
    }

    void setTowerType(int type)
    {
        towerType = type;
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

    IEnumerator DrawLine(Vector3 start, Vector3 end, Color color, float width, float duration)
    {
        laser.GetComponent<LineRenderer>().enabled = true;
        laser.transform.position = start;
        LineRenderer lr = laser.GetComponent<LineRenderer>();
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.material.color = color;
        yield return new WaitForSeconds(duration);
        laser.GetComponent<LineRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = fireRate;
        hasPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPlaced)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                if (towerType == 1)
                {
                    GameObject target = FindClosestEnemy();
                    if (target != null)
                    {
                        StartCoroutine(DrawLine(firePosition.position, target.transform.position, Color.red, lineWidth, lineDuration));
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                    }
                }
                else if(towerType == 2)
                {
                    GameObject target = FindWeakestEnemy();
                    if (target != null)
                    {
                        StartCoroutine(DrawLine(firePosition.position, target.transform.position, Color.magenta, lineWidth, lineDuration));
                        target.SendMessage("Hit", towerDamage);
                        fireTimer = 0;
                    }
                }
                else if (towerType == 3)
                {
                    GameObject target = FindFastestEnemy();
                    if (target != null)
                    {
                        StartCoroutine(DrawLine(firePosition.position, target.transform.position, Color.blue, lineWidth, lineDuration));
                        target.SendMessage("setSlowDuration", towerSlowDuration);
                        target.SendMessage("Slow", towerSpeedDecrease);
                        fireTimer = 0;
                    }
                }
                else if (towerType == 4)
                {
                    GameObject target = FindStrongestEnemy();
                    if (target != null)
                    {
                        StartCoroutine(DrawLine(firePosition.position, target.transform.position, Color.yellow, lineWidth, lineDuration));
                        target.SendMessage("Hit", towerDamage);
                        target.SendMessage("setLineWidth", lineWidth);
                        target.SendMessage("setLineDuration", lineDuration);
                        target.SendMessage("setTowerDamge", towerDamage);
                        target.SendMessage("setMinRange", splashMinRange);
                        target.SendMessage("setMaxRange", splashMaxRange);
                        target.SendMessage("Splash");
                        fireTimer = 0;
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
                    towerSlowDuration = towerSlowDuration + 1;
                }
                if (towerType == 4)
                {
                    towerDamage = towerDamage + 5;
                }
                timesUpgraded = timesUpgraded + 1;
                upgradeCost = upgradeCost * 2;
            }
        }
       
    }
}
