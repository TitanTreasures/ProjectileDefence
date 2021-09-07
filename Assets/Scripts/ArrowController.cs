using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    private Rigidbody rb;

    [HideInInspector]
    public GameObject ignoreGameObject = null;

    public int pierce, fork, chain;
    public float speed;
    public float damage;

    public ParticleSystem FX1, FX2;

    AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        FX1 = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        FX2 = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        AM = AudioManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null || other.gameObject.name.Contains("Arrow") || other.gameObject == ignoreGameObject)
        {


        }
        else if (other.gameObject.name.Contains("Enemy"))
        {
            //reset the ignored object now that arrow has traveled
            ignoreGameObject = other.gameObject;

            //take damage
            other.transform.parent.GetComponent<EnemyController>().TakeDamage(damage);

            //SFX
            AM.Play("Hit");

            //FX
            FX1.Play();
            FX2.Play();

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
        rb.AddForce(transform.forward * speed);
    }

    private void Fork(GameObject ignoreObject)
    {

        //spawn forking arrow/s
        GameObject forkingArrow = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        //change the rotations
        forkingArrow.transform.Rotate(0, 30, 0);
        //add the object to be ignored by the spawned arrow
        forkingArrow.GetComponent<ArrowController>().ignoreGameObject = ignoreObject;

        transform.Rotate(0, -30, 0);
        rb.AddForce(transform.forward * speed);

    }

    private void Chain(GameObject other)
    {

        Transform enemies = other.transform.root;
        if (enemies == null)
        {
            Debug.Log("enemies = null");
        }

        float distance2 = 999;
        int target = -1;
        for (int i = 0; i < enemies.childCount; i++)
        {
            float distance1 = Vector3.Distance(enemies.GetChild(i).transform.position, transform.position);
            if (distance1 < distance2 && enemies.GetChild(i).gameObject != other.transform.parent.gameObject && enemies.GetChild(i).GetComponent<EnemyController>().isAlive)
            {
                distance2 = distance1;
                target = i;
            }
        }

        if (target > -1)
        {
            //find the direction
            
            Vector3 lookPos = enemies.GetChild(target).transform.position - transform.position;
            //test this
            float angle = Vector3.SignedAngle(lookPos, transform.forward, Vector3.up);
            transform.Rotate(0, angle*-1, 0);

            rb.AddForce(transform.forward * speed);

            /*
            Vector3 targetDirection = enemies.GetChild(target).transform.position - transform.position;
            Vector3 currentDirection = transform.forward;

            float angle;
            double dotProduct = targetDirection.x * targetDirection.y + currentDirection.x * currentDirection.y;
            double magnitude = Math.Sqrt(Math.Pow(targetDirection.x, 2) + Math.Pow(targetDirection.y, 2)) * Math.Sqrt(Math.Pow(currentDirection.x, 2) + Math.Pow(currentDirection.y, 2));
            angle = (float)Math.Cos(dotProduct / magnitude);
            Debug.Log(targetDirection);
            Debug.Log(currentDirection);
            Debug.Log(dotProduct);
            Debug.Log(magnitude);
            Debug.Log(angle);
            */
        }
        else
        {
            StickArrow(other);
        }
    }
}
