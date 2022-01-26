using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public static AnimationHandler instance;

    public Animator pumpAnim;
    public Animator oil1, oil2, oil3, oil4;
    public Animator conveyor_Belt;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void speedUpAnimations()
    {
        oil1.speed = oil2.speed = oil3.speed = oil4.speed = 4;
    }

    public void speedDownAnimations()
    {
        oil1.speed = oil2.speed = oil3.speed = oil4.speed = 1;
    }
}
