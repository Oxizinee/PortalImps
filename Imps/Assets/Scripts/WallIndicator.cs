using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallIndicator : MonoBehaviour
{
    public float speed = 2f; 
    public float maxHeight = 5f; 
    public float minHeight = 0f; 
    public Player player;
    private float direction = 1; // 1 for moving up, -1 for moving down
    private void Start()
    {
    }
    void Update()
    {
      
            transform.Translate(Vector3.up * speed * direction * Time.deltaTime);

            // Check if the object has reached the maximum height or minimum height, change direction accordingly
            if (transform.position.y >= maxHeight)
            {
                direction = -1; // Change direction to move down
            }
            else if (transform.position.y <= minHeight)
            {
                direction = 1; // Change direction to move up
            }
        
        if(!player.IsMoving) 
        {
            speed = 0;
        }
        else
        {
            speed = 0.45f;
        }
       
    }
}
