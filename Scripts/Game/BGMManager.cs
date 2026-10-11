using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    // 싱글톤
    public static BGMManager instance;

    // 오디오 소스
    [HideInInspector] public AudioSource audioSource;

    public AudioClip title;              // 타이틀
    public AudioClip intro;              // 인트로
    public AudioClip palletTown;         // 마을
    public AudioClip doctor_O_Lab;       // 오박사 연구소
    public AudioClip route;              // 숲 길
    public AudioClip city;               // 도시
    public AudioClip pokemonCenterMart;  // 포켓몬 센터 & 마트
    public AudioClip battle;             // 전투
    public AudioClip battleWin;          // 전투 승리

    // 바꿀 클립
    AudioClip changeClip;

    // 전투 중
    bool isBattle = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;

            SceneManager.sceneLoaded -= StartScene;
            SceneManager.sceneLoaded += StartScene;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 외부에서 호출하여 배경음 수동 전환
    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayBattleBGM()
    {
        isBattle = true;
        PlayBGM(battle);
    }

    public void PlayBattleWinBGM()
    {
        isBattle = true;
        PlayBGM(battleWin);
    }

    public void EndBattleBGM()
    {
        isBattle = false;
        PlayBGM(changeClip); // 이전 BGM으로 복귀
    }

    void StartScene(Scene scene, LoadSceneMode mode)
    {
        if (isBattle) return; // 전투 중이면 씬 로딩에도 변경 안 함

        int index = scene.buildIndex;

        switch (index)
        {
            // 타이틀
            case 0:
                changeClip = title;
                break;

            // 인트로
            case 1:
                changeClip = intro;
                break;

            // 마을 / 플레이어 집
            case 2:
            case 3:
            case 4:
                changeClip = palletTown;
                break;

            // 오박사 연구소
            case 5:
                changeClip = doctor_O_Lab;
                break;

            // 숲 길 / 통로
            case 6:
            case 7:
            case 8:
                changeClip = route;
                break;

            // 도시
            case 9:
                changeClip = city;
                break;

            // 포켓몬 센터 / 마트
            case 10:
            case 11:
                changeClip = pokemonCenterMart;
                break;

            default:
                changeClip = null;
                break;
        }

        if (changeClip != null)
        {
            PlayBGM(changeClip);
        }
    }
}
