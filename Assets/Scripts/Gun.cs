using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private GameObject laser;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference quitAction;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private GameObject winText;
    private Transform _followTarget;
    private Rigidbody _body;
    private RaycastHit hit;
    private GameObject hitTarget;
    private int hitAmount;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootAction.action.Enable();
        quitAction.action.Enable();

        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;

        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsMove();

        if (shootAction.action.WasPressedThisFrame())
        {
            hitSound.Play();
            if (Physics.Raycast(laser.transform.position, laser.transform.forward, out hit, 100, 1 << 3))
            {
                hitTarget = hit.transform.gameObject;
                hitTarget.SetActive(false);
                hitAmount++;
                if (hitAmount >= 10)
                {
                    winText.SetActive(true);
                }
            }
        }

        if (quitAction.action.WasPressedThisFrame())
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
