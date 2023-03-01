using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstCar : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] WheelCollider[] rearWheels;
    [SerializeField] WheelCollider[] frontWheels;

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    [SerializeField] FixedJoystick joy;

    Vector3 camDistance;

    GameController gc;
    public Transform follow;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        camDistance = new Vector3(
            transform.position.x - cam.transform.position.x,
            transform.position.y - cam.transform.position.y,
            transform.position.z - cam.transform.position.z
        );
    }
    private void Update()
    {
        //Debug.Log(transform.localRotation.x * Mathf.Rad2Deg);
        if (
            transform.rotation.x * Mathf.Rad2Deg > 20f || transform.rotation.x * Mathf.Rad2Deg < -20f
            || transform.position.y < -50
            || transform.rotation.z * Mathf.Rad2Deg > 20f || transform.rotation.z < -20f
        )
        {
            gc.GameState = "gameOver";
            transform.position = new Vector3(0, 10.1f, 0);
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }
        if (gc.GameState == "menu")
        {
            transform.position = new Vector3(0, 10.1f, 0);
            transform.rotation = Quaternion.identity;
        }
    }

    void LateUpdate()
    {
        //cam.transform.position = transform.position - camDistance;
    }
    private void FixedUpdate()
    {
        /*
        float motor = maxMotorTorque * -Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        */
        follow.position = new Vector3(
            joy.Direction.x * 5,
            transform.position.y,
            joy.Direction.y * 5
        );
        float motor = 0;
        //float steering = maxSteeringAngle * -followTransform(follow);
        if (gc.isPc)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) motor = -maxMotorTorque;
        }
        else
        {
            if (joy.Direction.x != 0 || joy.Direction.y != 0) motor = -maxMotorTorque;
        }
        /*
        if (joy.Direction.x != 0 || joy.Direction.y != 0)
        {
            if (Mathf.Abs(joy.Direction.x) > Mathf.Abs(joy.Direction.y))
            {
                if (follow.position.x/Mathf.Abs(follow.position.x) < joy.Direction.x) motor = maxMotorTorque;
                else motor = -maxMotorTorque;
            }
            if (Mathf.Abs(joy.Direction.y) > Mathf.Abs(joy.Direction.x))
            {
                if (follow.position.y / Mathf.Abs(follow.position.y) < joy.Direction.y) motor = maxMotorTorque;
                else motor = -maxMotorTorque;
            }
        }
        */

        //Debug.Log("s " + followTransform(follow));

        if (gc.GameState == "game")
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (gc.isPc)
                {
                    if (axleInfo.steering)
                    {
                        axleInfo.leftWheel.steerAngle = Input.GetAxis("Horizontal") * maxSteeringAngle;
                        axleInfo.rightWheel.steerAngle = Input.GetAxis("Horizontal") * maxSteeringAngle;
                    }
                    if (axleInfo.motor)
                    {
                        axleInfo.leftWheel.motorTorque = Input.GetAxis("Vertical") * -maxMotorTorque;
                        axleInfo.rightWheel.motorTorque = Input.GetAxis("Vertical") * -maxMotorTorque;
                    }
                }
                else
                {
                    if (axleInfo.steering)
                    {
                        axleInfo.leftWheel.steerAngle = joy.Direction.x * maxSteeringAngle;
                        axleInfo.rightWheel.steerAngle = joy.Direction.x * maxSteeringAngle;
                    }
                    if (axleInfo.motor)
                    {
                        axleInfo.leftWheel.motorTorque = joy.Direction.y * -maxMotorTorque;
                        axleInfo.rightWheel.motorTorque = joy.Direction.y * -maxMotorTorque;
                    }
                }
            }
        }
    }
    float followTransform(Transform t)
    {
        Vector3 tp = transform.position - t.position;
        tp.Normalize();

        return Vector3.Cross(tp, transform.forward).y;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "ground")
        {
            gc.GameState = "gameOver";
            transform.position = new Vector3(0, 10.1f, 0);
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }
    }
}
[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}