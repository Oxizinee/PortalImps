using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ImpMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] _escapes;
    public Material _ghostMat, _deafultMat;
    public MeshRenderer Renderer;
    public bool _isGrounded, _isStunned, IsBeingHeld; 

    private float _distance;
    private int _closestEscape;
    private NavMeshAgent _agent;
    void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        _deafultMat = Renderer.sharedMaterial;
        _agent = GetComponent<NavMeshAgent>();
        _escapes = GameObject.FindGameObjectsWithTag("Escape");
        _distance = Vector3.Distance(transform.position, _escapes[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrounded && _agent != null)
        {
            if (_isStunned && !IsBeingHeld)
            {
                _agent.isStopped = true;
            }
            else if (!_isStunned && !IsBeingHeld)
            {
                _agent.isStopped = false;
                FindClosestEscape();
            }
        }

        if (isGrounded() && GetComponent<NavMeshAgent>() == null)
        {
            _agent = gameObject.AddComponent<NavMeshAgent>();
        }
        else if (!isGrounded())
        {
            transform.position -= Vector3.up * 2 * Time.deltaTime;
        }
    }

    private void FindClosestEscape()
    {
        if (_isStunned) return;
        if (IsBeingHeld) return;

        for (int i = 0; i < _escapes.Length; i++)
        {
            if (Vector3.Distance(transform.position, _escapes[i].transform.position) < _distance)
            {
                _distance = Vector3.Distance(transform.position, _escapes[i].transform.position);
                _closestEscape = i;
            }
        }

        _agent.SetDestination(_escapes[_closestEscape].transform.position); 
    }


    public void Stun(float stunDuration)
    {
        if(IsBeingHeld) return; 

        _isStunned = true;
        StartCoroutine(StunDuration(stunDuration)); 
    }

    public IEnumerator StunDuration(float stunDuration)
    {
        if (_isStunned)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(stunDuration);
           _isStunned = false;
        }
        yield return null;
    }

    public bool isGrounded()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, 0.8f))
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
