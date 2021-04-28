using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorConditions : MonoBehaviour
{
    [Header("Points Conditions")]
    public int points;
    [SerializeField] int maxPoints;

    [Header("Color Balls Conditions")]
    public int colorBalls;
    [SerializeField] int maxColorBalls;

    [Header("Pressure Button and single button Conditions")]
    public int pressureButtons;
    [SerializeField] int maxPressureButoons;
    public bool singleButton;

    public bool canExit;

    //LevelSpawnTrigger spawnTrigger;

    // Start is called before the first frame update
    void Start()
    {
        canExit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(points == maxPoints && colorBalls == maxColorBalls && pressureButtons == maxPressureButoons && singleButton)
        {
            canExit = true;
        }
        else
        {
            canExit = false;
        }
    }
}
