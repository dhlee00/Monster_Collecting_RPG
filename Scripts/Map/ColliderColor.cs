using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ColliderColor : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var colliders = GetComponentsInChildren<BoxCollider2D>();

        foreach (var box in colliders)
        {
            // 자식 오브젝트의 위치와 회전, 스케일을 적용
            Gizmos.matrix = box.transform.localToWorldMatrix;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box.offset, box.size);
        }
    }
}
