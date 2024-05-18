using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public AudioSource EnterPortalAudio;
    private LevelManager _levelManager;
    private void Start()
    {
        _levelManager = GameObject.FindFirstObjectByType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            _levelManager.ImpAmount++;
            EnterPortalAudio.Play();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            _levelManager.ImpAmount++;
            EnterPortalAudio.Play();
            Destroy(other.gameObject);
        }
    }


   

}
