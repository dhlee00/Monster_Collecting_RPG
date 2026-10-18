using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    // 아이템 가져오기
    Item[] sellItem = new Item[2];

    // 플레이어 소지금
    public TextMeshProUGUI playerMoneyTxt;

    // 선택한 아이템 정보 출력
    public Image selectItemImg;
    public TextMeshProUGUI selectItemInfoTxt;

    // 구매 확인 창
    public GameObject buyView;

    // 구매 확인 창 아이콘
    public GameObject[] selectIcon;

    // 아이템 리스트
    public Image[] itemList;

    // 아이템 리스트 이미지
    public Sprite ItemListimg;
    public Sprite ItemListimg_select;

    // 구매할 아이템
    int buyingItem;

    // 구매 확정
    public bool buyViewOpen;

    void OnEnable()
    {
        GameManager.instance.cantDoing = true;

        PrintMoney();

        // 처음 상점창 열리면 가장 첫번째 목록 선택된 상태
        itemList[1].sprite = ItemListimg;
        itemList[0].sprite = ItemListimg_select;
    }

    void Start()
    {
        sellItem[0] = DataLoader.instance.GetItem("몬스터볼");
        sellItem[1] = DataLoader.instance.GetItem("상처약");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBtnClick();
        }

        if (!buyViewOpen)
        {
            SelectIndex();
            PrintInfo();
        }

        else if (buyViewOpen)
        {
            BuyView();
        }
    }

    public void SelectIndex()
    {
        // 0번 목록 선택된 상태
        if (itemList[0].sprite == ItemListimg_select)
        {
            ChangeImageKey(KeyCode.DownArrow, 0, 1);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SelectItem(0);
            }
        }

        // 1번 목록 선택된 상태
        else if (itemList[1].sprite == ItemListimg_select)
        {
            ChangeImageKey(KeyCode.UpArrow, 1, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SelectItem(1);
            }
        }
    }

    // 키보드로 조작시 이미지 변경
    void ChangeImageKey(KeyCode key, int off, int on)
    {
        if (Input.GetKeyDown(key))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Choose");

            itemList[off].sprite = ItemListimg;
            itemList[on].sprite = ItemListimg_select;
        }
    }

    // 선택된 아이템 정보 출력
    void PrintInfo()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i].sprite == ItemListimg_select)
            {
                selectItemImg.enabled = true;
                selectItemImg.sprite = sellItem[i].item_Img;
                selectItemInfoTxt.text = sellItem[i].item_Info;
                break;
            }

            else
            {
                selectItemImg.enabled = false;
                selectItemInfoTxt.text = "";
            }
        }
    }

    // 리스트 선택
    public void SelectItem(int num)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        if (!buyViewOpen)
        {
            buyingItem = num;
            buyView.SetActive(true);
            StartCoroutine(WaitFrame(true));
        }
    }

    // 구매 창
    void BuyView()
    {
        // 구매
        if (selectIcon[0].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeIcon(1);
            }

            // 선택하면 구매
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBuy(0);
            }
        }

        // 취소
        else if (selectIcon[1].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeIcon(0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBuy(1);
            }
        }
    }

    // 버튼 이벤트에서 호출
    public void ClickBuy(int num)
    {
        if (buyViewOpen)
        {
            // 구매
            if (num == 0)
            {
                // 돈이 충분한 경우
                if (sellItem[buyingItem].item_Prize <= GlobalValue.playerMoney)
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("Select");

                    MyItem.item[buyingItem].item_HaveCount++;
                    GlobalValue.playerMoney -= sellItem[buyingItem].item_Prize;
                    PrintMoney();
                    buyView.SetActive(false);
                    StartCoroutine(WaitFrame(false));
                }

                else
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("Warning");
                }
            }

            // 취소
            else if (num == 1)
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Cancle");

                buyView.SetActive(false);
                StartCoroutine(WaitFrame(false));
            }
        }
    }

    // 이벤트 트리거에서 호출
    public void ChangeImage(Image img)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        for (int i = 0; i < itemList.Length; i++)
        {
            itemList[i].sprite = ItemListimg;
        }

        img.sprite = ItemListimg_select;
    }

    public void ChangeIcon(int on)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        for (int i = 0; i < selectIcon.Length; i++)
        {
            selectIcon[i].SetActive(false);
        }

        selectIcon[on].SetActive(true);
    }

    // 돈 출력
    void PrintMoney()
    {
        if (GlobalValue.playerMoney <= 0)
        {
            playerMoneyTxt.text = "0원";
        }

        else
        {
            playerMoneyTxt.text = $"{GlobalValue.playerMoney:#,###}원";
        }
    }

    // 닫기 버튼
    public void CloseBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Cancle");

        GameManager.instance.cantDoing = false;
        gameObject.SetActive(false);
    }

    // 대기 함수
    public IEnumerator WaitFrame(bool onoff)
    {
        yield return new WaitForSeconds(0.2f);
        buyViewOpen = onoff;
    }
}
