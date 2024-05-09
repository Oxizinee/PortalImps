using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _originalSize;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField]private Vector3 _targetScale;

    public float Time = 0.3f;
    public float ScaleFactor = 1.2f;
    void Start()
    {
        _originalSize = transform.localScale;
        _targetScale = _originalSize * ScaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, _targetScale, ref _velocity, Time);

        if (transform.localScale == _originalSize * ScaleFactor)
        {
            _targetScale = _originalSize;
        }
        else if (transform.localScale == _originalSize)
        {
            _targetScale = _originalSize * ScaleFactor;
        }

    }
}
