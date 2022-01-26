using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductionLevelData{

    public float m_BeltSpeed;
    public float m_WaitTime;
    public float m_currentProfit;
    public float m_currentBottleCapacity;
    public float m_speed;
    public int m_currentProductionLevel;
    public float init_Multiplier;
    public float init_speed;
    public int averageBottlePerMinute;

    public ProductionLevelData(float m_BeltSpeed, float m_WaitTime, float m_currentProfit, float m_currentBottleCapacity,
                                float m_speed, int m_currentProductionLevel, float init_Multiplier, float init_speed, int averageBottlePerMinute)
    {
        this.m_BeltSpeed = m_BeltSpeed;
        this.m_WaitTime = m_WaitTime;
        this.m_currentProfit = m_currentProfit;
        this.m_currentBottleCapacity = m_currentBottleCapacity;
        this.m_speed = m_speed;
        this.m_currentProductionLevel = m_currentProductionLevel;
        this.init_Multiplier = init_Multiplier;
        this.init_speed = init_speed;
        this.averageBottlePerMinute = averageBottlePerMinute;
    }

}
