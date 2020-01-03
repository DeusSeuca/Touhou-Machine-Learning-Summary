using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector3 force;
    public Vector3 pos;
    JointMotor a = new JointMotor();
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<HingeJoint>().motor;
         //GetComponent<HingeJoint>().motor.force=10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("ss");
            a.force = -a.force;
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<SpringJoint>().maxDistance *= 1.1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<SpringJoint>().maxDistance *= 0.9f;
        }
    }

}
