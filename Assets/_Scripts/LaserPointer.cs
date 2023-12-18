using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour
{
    [Header("Essential Settings")]
    [SerializeField]
    private Transform anchor;

    [SerializeField]
    private bool isRightHand;

    [Header("Render Settings")]
    [SerializeField]
    private float lineWidth;

    [SerializeField]
    private float lineMax;

    private LineRenderer _lineRenderer;
    private bool isPointing;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.widthMultiplier = lineWidth;
        _lineRenderer.enabled = false;
    }

    void Start()
    {
        isPointing = true;
        StartLaser();
    }

    public void StartLaser()
    {
        StartCoroutine(LaserCoroutine());
    }

    private IEnumerator LaserCoroutine()
    {
        _lineRenderer.enabled = true;

        while (isPointing)
        {
            //Vector3 anchorPosition = anchor.position;
            //Quaternion anchorRotation = anchor.rotation;
            //_lineRenderer.SetPosition(0, anchorPosition);

            //if (Physics.Raycast(new Ray(anchorPosition, anchorRotation * Vector3.left), out RaycastHit hit, lineMax))
            //{
            //    _lineRenderer.SetPosition(0, anchorPosition);
            //    _lineRenderer.SetPosition(1, hit.point);
            //}

            UpdateLength();
            

            yield return null;
        }

        _lineRenderer.enabled = false;
    }

    private void UpdateLength()
    {
        _lineRenderer.SetPosition(0, anchor.position);
        _lineRenderer.SetPosition(1, CalculateEndPoint());
    }

    private Vector3 CalculateEndPoint()
    {
        RaycastHit hit = CreatePointerRaycast();

        Vector3 endPosition = hit.collider ? hit.point : DefaultEnd(lineMax);
        return endPosition;
    }

    private RaycastHit CreatePointerRaycast()
    {
        Vector3 direction = isRightHand ? Vector3.right : Vector3.left;

        Physics.Raycast(new Ray(anchor.position, anchor.rotation * direction), out RaycastHit hit, lineMax);
        return hit;
    }

    private Vector3 DefaultEnd(float length)
    {
        Vector3 direction = isRightHand ? anchor.right : -anchor.right;

        return anchor.position + (direction * lineMax);
    }
}
