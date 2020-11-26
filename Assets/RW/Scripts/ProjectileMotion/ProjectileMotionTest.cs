using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrajectoryPoints(Vector3 startPos, Vector2 direction)
    {
        //TODO: 
        // Velocity(u) = Mathf. Sqrt((x * x) + (y * y));
        // Angle = Mathf.Atan2(y, x);

        // Horizontal Motion : x acceleration = 0
        // Horizontal Motion : x velocity = Diagonal Velocity(u) cos (Theta)
        // Horizontal Motion : x direction Displacement = 

        // Vertical Motion : y acceleration = -Gravity(g) = -g
        // Vertical Motion : y velocity =  Gravity(g) * Time(t)
        // Vertical  Motion : y direction Displacement = 


        var dirX = direction.x;
        var dirY = direction.y;
        
        var veloc = Mathf.Sqrt((dirX * dirX) + (dirY * dirY));

        var angle = Mathf.Rad2Deg * (Mathf.Atan2(dirY, dirX));







    }
}
