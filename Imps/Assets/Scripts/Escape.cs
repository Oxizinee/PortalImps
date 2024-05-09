using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _escapeSound;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            _escapeSound.Play();
            Destroy(other.gameObject);
        }
    }

    private void Start()
    {
        _escapeSound = GetComponent<AudioSource>();
    }
}
