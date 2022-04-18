using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform; //O objeto que a câmara irá seguir
    public Transform cameraPivotTransform;     //O objeto que a câmara usa como referência (Olhar para cima e para baixo)
    public Transform cameraTransform; //O transform do camera object na scene no Unity
    public LayerMask ignoreLayers; //As camadas que queremos com que a câmara colida
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraTransformPosition;

    public float followSpeed = 0.2f;
    public float lookSpeed = 2;
    public float pivotSpeed = 2;

    private float targetPosition;
    private float defaultPosition;
    public float lookAngle; //Olhar para cima e para baixo
    public float pivotAngle; //Olhar para a esquerda e para a direita
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;

    public void Awake()
    {
        targetTransform = FindObjectOfType<InputManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        transform.position = targetPosition;

        HandleCameraCollisions(delta);
    }

    public void HandleCameraRotaion(float delta, float mouseXInput, float mouseYInput)
    {
        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngle -= (mouseYInput * pivotSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition)
            , ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }
}
