using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstAI : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    bool move;

    GameObject player;
    GameController gc;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.x * Mathf.Rad2Deg > 20f || transform.rotation.x * Mathf.Rad2Deg < -20f
            || transform.position.y < -50
            || transform.rotation.z * Mathf.Rad2Deg > 20f || transform.rotation.z < -20f || gc.GameState == "menu")
        {
            if (gc.GameState == "game")
            {
                Debug.Log(transform.localRotation.x);
                
                gc.score++;
            }
            transform.parent = gc.pool.transform;
            gameObject.SetActive(false);
        }

    }
    private void FixedUpdate()
    {
        float motor = gc.GameState == "game" ? -maxMotorTorque : 0;
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float steering = gc.GameState == "game"? -followTransform(player.transform) : 0;

        
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = maxSteeringAngle * steering;
                axleInfo.rightWheel.steerAngle = maxSteeringAngle * steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
        
        /*
        GetComponent<Rigidbody>().angularVelocity = maxSteeringAngle * steering * new Vector3(0, 1, 0);
        GetComponent<Rigidbody>().velocity = transform.forward * motor;
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
        */
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "ground")
        {
            if (gc.GameState == "game")
            {
                Debug.Log(transform.localRotation.x);
                
                gc.score++;
            }
            transform.parent = gc.pool.transform;
            gameObject.SetActive(false);
        }
    }
    float followTransform(Transform t)
    {
        Vector3 tp = transform.position - t.position;
        tp.Normalize();

        return Vector3.Cross(tp, transform.forward).y;
    }
}
