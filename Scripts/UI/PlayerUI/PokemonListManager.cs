using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PokemonListManager : MonoBehaviour
{
    [Header("Object")]  // 오브젝트들

    // 옵션 창
    public GameObject optionView;

    // 선택 아이콘
    public GameObject[] selectIcon;

    // 포켓몬 정보 창
    public PokemonInfoManager pim;

    // 포켓몬 기술 창
    public SkillInfoManager sim;

    [Header("Print Pokemon")]  // 출력 관련

    // 포켓몬 아이콘
    public PokemonIcon[] pokemonIcon;

    // 이름
    public TextMeshProUGUI[] nameTxt;

    // 레벨
    public TextMeshProUGUI[] levelTxt;

    // 체력
    public Image[] hpBar;
    public TextMeshProUGUI[] hpTxt;

    [Header("Image")]  // 이미지 관련

    // 파트너 이미지
    public Image nowPartnerImg;

    // 포켓몬 리스트 이미지
    public Image[] listImage;

    // 선택 되지 않은 파트너 이미지
    public Sprite normalPartnerImg;

    // 선택 된 파트너 이미지
    public Sprite selectPartnerImg;

    // 선택 되지 않은 이미지
    public Sprite normalListImg;

    // 선택 된 이미지
    public Sprite selectListImg;

    // 선택
    public bool selectIndex = false;
    public bool canSelectOption = false;

    private void OnEnable()
    {
        for (int i = 0; i < 6; i++)
        {
            ResetIndex(i);
        }

        for (int i = 0; i < MyPokemon.pokemon.Count; i++)
        {
            PrintIndex(i);
        }
    }

    void Update()
    {
        // 선택하기 전
        if (!selectIndex)
        {
            optionView.SetActive(false);

            SelectIndex();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < selectIcon.Length; i++)
                {
                    selectIcon[i].SetActive(false);
                }

                selectIcon[0].SetActive(true);

                if (nowPartnerImg.sprite == selectPartnerImg)
                {
                    SelectPokemonIndex(0);
                    return;
                }

                for (int i = 0; i < listImage.Length; i++)
                {
                    if (listImage[i].sprite == selectListImg)
                    {
                        SelectPokemonIndex(i + 1);
                        break;
                    }
                }
            }
        }

        // 선택 후
        else if (selectIndex)
        {
            optionView.SetActive(true);

            // 한프레임 쉬기 (창 바로 열리는 문제 때문에)
            StartCoroutine(WaitFrame());

            if (canSelectOption)
            {
                SelectOption();
            }
        }
    }

    #region 포켓몬 목록 출력
    void PrintIndex(int i)
    {
        if (MyPokemon.pokemon[i] != null)
        {
            pokemonIcon[i].gameObject.SetActive(true);
            pokemonIcon[i].IconImg[0] = MyPokemon.pokemon[i].icon[0];
            pokemonIcon[i].IconImg[1] = MyPokemon.pokemon[i].icon[1];

            nameTxt[i].text = MyPokemon.pokemon[i].pokemonName;
            levelTxt[i].text = $"Lv {MyPokemon.pokemon[i].level}";
            hpBar[i].fillAmount = MyPokemon.pokemon[i].nowHP / MyPokemon.pokemon[i].maxHP;
            hpTxt[i].text = $"{Mathf.RoundToInt(MyPokemon.pokemon[i].nowHP)} / {Mathf.RoundToInt(MyPokemon.pokemon[i].maxHP)}";
        }
    }

    // 초기화
    void ResetIndex(int i)
    {
        pokemonIcon[i].gameObject.SetActive(false);

        nameTxt[i].text = "";
        levelTxt[i].text = "";
        hpBar[i].fillAmount = 1.0f;
        hpTxt[i].text = "";
    }
    #endregion

    #region 목록 관련
    // 목록 선택
    public void SelectIndex()
    {
        // 파트너 포켓몬
        if (nowPartnerImg.sprite == selectPartnerImg)
        {
            ChangeImageKey(KeyCode.DownArrow, -1, 0);
        }

        // 0번 목록 선택된 상태
        else if (listImage[0].sprite == selectListImg)
        {
            ChangeImageKey(KeyCode.UpArrow, 0, -1);
            ChangeImageKey(KeyCode.DownArrow, 0, 1);
        }

        // 1번 목록 선택된 상태
        else if (listImage[1].sprite == selectListImg)
        {
            ChangeImageKey(KeyCode.UpArrow, 1, 0);
            ChangeImageKey(KeyCode.DownArrow, 1, 2);
        }

        // 2번 목록 선택된 상태
        else if (listImage[2].sprite == selectListImg)
        {
            ChangeImageKey(KeyCode.UpArrow, 2, 1);
            ChangeImageKey(KeyCode.DownArrow, 2, 3);
        }

        // 3번 목록 선택된 상태
        else if (listImage[3].sprite == selectListImg)
        {
            ChangeImageKey(KeyCode.UpArrow, 3, 2);
            ChangeImageKey(KeyCode.DownArrow, 3, 4);
        }

        // 4번 목록 선택된 상태
        else if (listImage[4].sprite == selectListImg)
        {
            ChangeImageKey(KeyCode.UpArrow, 4, 3);
        }

        // 모든 목록 선택 안된 상태
        else if (nowPartnerImg.sprite != selectPartnerImg &&
                 listImage[0].sprite != selectListImg &&
                 listImage[1].sprite != selectListImg &&
                 listImage[2].sprite != selectListImg &&
                 listImage[3].sprite != selectListImg &&
                 listImage[4].sprite != selectListImg)
        {
            // 아무 방향키나 누르면 파트너 선택된 상태
            ChangeImageKey(KeyCode.UpArrow, -1, -1);
            ChangeImageKey(KeyCode.DownArrow, -1, -1);
        }
    }

    // 키보드로 조작시 이미지 변경
    void ChangeImageKey(KeyCode key, int off, int on)
    {
        if (Input.GetKeyDown(key))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Choose");

            if (off != -1) listImage[off].sprite = normalListImg;
            else nowPartnerImg.sprite = normalPartnerImg;

            if (on != -1) listImage[on].sprite = selectListImg;
            else nowPartnerImg.sprite = selectPartnerImg;
        }
    }

    // 이벤트 트리거에서 호출
    public void ChangeImage(Image img)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        nowPartnerImg.sprite = normalPartnerImg;
        for (int i = 0; i < listImage.Length; i++)
        {
            listImage[i].sprite = normalListImg;
        }

        // 이미지 변경
        if (img == nowPartnerImg)
        {
            img.sprite = selectPartnerImg;
        }

        else
        {
            img.sprite = selectListImg;
        }
    }

    // 선택한 포켓몬 정보 전달
    public virtual void SelectPokemonIndex(int num)
    {
        if (MyPokemon.pokemon.Count >= num + 1)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            // 선택한 목록 빼고 선택 안된 상태로 (마우스 클릭 때문에)
            if (num == 0)
            {
                for (int i = 0; i < listImage.Length; i++)
                {
                    listImage[i].sprite = normalListImg;
                }
            }

            else
            {
                nowPartnerImg.sprite = normalPartnerImg;

                for (int i = 0; i < listImage.Length; i++)
                {
                    if (i != num - 1)
                    {
                        listImage[i].sprite = normalListImg;
                    }
                }
            }

            selectIndex = true;
            pim.aboutPokemon = MyPokemon.pokemon[num];
            sim.aboutPokemon = MyPokemon.pokemon[num];
        }
    }
    #endregion

    #region 옵션 창
    // 옵션 이동
    void SelectOption()
    {
        // 정보
        if (selectIcon[0].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Choose");

                selectIcon[0].SetActive(false);
                selectIcon[1].SetActive(true);
            }

            // 선택하면 포켓몬 정보창 열기
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBtn(0);
            }
        }

        // 기술
        else if (selectIcon[1].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Choose");

                selectIcon[1].SetActive(false);
                selectIcon[0].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Choose");

                selectIcon[1].SetActive(false);
                selectIcon[2].SetActive(true);
            }

            // 선택하면 기술창 열기
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBtn(1);
            }
        }

        // 취소
        else if (selectIcon[2].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Choose");

                selectIcon[2].SetActive(false);
                selectIcon[1].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBtn(2);
            }
        }
    }

    // 버튼 이벤트에서 호출
    public void ClickBtn(int num)
    {
        canSelectOption = false;
        selectIndex = false;

        if (SceneManager.GetActiveScene().name != "BattleScene")
        {
            switch (num)
            {
                case 0:
                    // 효과음 재생
                    EMManager.instance.PlayEM("Select");
                    pim.gameObject.SetActive(true);
                    break;

                case 1:
                    // 효과음 재생
                    EMManager.instance.PlayEM("Select");
                    sim.gameObject.SetActive(true);
                    break;

                case 2:
                    // 효과음 재생
                    EMManager.instance.PlayEM("Cancle");
                    break;
            }
        }
    }

    // 이벤트 트리거에서 호출
    public void OnMouse(int num)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        for (int i = 0; i < selectIcon.Length; i++)
        {
            selectIcon[i].SetActive(false);
        }

        selectIcon[num].SetActive(true);
    }
    #endregion

    // 대기 함수
    public IEnumerator WaitFrame()
    {
        yield return new WaitForSeconds(0.1f);
        canSelectOption = true;
    }
}
