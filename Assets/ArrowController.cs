using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 1000);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == null){
            //Do nothing

        } else if (collision.gameObject.name.Contains("enemy")){
            //code for arrow behaviour

        }
        else{
            //stop the arrow and set hit object as parent
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            transform.parent = collision.transform;
        }
    }
}
