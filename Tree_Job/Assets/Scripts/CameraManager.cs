using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
/// <summary>
/// Script placed on Camera to move it from tree to tree
/// </summary>
public class CameraManager : MonoBehaviour
{
    float timer;
    readonly float errorMargin = 0.02f;
    readonly float movementSpeed = 0.01f;

    Transform cameraTransform;
    private void Start()
    {
        cameraTransform = gameObject.transform;

        EventBroker.OnTreeChanging += MoveCamera;
        EventBroker.OnGameEnded += OnGameEnded;
    }

    void MoveCamera(Vector3 distanceToNextTree)
    {
        StartCoroutine(MoveCameraCoroutine(distanceToNextTree));
    }

    IEnumerator MoveCameraCoroutine(Vector3 distanceToNextTree)
    {
        Vector3 targetPosition = cameraTransform.position + distanceToNextTree;
        timer = 0;

        while (Vector3.Distance(cameraTransform.position, targetPosition) > errorMargin)
        {
            timer += Time.deltaTime * movementSpeed;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, timer);

            yield return null;
        }
        EventBroker.CallOnAnimationEnded();
    }

    void OnGameEnded()
    {
        EventBroker.OnTreeChanging -= MoveCamera;
        EventBroker.OnGameEnded -= OnGameEnded;
    }
}
