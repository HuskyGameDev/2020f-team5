using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public GameObject laser;
    public Color color;
    public float width;
    public float duration;

    void setStartPosition(Vector3 position)
    {
        start = position;
    }

    void setEndPosition(Vector3 position)
    {
        end = position;
    }

    void setColor(Color newColor)
    {
        color = newColor;
    }

    void setWidth(float newWidth)
    {
        width = newWidth;
    }

    void setDuration(float newDuration)
    {
        duration = newDuration;
    }

    IEnumerator DrawLine()
    {
        laser.GetComponent<LineRenderer>().enabled = true;
        laser.transform.position = start;
        LineRenderer lr = laser.GetComponent<LineRenderer>();
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startColor = color;
        lr.endColor = color;
        yield return new WaitForSeconds(duration);
        laser.GetComponent<LineRenderer>().enabled = false;
        Destroy(gameObject);
    }

    void draw()
    {
        StartCoroutine(DrawLine());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
