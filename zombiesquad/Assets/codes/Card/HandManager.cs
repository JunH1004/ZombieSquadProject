using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    public List<Card> hand = new List<Card>();
    void addCard(Card card) {
        hand.Add(card);
    }
    void removeCard(Card card) {
        hand.Remove(card);
    }
    void playCard(Card card) {
        // 카드 플레이
    }
}
