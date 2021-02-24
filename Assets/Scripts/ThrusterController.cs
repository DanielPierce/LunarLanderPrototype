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

    // Start is called before the first frame update
    void Start()
    {
        lander.drag = 0;
    }

    void FixedUpdate()
    {
        lander.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        if(Input.GetKey(KeyCode.UpArrow))
        {
            lander.AddForce(Vector3.up * thrust * Time.deltaTime, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
