using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    private Rigidbody rb;

    [HideInInspector]
    public GameObject ignoredInitialObject = null;

    public int pierce, fork, chain;
    public float speed;

    AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        AM = AudioManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null || other.gameObject.name.Contains("Arrow") || other.gameObject == ignoredInitialObject)
        {
            //ignore only once!
            if (other.gameObject == ignoredInitialObject)
            {
                ignoredInitialObject = null;
            }
            //Do nothing

        }
        else if (other.gameObject.name.Contains("Enemy"))
        {
            //reset the ignored object now that arrow has traveled
            ignoredInitialObject = null;

            //take damage
            //other.GetComponent<EnemyController>().TakeDamage();

            //Audio
            AM.Play("Hit");

            rb.velocity = Vector3.zero;

            //code for arrow behaviour
            if (pierce > 0)
            {
                pierce--;
                Pierce();

            }
            else if (fork > 0)
            {
                fork--;
                Fork(other.gameObject);

            }
            else if (chain > 0)
            {
                chain--;
                Chain(other.gameObject);

            }
            else
            {
                StickArrow(other.gameObject);
            }

        }
        else
        {
            StickArrow(other.gameObject);
        }
    }

    private void StickArrow(GameObject other)
    {
        //stop the arrow and set hit object as parent
        rb.isKinematic = true;
        transform.SetParent(other.transform, true);
    }

    private void Pierce()
    {

    }

    private void Fork(GameObject ignoreObject)
    {

        //spawn forking arrow/s
        GameObject forkingArrow = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        //change the rotations
        forkingArrow.transform.Rotate(0, 30, 0);
        //add the object to be ignored by the spawned arrow
        forkingArrow.GetComponent<ArrowController>().ignoredInitialObject = ignoreObject;

        transform.Rotate(0, -30, 0);
        rb.AddForce(transform.forward * speed);

    }

    private void Chain(GameObject other)
    {

        Transform enemies = other.transform.parent;

        float distance2 = 999;
        int target = -1;
        for (int i = 0; i < enemies.childCount; i++)
        {
            float distance1 = Vector3.Distance(enemies.GetChild(i).transform.position, transform.position);
            if (distance1 < distance2 && enemies.GetChild(i).gameObject != other.gameObject)
            {
                distance2 = distance1;
                target = i;
            }
        }
        if (target > -1)
        {
            //find the direction
            
            Vector3 lookPos = enemies.GetChild(target).transform.position - transform.position;

            transform.Rotate(0, Vector3.Angle(lookPos, transform.forward)*Mathf.Sign(enemies.GetChild(target).transform.position.z), 0);

            rb.AddForce(transform.forward * speed);
        }
        else
        {
            StickArrow(other);
        }
    }
}
