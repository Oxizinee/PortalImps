using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Level1Completed;
    public bool Level2Completed;
    public bool Level3Completed;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
