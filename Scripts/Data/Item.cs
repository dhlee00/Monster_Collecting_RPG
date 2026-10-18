using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Add Item/Item")]
public class Item : ScriptableObject
{
    // 아이템 이미지
    public Sprite item_Img;

    // 아이템 이름
    public string item_Name;

    // 설명
    [TextArea]
    public string item_Info;

    // 보유 개수
    public int item_HaveCount;

    // 아이템 가격
    public int item_Prize;
}
