using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PathBlocker;
    public float RotationSpeed = 360;
    public float MovementSpeed = 100;
    public float ScaleSpeed = 80;
    public int LifeTime = 10;
    public int MaxScale = 100;
    private GameObject _pathInstance;
    private float _timer;
    private Vector3 _startPos;
    private bool _blockerSpawned = false;
    void Start()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
        transform.position += transform.forward * MovementSpeed * Time.deltaTime;

        if (_timer > LifeTime)
        {
            Destroy(gameObject);
            Destroy(_pathInstance);
        }
    }

    private void LateUpdate()
    {
        if (!_blockerSpawned)
        {
            _pathInstance = Instantiate(PathBlocker, _startPos, transform.rotation);
            _blockerSpawned=true;
        }

        if(_pathInstance != null) 
        {
            _pathInstance.transform.localScale = new Vector3(1, 1, Mathf.Clamp(_pathInstance.transform.localScale.z + ScaleSpeed * Time.deltaTime, -2, MaxScale));
        }
    }
}
