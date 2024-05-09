using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorChange : MonoBehaviour
{
    private Vector3 _originalSize;
    private Text _text;
    private float _timer = 0;

    private void Start()
    {
        _originalSize = transform.localScale;
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer < GetComponent<ScalingUI>().Time * 4)
        {
            _text.color = Color.red;
        }
        else
        {
            _text.color = Color.black;
            if (_timer > GetComponent<ScalingUI>().Time * 8)
            {
                _timer = 0;
            }
        }
    }
}
