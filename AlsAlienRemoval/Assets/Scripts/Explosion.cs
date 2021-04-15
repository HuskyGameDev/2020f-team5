using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float splashMinRange;
    public float splashMaxRange;
    public float damagePerUpdate;
    public float explosionDuration;

    // Start is called before the first frame update
    void Start()
    {
        SplashFire();
        StartCoroutine(DestroyAfterTime(explosionDuration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }

    void SplashFire()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        Vector3 position = this.transform.position;

        // calculate squared distances
        float min = splashMinRange * splashMinRange;
        float max = splashMaxRange * splashMaxRange;
        foreach (GameObject go in gos)
        {
            if (!(go.Equals(this.gameObject)))
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance >= min && curDistance <= max)
                {
                    go.SendMessage("setFireDamage", damagePerUpdate);
                    go.SendMessage("SetOnFire");
                }
            }
        }
    }
}
