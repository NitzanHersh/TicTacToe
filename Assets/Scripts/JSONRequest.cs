using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JSONRequest : MonoBehaviour
{
    public string toSend;
    Board theBoard = new Board();

    bool gameOver = false;

    public char[] currentBoard = new char[9];
    public Text[] textArray = new Text[9];
    public Text turnIndicator;
    public Text message;

    //private int _acendingInt = 1;
    //private bool isPlayerTurn;

    //private bool _isFinished = false;
    private bool _player1Won = false;
    private bool _player2Won = false;

    void Start()
    {
        theBoard.board = currentBoard;
        InitBoard();
        //ShowBoard();
        theBoard.playerOneTurn = true;
        theBoard.playerTwoTurn = false;

        //theBoard.random = "";
        //isPlayerTurn = true;

        toSend = Serialize(theBoard);
        Invoke("DoPost", 0.1f);
        InvokeRepeating("DoGet", 0.5f, 1f);
        //InvokeRepeating("DoPost", 1f, 1.5f);



        //Invoke("DoGet", 0.1f);
        //theBoard = Deserialize(toSend);


    }

    void Update()
    {

        if (gameOver)
        {
            return;
        }

        if (theBoard.playerOneTurn)
        {
            turnIndicator.text = "Player 1 Turn";
            turnIndicator.color = new Color32(255, 0, 186, 255);

            for (int i = 0; i < textArray.Length; i++)
            {
                HandleKeyPress(i + 1);

            }

        }

        if (theBoard.playerTwoTurn)
        {  
            turnIndicator.text = "Player 2 Turn";
            turnIndicator.color = new Color32(255, 255, 0, 255);

            for (int i = 0; i < textArray.Length; i++)
            {
                HandleKeyPress(i + 1);

            }

        }


    }

    void InitBoard()
    {
        for (int i = 0; i < currentBoard.Length; i++)
        {
            currentBoard[i] = '_';
        }
    }

    /*void ShowBoard()
    {
        string boardStr = "";
        for (int i = 0; i < currentBoard.Length; i += 3)
        {
            boardStr += currentBoard[i] + " " +
                currentBoard[i + 1] + " " + currentBoard[i + 2] + "\n";
        }
        Debug.Log(boardStr);
    }*/

    void HandleKeyPress(int key)
    {
        if (theBoard.playerOneTurn)
        {
            if (Input.GetKeyDown(key.ToString()) && currentBoard[key - 1] == '_')
            {
                currentBoard[key - 1] = 'X';

                //currentBoard = theBoard.board;
                theBoard.playerOneTurn = false;
                theBoard.playerTwoTurn = true;
                
                //toSend = Serialize(theBoard);
                Invoke("DoPost", 0.1f);
                //Invoke("DoGet", 0.5f);
            }
 
        }

        if (theBoard.playerTwoTurn)
        {
            if (Input.GetKeyDown(key.ToString()) && currentBoard[key - 1] == '_')
            {
                currentBoard[key - 1] = 'O';

                //currentBoard = theBoard.board;
                
                theBoard.playerOneTurn = true;
                theBoard.playerTwoTurn = false;

                Invoke("DoPost", 0.1f);
                //Invoke("DoGet", 0.5f);
            }

        }
        //theBoard.board = currentBoard;
        //toSend = Serialize(theBoard);
        //Invoke("DoPost", 0.5f);

        if (isWinning())
        {
            gameOver = true;
            if (_player1Won)
            {
                message.text = "CONGRATS Player 1!";
                message.color = new Color32(255, 0, 186, 255);
            }
            if (_player2Won)
            {
                message.text = "CONGRATS Player 2!";
                message.color = new Color32(255, 255, 0, 255);
            }

        }
        if (isFullBoard() && !_player1Won && !_player2Won)
        {
            gameOver = true;
            message.text = "Well, You both suck.";
            message.color = new Color32(255, 255, 255, 255);
        }




    }

    void DoGet()
    {
        StartCoroutine("GetText");
       

    }

    IEnumerator GetText()
    {
        UnityWebRequest www =
            UnityWebRequest.Get("https://cooprougelike.herokuapp.com/getData.php");

        Debug.Log("Hey! Wait a minute ya filth!");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            toSend = www.downloadHandler.text;
        }

        theBoard = Deserialize(toSend);
        currentBoard = theBoard.board;

        for (int i = 0; i < currentBoard.Length; i++)
        {
            if (currentBoard[i] == 'O')
            {
                textArray[i].text = "O";
                textArray[i].color = new Color32(255, 255, 0, 255);
            }

            if (currentBoard[i] == 'X')
            {
                textArray[i].text = "X";
                textArray[i].color = new Color32(255, 0, 186, 255);
            }
        }
    }

    void DoPost()
    {
        theBoard.board = currentBoard;

        for (int i = 0; i < currentBoard.Length; i++)
        {
            if (currentBoard[i] == 'O')
            {
                textArray[i].text = "O";
                textArray[i].color = new Color32(255, 255, 0, 255);
            }

            if (currentBoard[i] == 'X')
            {
                textArray[i].text = "X";
                textArray[i].color = new Color32(255, 0, 186, 255);
            }
        }

        toSend = Serialize(theBoard);
        StartCoroutine("Upload");
    }

    IEnumerator Upload()
    {

        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection("data", toSend));

        using (UnityWebRequest www =
                UnityWebRequest.Post("" + "https://cooprougelike.herokuapp.com/postData.php", form))

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

    string Serialize(Board board)
    {
        return JsonUtility.ToJson(board);
    }

    private Board Deserialize(string toSend)
    {
        return JsonUtility.FromJson<Board>(toSend);

    }

    bool isFullBoard()
    {
        for (int i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].text == "")
            {
                return false;
            }
        }
        return true;
    }

    bool isWinning()
    {
        for (int i = 0; i < textArray.Length; i += 3)
        {
            if (textArray[i].text == textArray[i + 1].text && textArray[i + 1].text == textArray[i + 2].text && textArray[i].text != "")
            {
                if(textArray[i].text == "X")
                {
                    _player1Won = true;
                }
                if (textArray[i].text == "O")
                {
                    _player2Won = true;
                }
                return true;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (textArray[i].text == textArray[i + 3].text && textArray[i + 3].text == textArray[i + 6].text && textArray[i].text != "")
            {
                if (textArray[i].text == "X")
                {
                    _player1Won = true;
                }
                if (textArray[i].text == "O")
                {
                    _player2Won = true;
                }
                return true;
            }
        }

        if (textArray[0].text == textArray[4].text && textArray[4].text == textArray[8].text && textArray[0].text != "")
        {
            if (textArray[0].text == "X")
            {
                _player1Won = true;
            }
            if (textArray[0].text == "O")
            {
                _player2Won = true;
            }
            return true;
        }
        if (textArray[2].text == textArray[4].text && textArray[4].text == textArray[6].text && textArray[2].text != "")
        {
            if (textArray[2].text == "X")
            {
                _player1Won = true;
            }
            if (textArray[2].text == "O")
            {
                _player2Won = true;
            }
            return true;
        }
        return false;
    }

}

