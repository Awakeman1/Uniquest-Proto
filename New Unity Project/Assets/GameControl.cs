using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;

    public static string DiceColour;

    // Use this for initialization
    void Start () {

        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(player1.GetComponent<FollowThePath>().waypointIndex){
            case 5:
                rollagain();
                break;
            case 8:
                portal();
                break;
            case 14:
                rollagain();
                break;
            case 18:
                rollagain();
                break;
            case 24:
                rollagain();
                break; 
            case 26:
                portal();
                break;
        }

        switch(player2.GetComponent<FollowThePath>().waypointIndex){
            case 5:
                rollagain();
                break;
            case 8:
                portal();
                break;
            case 14:
                rollagain();
                break;
            case 18:
                rollagain();
                break;
            case 24:
                rollagain();
                break; 
            case 26:
                portal();
                break;
        }

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
