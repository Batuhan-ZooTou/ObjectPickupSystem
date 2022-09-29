using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private float pickUpDistance;
    [SerializeField] private float grabPointSpeed;
    [SerializeField] private CapsuleCollider boxCollider;

    private Vector3 defaultObjectGrabPointTransform;
    private ObjectGrabbable objectGrabbable;

    // Update is called once per frame
    void Update()
    {
        defaultObjectGrabPointTransform = playerCameraTransform.position + playerCameraTransform.forward*1.15f;
        if (Input.GetMouseButtonDown(0))
        {
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        Physics.IgnoreCollision(objectGrabbable.GetComponent<Collider>(), boxCollider, true);
                        objectGrabPointTransform.position = defaultObjectGrabPointTransform;
                        objectGrabbable.Grab(objectGrabPointTransform);
                        Debug.Log(objectGrabbable.transform.name);
                    }
                }

            }
            else
            {
                Physics.IgnoreCollision(objectGrabbable.GetComponent<Collider>(), boxCollider, false);
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (objectGrabbable != null)
            {
                Physics.IgnoreCollision(objectGrabbable.GetComponent<Collider>(), boxCollider, false);
                objectGrabbable.Drop();
                objectGrabbable.ThrowObject(playerCameraTransform);
                Debug.Log("throw");
                objectGrabbable = null;
            }
        }
        if (objectGrabbable != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && Vector3.Distance(objectGrabPointTransform.position, defaultObjectGrabPointTransform)<2f) // forward
            {
                objectGrabPointTransform.position += playerCameraTransform.forward * Time.deltaTime * grabPointSpeed;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Vector3.Distance(objectGrabPointTransform.position, defaultObjectGrabPointTransform) > 0.13f) // backwards
            {
                objectGrabPointTransform.position -= playerCameraTransform.forward * Time.deltaTime * grabPointSpeed;
            }
        }
    }
}
