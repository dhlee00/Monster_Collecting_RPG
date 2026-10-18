using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBattleScene : MonoBehaviour
{
    // 애니메이션 끝나면 호출
    public void EndAnim()
    {
        // 배틀씬 로드
        SceneManager.LoadScene("BattleScene");
    }
}
