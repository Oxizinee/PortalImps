using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PortalSuck : MonoBehaviour
{
    public Transform Portal;
    public float Force = 500f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.GetComponent<Rigidbody>().AddForce((Portal.position - other.gameObject.transform.position) * Force * Time.fixedDeltaTime );
        }
    }
}
