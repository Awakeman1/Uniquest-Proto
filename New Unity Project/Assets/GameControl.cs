using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.IO;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;
    private static GameObject player1, player2;
    private static GameObject MainOverlay, QuestionOverlay, QuestionOverlay2, Wrong, Right;
    private static GameObject qPrompt, qPrompt2;
    //public static GameObject qTrue, qFalse;
    public static Button qTrue, qFalse, qTrue2, qFalse2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;

    public static string DiceColour;

    public static List<string> questionlist = new List<string>();
    public static List<string> answerlist = new List<string>();

    // Use this for initialization
    void Start () {

        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        MainOverlay = GameObject.Find("Canvas");
        Wrong = GameObject.Find("wrong");
        Right = GameObject.Find("right");

        QuestionOverlay = GameObject.Find("Question");
        qPrompt = GameObject.Find("qPrompt");
        qTrue = GameObject.Find("qTrue").GetComponent<Button>();
        qFalse = GameObject.Find("qFalse").GetComponent<Button>();

        QuestionOverlay2 = GameObject.Find("Question2");
        qPrompt2 = GameObject.Find("qPrompt2");
        qTrue2 = GameObject.Find("qTrue2").GetComponent<Button>();
        qFalse2 = GameObject.Find("qFalse2").GetComponent<Button>();



        MainOverlay.SetActive(true);        
        QuestionOverlay.SetActive(false);
        QuestionOverlay2.SetActive(false);
        Wrong.SetActive(false);
        Right.SetActive(false);

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);

        
        ReadTextFile();
        
        


    }



    public void ReadTextFile(){

        try{
            using (StreamReader streamreader = new StreamReader("Assets/QuestionList2.txt"))
            {
                string currentline;
                while ((currentline = streamreader.ReadLine()) != null)
                {
                    string question = currentline.Remove(currentline.IndexOf("@"));
                    questionlist.Add(question);
                    string answer = currentline.Substring(currentline.LastIndexOf("@") + 1);;
                    answerlist.Add(answer);
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("File could not be read");
            Console.WriteLine(e.Message);
        }

    }
   

    // Update is called once per frame
    void Update()
    {
        // switch(player1.GetComponent<FollowThePath>().waypointIndex){
        //     case 5:
        //         rollagain();
        //         break;
        //     case 8:
        //         portal();
        //         break;
        //     case 14:
        //         rollagain();
        //         break;
        //     case 18:
        //         rollagain();
        //         break;
        //     case 24:
        //         rollagain();
        //         break; 
        //     case 26:
        //         portal();
        //         break;
        // }

        // switch(player2.GetComponent<FollowThePath>().waypointIndex){
        //     case 5:
        //         rollagain();
        //         break;
        //     case 8:
        //         portal();
        //         break;
        //     case 14:
        //         rollagain();
        //         break;
        //     case 18:
        //         rollagain();
        //         break;
        //     case 24:
        //         rollagain();
        //         break; 
        //     case 26:
        //         portal();
        //         break;
        // }

        if (player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }


      
        if (player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

//if p1's position is the last in the waypoint array, p1 wins
        if (player1.GetComponent<FollowThePath>().waypointIndex == player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            string winner = "Player 1";
            gameend(winner);
        }

//if p2's position is the last in the waypoint array, p2 wins
        if (player2.GetComponent<FollowThePath>().waypointIndex ==  player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            string winner = "Player 2";
            gameend(winner);
        }

       
    }

    public static void AskQuestion(int playerToAsk)
    {
        switch (playerToAsk) {
            case 1:
                MainOverlay.SetActive(false);
                QuestionOverlay.SetActive(true);
                QuestionOverlay2.SetActive(false);

                int questiontoask = UnityEngine.Random.Range(0, questionlist.Count);
                qPrompt.GetComponent<Text>().text = questionlist[questiontoask];
                Debug.Log(answerlist[questiontoask]);
                qTrue.onClick.AddListener(delegate{Truepressed1(questiontoask);});
                qFalse.onClick.AddListener(delegate{Falsepressed1(questiontoask);});

                break;
            case 2:
                MainOverlay.SetActive(false);
                QuestionOverlay.SetActive(false);
                QuestionOverlay2.SetActive(true);

                int questiontoask2 = UnityEngine.Random.Range(0, questionlist.Count);
                qPrompt2.GetComponent<Text>().text = questionlist[questiontoask2];
                Debug.Log(answerlist[questiontoask2]);
                qTrue2.onClick.AddListener(delegate{Truepressed2(questiontoask2);});
                qFalse2.onClick.AddListener(delegate{Falsepressed2(questiontoask2);});
                break;

        }
    }
    public static void Truepressed1(int questiontoask){
       if(answerlist[questiontoask] == "true"){
           Right.SetActive(true);
            MovePlayer(1);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
            Right.SetActive(false);
            

       }
       else if(answerlist[questiontoask] == "false"){
            Wrong.SetActive(true);
            Debug.Log("the answer was meant to be false." + questiontoask);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
            Wrong.SetActive(false);
            
            
       }
    }


    public static void Truepressed2(int questiontoask2){
       if(answerlist[questiontoask2] != "false"){
            
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
            MovePlayer(2);
       }
       else if(answerlist[questiontoask2] == "false"){
            Debug.Log("the answer was meant to be false." + questiontoask2);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
       }
    }

    public static void Falsepressed1(int questiontoask){
       if(answerlist[questiontoask] == "false"){
            MovePlayer(1);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
       }
       else if(answerlist[questiontoask] == "true"){
            Debug.Log("the answer was meant to be false." + questiontoask);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
       }
    }
    public static void Falsepressed2(int questiontoask2){
       if(answerlist[questiontoask2] == "false"){
            MovePlayer(2);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
       }
       else if(answerlist[questiontoask2] == "true"){
            Debug.Log("the answer was meant to be true. " + questiontoask2);
            MainOverlay.SetActive(true);
            QuestionOverlay.SetActive(false);
            QuestionOverlay2.SetActive(false);
       }
    }


    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }
    

    public static void coloureddice(int playerToMove, int randomDiceSide)
    {
        switch(randomDiceSide){
            case 0: 
         
                DiceColour = "Red";
                break;
            case 1: 
                DiceColour = "Yellow";
                break;
            case 2: 
                DiceColour = "Purple";
                break;
            case 3:    
                DiceColour = "Blue";
                break;
            case 4:        
                DiceColour = "Orange";
                break;
            case 5:         
                DiceColour = "Green";
                break;
        }
        return;
    }



    public static void rollagain(){
        if(player1MoveText.activeSelf == true){
            Debug.Log("Player 2 Rolls Again");
        }
        else if(player2MoveText.activeSelf == true){
            Debug.Log("Player 2 Rolls Again");
        }
        else{
            Debug.Log("Dunno who Rolls again");
        }
        
    }


    public static void portal(){
        Debug.Log("We've entered a portal. Wow.");
    }



    public static void gameend(string winner){
        whoWinsTextShadow.gameObject.SetActive(true);
        player1MoveText.gameObject.SetActive(false);
        player2MoveText.gameObject.SetActive(false);
        whoWinsTextShadow.GetComponent<Text>().text = winner + " Wins";
        gameOver = true;
    }

}
