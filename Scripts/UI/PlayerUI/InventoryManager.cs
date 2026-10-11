using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // 포커싱 중인 아이템 정보
    public Image itemInfoImg;
    public TextMeshProUGUI itemInfoName;
    public TextMeshProUGUI itemInfoTxt;

    // 아이템 리스트
    public Image[] itemList;

    public Image[] itemListImg;                  // 보유 아이템 이미지
    public TextMeshProUGUI[] itemListName;       // 보유 아이템 이름
    public TextMeshProUGUI[] itemListHaveCount;  // 아이템 보유 개수

    // 원래 아이템 리스트 이미지
    public Sprite normalImage;

    // 선택된 아이템 리스트 이미지
    public Sprite selectImage;

    void OnEnable()
    {
        // 가지고 있는 아이템 출력
        for (int i = 0; i < MyItem.item.Count; i++)
        {
            if (MyItem.item[i] != null)
            {
                itemListImg[i].enabled = true;
                itemListImg[i].sprite = MyItem.item[i].item_Img;
                itemListName[i].text = MyItem.item[i].item_Name;
                itemListHaveCount[i].text = $"보유 개수\n{MyItem.item[i].item_HaveCount}개";
            }

            else
            {
                itemListImg[i].enabled = false;
                itemListName[i].text = "";
                itemListHaveCount[i].text = "";
            }
        }
    }

    void Update()
    {
        SelectIndex();
    }

    public void SelectIndex()
    {
        // 0번 목록 선택된 상태
        if (itemList[0].sprite == selectImage)
        {
            ChangeImageKey(KeyCode.DownArrow, 0, 1);
            PrintInfo(0);
        }

        // 1번 목록 선택된 상태
        else if (itemList[1].sprite == selectImage)
        {
            ChangeImageKey(KeyCode.UpArrow, 1, 0);
            ChangeImageKey(KeyCode.DownArrow, 1, 2);
            PrintInfo(1);
        }

        // 2번 목록 선택된 상태
        else if (itemList[2].sprite == selectImage)
        {
            ChangeImageKey(KeyCode.UpArrow, 2, 1);
            ChangeImageKey(KeyCode.DownArrow, 2, 3);
            PrintInfo(2);
        }

        // 3번 목록 선택된 상태
        else if (itemList[3].sprite == selectImage)
        {
            ChangeImageKey(KeyCode.UpArrow, 3, 2);
            ChangeImageKey(KeyCode.DownArrow, 3, 4);
            PrintInfo(3);
        }

        // 4번 목록 선택된 상태
        else if (itemList[4].sprite == selectImage)
        {
            ChangeImageKey(KeyCode.UpArrow, 4, 3);
            PrintInfo(4);
        }

        // 모든 목록 선택 안된 상태
        else if (itemList[0].sprite != selectImage &&
                 itemList[1].sprite != selectImage &&
                 itemList[2].sprite != selectImage &&
                 itemList[3].sprite != selectImage &&
                 itemList[4].sprite != selectImage)
        {
            // 아무 방향키나 누르면 맨 위 선택된 상태
            ChangeImageKey(KeyCode.UpArrow, 0, 0);
            ChangeImageKey(KeyCode.DownArrow, 0, 0);
            PrintInfo(MyItem.item.Count);
        }
    }

    // 키보드로 조작시 이미지 변경
    void ChangeImageKey(KeyCode key, int off, int on)
    {
        if (Input.GetKeyDown(key))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Choose");

            itemList[off].sprite = normalImage;
            itemList[on].sprite = selectImage;
        }
    }

    // 선택된 목록 정보 출력
    void PrintInfo(int num)
    {
        if (num < MyItem.item.Count)
        {
            itemInfoImg.enabled = true;
            itemInfoImg.sprite = MyItem.item[num].item_Img;
            itemInfoName.text = MyItem.item[num].item_Name;
            itemInfoTxt.text = MyItem.item[num].item_Info;
        }

        else
        {
            itemInfoImg.enabled = false;
            itemInfoName.text = "";
            itemInfoTxt.text = "";
        }
    }

    // 이벤트 트리거에서 호출
    public void ChangeImage(Image img)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        img.sprite = selectImage;
    }

    public void RevertImage(Image img)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        img.sprite = normalImage;
    }
}
