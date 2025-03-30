using UnityEngine;
using UnityEngine.UI;
public enum CardType {
    Unit, Facility, Utility, Enhancement, Research
}
public class Card : MonoBehaviour {
    public GameObject zonePrefab; // 해당 카드가 드래그될 때 사용할 프리팹 (농장 등)
    public string cardName; // 카드 이름 (농장 등)
    public Sprite cardImage; // 카드 이미지
    public string description; // 카드 설명
    public CardType cardType; // 카드 종류
}
