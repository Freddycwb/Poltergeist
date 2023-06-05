using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAfterCounter : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private float maxValue;
    [SerializeField] private float value;
    [SerializeField] private float minValue;
    [SerializeField] private GameObject[] instantiateOnAction;
    [SerializeField] private bool destroyOnDisable = true;

    private void Start()
    {
        value = maxValue;   
    }

    public void IncreaseValue(float a)
    {
        value = Mathf.Clamp(value + a, minValue, maxValue);
    }

    public void DecreaseValue(float a)
    {
        value = Mathf.Clamp(value - a, minValue, maxValue);
        if (value == minValue)
        {
            InstantiateGameObjects(instantiateOnAction);
            action.Invoke();
        }
    }

    private void InstantiateGameObjects(GameObject[] instantiate)
    {
        if (instantiate.Length > 0)
        {
            foreach (GameObject obj in instantiate) 
            {
                Instantiate(obj, transform.position, transform.rotation);
            }
        }
    }

    private void OnDisable()
    {
        if (destroyOnDisable) Destroy(gameObject);
    }
}
