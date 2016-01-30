using UnityEngine;
using System.Collections;

public class InGameCameraController : MonoBehaviour
{
    private float LookAtSpeed = 1.5f;

    private float ZoomSpeed = 5;
	private float ZoomTarget;

    bool switchFocus;
    bool Zooming, Shaking;

    private Transform LookAtTarget;

    private Transform targetPosition;
    
	private float startingFieldOfView, lookAtSpeed;

    private Vector3 normalPosition;

	private float ShakeForce,targetShakeForce,  targetZoom, moveToSpeed ;

    void Start()
    {
        normalPosition = transform.position;
        startingFieldOfView = Camera.main.fieldOfView;
    }

    void Update()
    {
        if (LookAtTarget != null)
        {
            Vector3 relFocusPosition = LookAtTarget.position - transform.position;
            Quaternion lookAtRotation = Quaternion.LookRotation(relFocusPosition, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, LookAtSpeed * Time.deltaTime);
        }

        if (Zooming != false)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, ZoomTarget, Time.deltaTime * ZoomSpeed);
        }

        if (targetPosition != null)
        {
			normalPosition = Vector3.Lerp(normalPosition, targetPosition.position, Time.deltaTime * moveToSpeed );
        }

        ShakeForce = Mathf.Lerp(ShakeForce, targetShakeForce, Time.deltaTime);       
        transform.position = normalPosition + (Random.insideUnitSphere * ShakeForce);
    }

    public void RotateCamera(Transform target)
    {
        Vector3 relativePosition = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = rotation;
    }

    public void LookAt(Transform target, float lookAtSpeed)
    {
		this.LookAtSpeed = lookAtSpeed;
        LookAtTarget = target;
    }

    public void Zoom(float targetZoomLevel, float targetZoomSpeed)
    {
        Zooming = true;
        ZoomSpeed = targetZoomSpeed;
        ZoomTarget = targetZoomLevel;
    }

    public void Shake(float targetShakeForce)
    {
        this.targetShakeForce = targetShakeForce;
    }

    public void StopShake()
    {
        targetShakeForce = 0;
    }

    public void MoveToPosition(Transform targetPosition, float speed)
    {
		moveToSpeed = speed;
        this.targetPosition = targetPosition;
    }

    public void resetZoom(float speed)
    {
        Zoom(startingFieldOfView,speed);
    }
}
