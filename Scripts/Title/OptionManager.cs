using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    // 배경음
    [Header("BGM")]
    public Slider bgmSlider;
    public TextMeshProUGUI bgmTxt;

    // 효과음
    [Header("EM")]
    public Slider emSlider;
    public TextMeshProUGUI emTxt;

    float lastValue = 10;

    private void OnEnable()
    {
        // 설정값 불러오기
        bgmSlider.value =BGMManager.instance.audioSource.volume;
        bgmTxt.text = $"{(bgmSlider.value * 100).ToString("F0")}%";

        emSlider.value = EMManager.instance.audioSourceVolume[0].volume;
        emTxt.text = $"{(emSlider.value * 100).ToString("F0")}%";
    }

    // 배경음 조절
    public void SettingBGM()
    {
        BGMManager.instance.audioSource.volume = bgmSlider.value;
        bgmTxt.text = $"{(bgmSlider.value * 100).ToString("F0")}%";
    }

    // 효과음 조절
    public void SettingEM()
    {
        EMManager.instance.SetVolume(emSlider.value);
        emTxt.text = $"{(emSlider.value * 100).ToString("F0")}%";

        // 볼륨 값이 10% 씩 변할 때 마다 효과음 재생
        if (Mathf.Abs(lastValue - Mathf.Ceil(emSlider.value * 10)) >= 1.0f)
        {
            lastValue = Mathf.Ceil(emSlider.value * 10);

            EMManager.instance.PlayEM("Select");
        }
    }

    // 닫기 버튼
    public void CloseBtn()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        gameObject.SetActive(false);
    }

    // 저장 버튼
    public void SaveBtn()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        // 현재 위치값 저장
        GameManager.instance.playerX = GameManager.instance.pm.transform.position.x;
        GameManager.instance.playerY = GameManager.instance.pm.transform.position.y;

        // 저장
        DataManager.Save();
    }

    // 타이틀 버튼
    public void TitleBtn()
    {
        gameObject.SetActive(false);
        GameManager.instance.pm.CloseBtnClick();
        GameManager.instance.sceneNum = -1;
        GameManager.instance.pastSceneNum = 0;
        GameManager.instance.canvas.transform.Find("Fade").gameObject.SetActive(true);
    }
}
