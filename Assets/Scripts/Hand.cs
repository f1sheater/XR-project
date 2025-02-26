using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;
    public float speed;
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";

    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private Transform _followTarget;
    private Rigidbody _body;
    private SkinnedMeshRenderer _mesh;

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    void AnimateHand()
    {
        if (gripCurrent != gripTarget) {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }
        
        if (triggerCurrent != triggerTarget) {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }
    }

    private void PhysicsMove()
    {
        var positionWOffset = _followTarget.position + positionOffset;
        var distance = Vector3.Distance(positionWOffset, transform.position);
        _body.linearVelocity = (positionWOffset - transform.position).normalized * followSpeed * distance;

        var rotationWOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = angle * axis * Mathf.Deg2Rad * rotateSpeed;
    }

    public void ToggleVisibility()
    {
        _mesh.enabled = !_mesh.enabled;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;

        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;

        _mesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
        PhysicsMove();
    }
}

