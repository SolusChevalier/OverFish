using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player_1 P1;
    public Player_2 P2;
    public int CostPerFish = 10;

    public TextMeshProUGUI P1FishTxt, P2FishTxt;

    public TextMeshProUGUI P1MoneyTxt, P2MoneyTxt;

    private void Update()
    {
        P1FishTxt.text = P1.FishCount.ToString() + " / " + P1.CargoSpace;
        P2FishTxt.text = P2.FishCount.ToString() + " / " + P2.CargoSpace;
        P1MoneyTxt.text = P1.Money.ToString();
        P2MoneyTxt.text = P2.Money.ToString();
    }
}