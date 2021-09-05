using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject arrow;
    public GameObject arrowParent;
    public GameObject spawnPoint;
    public float attackSpeed;

    private float timeStamp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timeStamp <= Time.time)
        {
            timeStamp = Time.time + (1 - (0.1f * attackSpeed));
            Instantiate(arrow, spawnPoint.transform.position, spawnPoint.transform.rotation, arrowParent.transform);
        }

    }
}
