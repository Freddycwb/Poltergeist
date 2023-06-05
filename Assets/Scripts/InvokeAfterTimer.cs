using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAfterTimer : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private float timeToAction;
    [SerializeField] private GameObject[] instantiateOnAction;
    [SerializeField] private bool destroyOnDisable = true;

    private void OnEnable()
    {
        if (timeToAction > 0)
        {
            StartCoroutine("InvokeAfterSeconds");
        }
    }

    private IEnumerator InvokeAfterSeconds()
    {
        yield return new WaitForSeconds(timeToAction);
        InstantiateGameObjects(instantiateOnAction);
        action.Invoke();
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
