using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameValues {

    public float m_currentMoneyMade;
    public float m_currentCollection;
    public float m_currentBottleCapacity;
    public int m_tubeSpeed;
    public float m_currentBottleProfit;
    public float WaitTime;


    public GameValues(float m_currentMoneyMade, float m_currentCollection, float m_currentBottleCapacity, float m_currentBottleProfit, float WaitTime, int m_tubeSpeed)
    {
        this.m_tubeSpeed = m_tubeSpeed;
        this.m_currentMoneyMade = m_currentMoneyMade;
        this.m_currentCollection = m_currentCollection;
        this.m_currentBottleProfit = m_currentBottleProfit;
        this.m_currentBottleCapacity = m_currentBottleCapacity;
        this.WaitTime = WaitTime;
    }


    public bool StageSwipeHintShow = false;

}
