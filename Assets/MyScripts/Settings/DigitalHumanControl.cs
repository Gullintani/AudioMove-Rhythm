using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalHumanControl : MonoBehaviour

{
    public Transform target;

    private Animator animator;
    private AvatarIKGoal armIKGoal = AvatarIKGoal.LeftHand;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (target != null)
        {
            // Set IK and weight
            // animator.SetIKPositionWeight(armIKGoal, 1f);
            // animator.SetIKPosition(armIKGoal, target.position);
            // animator.SetIKRotationWeight(armIKGoal, 1f);
            // animator.SetIKRotation(armIKGoal, target.rotation);
        }
    }
}