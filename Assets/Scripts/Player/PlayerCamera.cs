using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private IInput _input;
    [SerializeField] private GameObjectVariable camera;

    private GameObject target;

    [SerializeField] private Object targetObject;
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float rotXRange;

    private float xAxis;

    private void OnEnable()
    {
        camera.Value = gameObject;
    }

    private void Start()
    {
        target = targetObject is GameObject ? (GameObject)targetObject : (targetObject as GameObjectVariable).Value;
        _input = target.GetComponent<IInput>();
    }

    private void Update()
    {
        LookTarget();
        //cameraSetting.m_YAxis.Value += -_input.look.x * sensitivity * Time.deltaTime;
    }

    private void LookTarget()
    {
        Vector3 diff = target.transform.position - transform.position;
        Vector3 diffy = diff - diff.y * Vector3.up;
        float rotY = 360 - Mathf.Atan2(diffy.z, diffy.x) * Mathf.Rad2Deg;

        Vector3 diffx = diff - diff.x * Vector3.right;
        float rotX = Mathf.Atan2(diffy.magnitude, diffx.y) * Mathf.Rad2Deg;

        rotX = Mathf.Clamp(rotX, -rotXRange/2 - rotationOffset.x, rotXRange/2 - rotationOffset.x);

        Vector3 rotation = new Vector3(rotX, rotY, transform.eulerAngles.z) + rotationOffset;

        transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
