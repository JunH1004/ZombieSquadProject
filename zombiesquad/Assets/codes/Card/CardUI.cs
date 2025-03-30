using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour {
    public Card cardData;    // ScriptableObject 기반 카드 데이터
    public Image cardImage;  // 카드 이미지 UI (인스펙터에서 연결)

    void Start() {
        if (cardData != null && cardImage != null) {
            cardImage.sprite = cardData.cardImage;
        }
    }
}
