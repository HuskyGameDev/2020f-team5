﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;                             // Speed in units/second
    public Vector3 destination = Vector2.zero;      // Current point enemy is moving towards in world space
    public Vector3 nextDestination = Vector2.zero;  // Next point enemy will move to. Assigned when new waypoint area is entered, and set to destination when previous area is left
    public bool isProtected;                        // If true, enemy is invincible due to spawn protection, or it is attacking a cow
    public float health;
    public int moneyDropped;
    private float slowDuration;
    public GameObject laser;
    private float splashMinRange;
    private float splashMaxRange;
    private float lineWidth;
    private float lineDuration;
    private float towerDamage;
    private Vector3 lineEnd;

    // slow tower settings
    private Color baseColor = new Color(255, 255, 255, 255);        // normal color
    private Color slowedColor = new Color(0, 150, 255, 200);        // blue tint
    private SlowTimer slowTimer;                                    // timer that contols enemy speed
    private float minSpeed;                                         // min speed this enemy can move
    private float initialSpeed;                                     // initial speed of this enemy
    private bool slowed;                                            // flag for being slowed

    // Start is called before the first frame update
    void Start() {
        isProtected = true;
        slowTimer = gameObject.AddComponent<SlowTimer>();
        slowed = false;
        initialSpeed = speed;
        minSpeed = initialSpeed * 0.10f;
    }

    // called once per frame
    void FixedUpdate()
    {
        // unslows enemy if timer expired
        if (slowed && !slowTimer.enabled) {
            UnSlow();
        }

        // Move towards destination
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);

        // enemy death
        if (health <= 0) {
            Currency.addCurrency(moneyDropped);
            Level.EnemiesRemaining--;
            Destroy(gameObject);
        }
    }

    // enemy takes damage
    void Hit(float damage) {
        if (!isProtected)
            health -= damage;
    }

    // applies slow, adds to timer, colors enemy blue
    void Slow(float decrease) {

        slowed = true;
        float newSpeed = (speed * (1 - decrease));
        if (newSpeed >= minSpeed) {
            speed = newSpeed;
        }

        slowTimer.addDuration(4.0f);
        GetComponent<SpriteRenderer>().color = slowedColor;
    }

    // resets enemy speed / color
    void UnSlow() {
        speed = initialSpeed;
        GetComponent<SpriteRenderer>().color = baseColor;
        slowed = false;
    }
    
    void setSlowDuration(float duration) {
        slowDuration = duration;
    }

    void setMinRange(float min) {
        splashMinRange = min;
    }

    void setMaxRange(float max) {
        splashMaxRange = max;
    }

    void setLineWidth(float width) {
        lineWidth = width;
    }

    void setLineDuration(float duration) {
        lineDuration = duration;
    }

    void setTowerDamge(float damage) {
        towerDamage = damage;
    }

    void setLineEnd(Vector3 end) {
        lineEnd = end;
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

    void splashHit()
    {
        StartCoroutine(DrawLine(this.transform.position, lineEnd, Color.yellow, lineWidth, lineDuration));
    }

    void Splash()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        Vector3 position = this.transform.position;

        // calculate squared distances
        float min = splashMinRange * splashMinRange;
        float max = splashMaxRange * splashMaxRange;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance >= splashMinRange && curDistance <= splashMaxRange)
            {
                go.SendMessage("Hit", towerDamage);
                go.SendMessage("setLineWidth", lineWidth);
                go.SendMessage("setLineDuration", lineDuration);
                go.SendMessage("setTowerDamge", towerDamage);
                go.SendMessage("setLineEnd", this.transform.position);
                go.SendMessage("splashHit");
            }
        }
    }

    // timer for tracking duration remaining on slow
    class SlowTimer : MonoBehaviour {

        private float timeRemaining = 0.0f;     // time remaining
        private float maxDuration = 7.5f;       // maximum duration

        private void Awake() {
            this.enabled = false;
        }

        // adds time to the slow, cannot exceed max duration
        public void addDuration(float seconds) {
            if (! this.enabled)
                this.enabled = true;

            timeRemaining += seconds;
            if (timeRemaining > maxDuration) {
                timeRemaining = maxDuration;
            }
        }

        // updates timer, disables when reaching 0
        private void FixedUpdate() {
            if (timeRemaining >= 0) {
                timeRemaining -= Time.fixedDeltaTime;
            }
            else {
                this.enabled = false;
            }
        }
    }
}
