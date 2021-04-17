using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMP_Text TimeText;

    private float TimeGoal = 0;
    private bool IsEnabled = false;
    private float ElapsedTime = 0;

    [SerializeField]
    HackingPanel HackingGame;
    
    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled) return;
        ElapsedTime += Time.deltaTime;

        if (ElapsedTime >= TimeGoal)
        {
            HackingGame.GameLose();
        }
        TimeText.text = ((int)(TimeGoal - ElapsedTime)).ToString();

    }

    public void SetTimer(float time)
    {
        TimeGoal = time;
        ElapsedTime = 0;
        IsEnabled = true;
    }

    public void StopTimer()
    {
        IsEnabled = false;
    }

    public void ReduceElapsedTime(float timeAmount)
    {
        ElapsedTime -= timeAmount;

        if (ElapsedTime <0)
        {
            ElapsedTime = 0;
        }
    }
}
