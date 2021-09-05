using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject arrow;
    public GameObject enemyParent;
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
        timeStamp = Time.time + (1 - (0.1f * attackSpeed));
        
        if (timeStamp <= Time.time)
        {
            Instantiate(arrow, spawnPoint.transform.position, Quaternion.identity, enemyParent.transform);
        }

    }
}
