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
    public float Speed = 2f;
    public float MaxHeight = 5f;
    public float MinHeight = 0f;
    private float _direction = 1; 
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



    private void Update()
    {
        transform.Rotate(Vector3.right, 30 * Time.deltaTime);


        transform.Translate(Vector3.right * Speed * _direction * Time.deltaTime);

        // Check if the object has reached the maximum height or minimum height, change direction accordingly
        if (transform.position.y >= MaxHeight)
        {
            _direction = -1; // Change direction to move down
        }
        else if (transform.position.y <= MinHeight)
        {
            _direction = 1; // Change direction to move up
        }
    }



}
