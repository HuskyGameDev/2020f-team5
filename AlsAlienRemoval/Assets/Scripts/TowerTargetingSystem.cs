using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TowerTargetingSystem : MonoBehaviour
{
    public Transform firePosition;
    public Transform towerPosition;
    public float fireRate;
    public float lineWidth;
    public float lineDuration;
    public float towerDamage;
    public float rangeMin;
    public float rangeMax;
    private float fireTimer;
    public GameObject laser;
    private bool hasPlaced;

    void placed(bool placed)
    {
        hasPlaced = true;
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
            Enemy enemy = GetComponent<Enemy>();
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

    IEnumerator DrawLine(Vector3 start, Vector3 end, Color color, float width, float duration)
    {
        laser.GetComponent<LineRenderer>().enabled = true;
        laser.transform.position = start;
        LineRenderer lr = laser.GetComponent<LineRenderer>();
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
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
                GameObject target = FindClosestEnemy();
                if (target != null)
                {
                    StartCoroutine(DrawLine(firePosition.position, target.transform.position, Color.red, lineWidth, lineDuration));
                    target.SendMessage("Hit", towerDamage);
                    fireTimer = 0;
                }
            }
        }
    }
}
