using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Imp"))
        {
            other.GetComponent<ImpMovement>().IsInBounds = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Imp"))
        {
            other.GetComponent<ImpMovement>().IsInBounds = true;
        }
    }
}
