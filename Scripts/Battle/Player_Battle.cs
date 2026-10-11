using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Battle : MonoBehaviour
{
    // 이동 시작
    bool startMove;

    // 몬스터볼
    public GameObject monsterBall;

    void Update()
    {
        // 왼쪽으로 이동하는 연출
        if (startMove)
        {
            transform.localPosition -= new Vector3(500 * Time.deltaTime, 0);
        }

        // 이동 후 비활성화
        if (transform.localPosition.x <= -550.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    // 애니메이션에서 호출
    public void StartMove()
    {
        startMove = true;
    }

    // 애니메이션에서 호출
    public void MonsterBallAnim()
    {
        monsterBall.SetActive(true);
    }
}
