using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int Score = 0;
    public int HighestScore;

    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {

        /*if (Input.GetKeyDown("space"))
        {
            Score++;
            Debug.Log("My Score is" + " " + Score + " " + "And the previous Highest Score was" + " " + HighestScore);
        }

        if (Score > HighestScore)
        {
            HighestScore = Score;
            Invoke("UpdateHighestScore", 0.5f);

        }*/
    }
}
