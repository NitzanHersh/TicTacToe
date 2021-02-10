using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public int Score;
    public int HighestScore;

    public string HighestScoreString;
    // Start is called before the first frame update
    void Start()
    {
        // Get highest score from server
        
        StartCoroutine("GetText");
        //Invoke("DoPost", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Score++;
            Debug.Log("My Score is" + " " + Score + " " + "And the previous Highest Score was" +" "+ HighestScoreString);
        }

        if (Score > HighestScore)
        {
            HighestScore = Score;
            HighestScoreString = HighestScore.ToString();
            Invoke("DoPost", 0.1f);
        }

        
    }

    IEnumerator GetText()
    {
        UnityWebRequest www =
            UnityWebRequest.Get("https://cooprougelike.herokuapp.com/getData.php");
        //UnityWebRequest.Get("https://checkip.amazonaws.com/");

        Debug.Log("Hey! Wait a minute ya filth!");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
            Debug.Log("New Highest Score Recived!" + " " + www.downloadHandler.text);
            HighestScoreString = www.downloadHandler.text;
            HighestScore = int.Parse(HighestScoreString);
            Debug.Log("i did GetText");
        }

    }

    void DoPost()
        {
          StartCoroutine("Upload");
        }

    IEnumerator Upload()
        {
          List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        //form.Add(new MultipartFormDataSection("data", "This is akward, did I just hack (?) Nitzan's server?"));
        form.Add(new MultipartFormDataSection("data", HighestScoreString));

        using (UnityWebRequest www =
                UnityWebRequest.Post("" + "https://cooprougelike.herokuapp.com/postData.php", form))
                //UnityWebRequest.Post("" + "https://unityeran.herokuapp.com/postData.php", form))

        {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log("Y'er Stupid File Has Been Uplowded");
                    }
                }
            }
}
    
