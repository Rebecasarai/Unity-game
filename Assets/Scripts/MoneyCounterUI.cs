﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour {

    [SerializeField]
    private Text moneyText;

    void Awake() {
        moneyText = GetComponent<Text>();
    }

    void Update() {
        moneyText.text = "MONEY:  " + GameMaster.Money.ToString();
    }
}
