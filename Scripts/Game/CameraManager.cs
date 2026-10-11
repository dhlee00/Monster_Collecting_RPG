using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 플레이어 트랜스폼
    Transform playerT;

    // 카메라 속도
    public float cameraSpeed;

    // 카메라 경계
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;

    void Start()
    {
        playerT = GameManager.instance.playerT;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(playerT.position.x, playerT.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed);
    }
}
