using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Vector3 moveVector;
    [SerializeField] float movePeriod = 2f; // 2 seconds

    [SerializeField] Vector3 euler;
    [SerializeField] float rotPeriod = 2f; // 2 seconds

    private Vector3 initPos;
    private Quaternion initRot;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        initRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
        RotationUpdate();
    }

    void MoveUpdate()
	{
        if (movePeriod == 0) return;
        float cycles = Time.time / movePeriod;
        const float tau = Mathf.PI * 2;
        float rawSInWave = Mathf.Sin(cycles * tau); // -1 ~ 1

        float moveFactor = (rawSInWave + 1f) / 2f; // 0 ~ 1
        Vector3 offset = moveVector * moveFactor;
        transform.position = initPos + offset;
    }
    void RotationUpdate()
	{
        if (rotPeriod == 0) return;
        float cycles = Time.time / rotPeriod;
        const float tau = Mathf.PI * 2;
        float rawSInWave = Mathf.Sin(cycles * tau); // -1 ~ 1

        float rotFactor = (rawSInWave + 1f) / 2f; // 0 ~ 1
        Quaternion newQuaternion = Quaternion.Euler(
            initRot.x + euler.x * rotFactor, 
            initRot.y + euler.y * rotFactor, 
            initRot.z + euler.z * rotFactor);
        transform.rotation = newQuaternion;
    }
}
