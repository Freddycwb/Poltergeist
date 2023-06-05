using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Dog : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 direction;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAccel;

    private Transform myTransform;
    [SerializeField] private float rotateVel;

    [SerializeField] private float knocksToStop;
    private float currentKnocksToStop;

    [SerializeField] private float waitToRunTime;
    private float currentWaitToRunTime;

    [SerializeField] private GameObject Bottle;

    [SerializeField] private float rainTime;
    [SerializeField] private int bottlesPerRain;
    private float lastBottleTime;
    [SerializeField] private Vector2 rainTopLeft;
    [SerializeField] private Vector2 rainDownRight;

    [SerializeField] private GameObject bigBottle;
    private bool bigBottleSpawned;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator[] Heads;

    [SerializeField] private InvokeAfterTimer room;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector2(-1,-1);
        TryGetComponent(out myTransform);
        currentKnocksToStop = knocksToStop;
    }

    private void Update()
    {
        if (currentWaitToRunTime > 0)
        {
            float timePassed = waitToRunTime - currentWaitToRunTime;
            if (timePassed <= rainTime)
            {
                BottleRain();
            }
            else if (!bigBottleSpawned)
            {
                bigBottle.transform.SetParent(null);
                bigBottle.GetComponent<ObjectMovement>().enabled = true;
                bigBottle.transform.GetChild(0).gameObject.SetActive(true);
                bigBottleSpawned = true;
            }
            
            Wait();
        }
        else
        {
            HorizontalMove();
        }
    }

    private void HorizontalMove()
    {
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y)), Time.deltaTime * rotateVel);
        Vector3 goalVel = new Vector3(direction.normalized.x, 0, direction.normalized.y) * maxSpeed;
        Vector3 neededAccel = goalVel - rb.velocity;
        neededAccel -= Vector3.up * neededAccel.y;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        rb.AddForce(neededAccel, ForceMode.Impulse);
    }

    private void Knock()
    {
        currentKnocksToStop--;
        if (currentKnocksToStop <= 0)
        {
            animator.SetTrigger("Stun");
            foreach (var head in Heads)
            {
                head.SetTrigger("Stun");
            }
            myTransform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            currentWaitToRunTime = waitToRunTime;
        }
    }

    public void InvertDirectionX()
    {
        direction = new Vector2(direction.x * -1, direction.y);
        Knock();
    }

    public void InvertDirectionZ()
    {
        direction = new Vector2(direction.x, direction.y * -1);
        Knock();
    }

    private void Wait()
    {
        currentWaitToRunTime -= Time.deltaTime;
        if (currentWaitToRunTime <= 0)
        {
            animator.SetTrigger("Recover");
            foreach (var head in Heads)
            {
                head.SetTrigger("Recover");
            }
            currentKnocksToStop = knocksToStop;
            bigBottleSpawned = false;
        }
    }

    private void BottleRain()
    {
        float timePerBottle = rainTime / bottlesPerRain;
        float deltaBottleTime = Time.time - lastBottleTime;

        if (deltaBottleTime < timePerBottle)
        {
            return;
        }

        Instantiate(Bottle, new Vector3(Random.Range(rainTopLeft.x, rainDownRight.x), 40, Random.Range(rainDownRight.y, rainTopLeft.y)), Quaternion.Euler(new Vector3(Random.Range(-45,45), Random.Range(0, 360), Random.Range(-45, 45))));
        deltaBottleTime = Time.time;
    }

    public void SetBigBottlePos(float x)
    {
        StartCoroutine("WaitToSetBigBottlePos", x);
    }

    private IEnumerator WaitToSetBigBottlePos(float x)
    {
        yield return new WaitForSeconds(0.2f);
        bigBottle.transform.SetParent(transform);
        bigBottle.transform.localPosition = new Vector3(x, 13.4f, 0.5f);
    }

    public void Sleep()
    {
        animator.SetTrigger("Sleep");
        foreach (var head in Heads)
        {
            head.SetTrigger("Sleep");
        }
        room.enabled = true;
        enabled = false;
    }
}
