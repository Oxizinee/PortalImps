using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ImpMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] _escapes;
    public bool _isGrounded; 
    private float _distance;
    private int _closestEscape;
    private NavMeshAgent _agent;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _escapes = GameObject.FindGameObjectsWithTag("Escape");
        _distance = Vector3.Distance(transform.position, _escapes[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isGrounded())
        //{
        //    FindClosestEscape();
        //}
        //else
        //{
        //    transform.position -= (Vector3.up * 2f) * Time.deltaTime;
        //}
        FindClosestEscape();
    }

    private void FindClosestEscape()
    {
        for (int i = 0; i < _escapes.Length; i++)
        {
            if (Vector3.Distance(transform.position, _escapes[i].transform.position) < _distance)
            {
                _distance = Vector3.Distance(transform.position, _escapes[i].transform.position);
                _closestEscape = i;
            }
        }

        // transform.position = Vector3.Lerp(transform.position, _escapes[_closestEscape].transform.position, _distance * Time.deltaTime);  
        _agent.SetDestination(_escapes[_closestEscape].transform.position); 
    }

    private bool isGrounded()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, 0.4f))
        {
            _isGrounded = true;
            return true;
        }
        else
        {
            _isGrounded = false;
            return false;
        }
    }
}
