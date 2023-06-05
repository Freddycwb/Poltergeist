using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAfterCollision : MonoBehaviour
{
    [SerializeField] private List<Collider> trigger = new List<Collider>();
    [SerializeField] private List<Collider> collision = new List<Collider>();
    [SerializeField] private UnityEvent action;
    [SerializeField] private GameObject[] instantiateOnAction;
    [SerializeField] private bool destroyOnDisable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (trigger.Count == 0) 
        {
            if (collision.Count == 0)
            {
                CollisionAction();
            }
            return;
        }
        if (trigger.Contains(other))
        {
            CollisionAction();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collision.Count == 0)
        {
            if (trigger.Count == 0)
            {
                CollisionAction();
            }
            return;
        }
        if (collision.Contains(other.collider))
        {
            CollisionAction();
        }
    }

    private void CollisionAction()
    {
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
