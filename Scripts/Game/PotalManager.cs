using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalManager : MonoBehaviour
{
    // 페이드
    GameObject fade;

    void Start()
    {
        fade = GameManager.instance.canvas.transform.Find("Fade").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 닿은 경우
        if (collision.gameObject.layer == 3)
        {
            // 전에 있던 곳 저장
            GameManager.instance.pastSceneNum = GameManager.instance.sceneNum;

            // 포탈의 이름에 따라 씬 전환
            switch (this.gameObject.name)
            {
                // 플레이어 집 2층
                case "Potal_PlayerHouse_2F":
                    GameManager.instance.sceneNum = 2;
                    fade.SetActive(true);
                    break;

                // 플레이어 집 1층
                case "Potal_PlayerHouse_1F":
                    GameManager.instance.sceneNum = 3;
                    fade.SetActive(true);
                    break;

                // 마을
                case "Potal_PalletTown":
                    GameManager.instance.sceneNum = 4;
                    fade.SetActive(true);
                    break;

                // 오박사 연구소
                case "Potal_Doctor_O_Lab":
                    GameManager.instance.sceneNum = 5;
                    fade.SetActive(true);
                    break;

                // 숲 길 1
                case "Potal_Route_1":
                    if (GameManager.instance.tutorialOver)
                    {
                        GameManager.instance.sceneNum = 6;
                        fade.SetActive(true);
                    }
                    break;

                // 숲 길 2
                case "Potal_Route_2":
                case "Potal_OutGate_0":
                case "Potal_OutGate_1":
                    GameManager.instance.sceneNum = 7;
                    fade.SetActive(true);
                    break;

                // 통로
                case "Potal_Gate_0":
                case "Potal_Gate_1":
                    GameManager.instance.sceneNum = 8;
                    fade.SetActive(true);
                    break;

                // 도시
                case "Potal_City":
                    GameManager.instance.sceneNum = 9;
                    fade.SetActive(true);
                    break;

                // 마트
                case "Potal_Mart":
                    GameManager.instance.sceneNum = 10;
                    fade.SetActive(true);
                    break;

                // 포켓몬 센터
                case "Potal_PokemonCenter":
                    GameManager.instance.sceneNum = 11;
                    fade.SetActive(true);
                    break;
            }

            if (this.gameObject.name == "Potal_Route_1" &&
                !GameManager.instance.tutorialOver)
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Warning");
            }

            else
            {
                // 효과음 재생
                EMManager.instance.PlayEM("SceneMove");
            }
        }
    }
}
