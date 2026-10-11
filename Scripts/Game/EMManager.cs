using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMManager : MonoBehaviour
{
    // 싱글톤
    public static EMManager instance;

    // 오디오 클립
    public AudioClip[] audioClip;

    // 오디오 소스 저장할 딕셔너리
    public Dictionary<string, AudioSource> audioSource = new Dictionary<string, AudioSource>();

    // 볼륨 조절용 리스트
    public List<AudioSource> audioSourceVolume = new List<AudioSource>();

    void Awake()
    {
        // 인스턴스에 값이 없으면 자기자신 할당
        if (instance == null)
        {
            instance = this;

            // 사라지지 않음
            DontDestroyOnLoad(gameObject);
        }

        // 인스턴스에 값이 있으면 삭제
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var clip in audioClip)
        {
            // 오디오 소스 생성
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.playOnAwake = false;

            // 이름으로 딕셔너리, 리스트에 저장
            audioSource[clip.name] = source;
            audioSourceVolume.Add(source);
        }
    }

    // 효과음 재생
    public void PlayEM(string name, int repeatCount = 1, float repeatTime = 10.0f)
    {
        StartCoroutine(PlayEMRoutine(name, repeatCount, repeatTime));
    }

    private IEnumerator PlayEMRoutine(string name, int repeatCount, float repeatTime)
    {
        while (repeatCount-- > 0)
        {
            if (audioSource.TryGetValue(name, out var source))
            {
                source.Play();
            }

            yield return new WaitForSeconds(repeatTime);
        }
    }

    // 볼륨 조절
    public void SetVolume(float value)
    {
        for (int i = 0; i < audioSourceVolume.Count; i++)
        {
            audioSourceVolume[i].volume = value;
        }
    }
}
