using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLines : MonoBehaviour
{
    // Start is called before the first frame update
    public float LinesShowDuration = 0.4f;
    private float _timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > LinesShowDuration)
        {
            _timer = 0;
            this.gameObject.SetActive(false);
        }
    }
}
