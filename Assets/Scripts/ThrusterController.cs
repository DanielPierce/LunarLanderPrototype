using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{

    public Rigidbody lander;
    [Header("Thrust in newtons")]
    public int thrust;
    [Header("Experienced gravity in m/s/s")]
    public float gravity;
    
    private bool isPaused = false;

    private float yVelocity;
    private float yForce;

    private bool hasLanded = false;
    private bool hasCrashed = false;

    private int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        lander.drag = 0;
    }

    // Fixed update is called every physics step
    void FixedUpdate()
    {
        // Add affects of gravity
        lander.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        // Calculate force of gravity on lander
        yForce = -1 * gravity * lander.mass;

        // If the up arrow is down, apply an impulse this timestep
        if(Input.GetKey(KeyCode.UpArrow))
        {
            // Thrust is newtons, multiply by time since last physics step to get newton-seconds
            lander.AddForce(Vector3.up * thrust * Time.deltaTime, ForceMode.Impulse);
            // If thrusting, add thrust to net force
            yForce += thrust;
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            lander.AddForce(Vector3.left * thrust * Time.deltaTime, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lander.AddForce(Vector3.right * thrust * Time.deltaTime, ForceMode.Impulse);
        }

        // Record the current velocity of the lander
        yVelocity = lander.velocity.y;
        // If the velocity is very very small, set it to zero
        if(Mathf.Abs(yVelocity) < 0.001f)
        {
            yVelocity = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the space key was pressed this frame
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Pause or unpause
            isPaused = !isPaused;
            if(isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                //Can probably put fudge factor here, set time to like 0.8f instead of 1
                Time.timeScale = 1;
            }
        }
    }

    // OnGUI is called every time the GUI is updated
    // (which I think is every frame but don't quote me on that)
    void OnGUI()
    {
        GUI.Label(new Rect(20, 40, 250, 20), "Y Velocity: " + yVelocity + " m/s");
        GUI.Label(new Rect(20, 60, 250, 20), "Y Net Force: " + yForce + " N");
        if(hasCrashed)
        {
            GUI.Label(new Rect(20, 80, 250, 20), "You've crashed");
        }
        if (hasLanded)
        {
            GUI.Label(new Rect(20, 100, 250, 20), "Sucessfully landed: " + points + " points");
        }
    }

    void OnCollisionEnter(Collision targetObj)
    {
        if (targetObj.gameObject.tag == "Goal")
        {
            hasLanded = true;
            points = int.Parse(targetObj.gameObject.name);
        }
        if (targetObj.gameObject.tag == "Hazard")
        {
            hasCrashed = true;
            points = 0;
        }
    }
}
