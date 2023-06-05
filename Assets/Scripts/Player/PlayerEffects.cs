using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffects : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Animator glasses;
    [SerializeField] private Animator body;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DamageAnimation()
    {
        animator.SetTrigger("Damage");
        glasses.SetTrigger("Damage");
        body.SetTrigger("Damage");
    }
}
