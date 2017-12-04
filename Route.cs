using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Route : MonoBehaviour {

	public int stops = 10;
	public float maxDimention = 100;
	public GameObject mark;
	public GameObject home;
	public bool debugInConsole = false;
	public Text textOutput; 
	public Vector2[] board;
	Vector2[] bestBoard;
	float maxDistance;
	LineRenderer lRen;
	double totalCombos = 1; // Needed for CountCombos();
	float tempResult = 0f;
	float bestResult = Mathf.Infinity;
	private int comboCheck = 0;

	IEnumerator Start () {
		//GenerateTestBoard();
		GenerateBoard ();
		yield return StartCoroutine ("DrawBoard");
		Debug.Log ("Starting Algorithm");
		bestBoard = new Vector2[stops];
		board.CopyTo (bestBoard, 0);
	    float timeTook = Time.realtimeSinceStartup;
		BruteForceRoute (0, (Vector2[])board.Clone());
		timeTook = Time.realtimeSinceStartup - timeTook;
		textOutput.text = "It took: " + timeTook.ToString() + " for " + comboCheck + " checks";
		DrawSolution ();
		yield return null;
	}
	
	void GenerateBoard(){
		//Method that generates random routes using the maxDimention for limiting the x & y
		board = new Vector2[stops];
		maxDistance = 0f;
		for (int i = 0; i < stops; i++){
			board [i] = new Vector2 (Random.Range (-maxDimention, maxDimention), Random.Range (-maxDimention, maxDimention));
			maxDistance = Mathf.Max (maxDistance, Mathf.Abs(board [i].x), Mathf.Abs(board [i].y));
		
		}
		Debug.Log ("Destinations: " + stops + " Possible combinations: " + (CountCombos(stops)/2) );
	}

	void GenerateTestBoard(){
		//Method for generation a specific board of stops for debugging persposes only
		board = new Vector2[stops];
		maxDistance = 0f;
		for (int i = 0; i < stops; i++){
			board [i] = new Vector2 (i + 1, 0);
			maxDistance = Mathf.Max (maxDistance, Mathf.Abs(board [i].x), Mathf.Abs(board [i].y));

		}
		Debug.Log ("Destinations: " + stops + " Possible combinations: " + (CountCombos(stops)/2) );
	}

	double CountCombos(int target){
		//Method for calculating actual different route combinations (back and forth)
		totalCombos *= target;
		if (target>1) return CountCombos(target-1);
		else return totalCombos;
	}

	IEnumerator DrawBoard(){
		//Method to draw a route on screen
		// INITIALIZE VALUES
		GameObject[] design = new GameObject[stops + 1];

		// COUNT MIN DISTANCES
		float minDistance = maxDistance;
		for (int i = 0; i < stops; i++) {
			minDistance = Mathf.Min (minDistance, Vector2.Distance (Vector2.zero, board [i]));
			//yield return null;
		}
		for (int i = 0; i < stops-1; i++) {
			for (int d = i + 1; d < stops; d++) {
				minDistance = Mathf.Min (minDistance, Vector2.Distance (board [i], board [d]));
				//yield return null;
			}
		}
		minDistance = minDistance / 1.25f;

		// CREATE OBJECT & SET SIZES
		design[0] = Instantiate (home, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		lRen = design [0].GetComponent<LineRenderer> ();
		for (int i = 1; i < stops + 1; i++) {
			design[i] = Instantiate (mark, new Vector3 (board [i - 1].x, board [i - 1].y, 0), Quaternion.identity) as GameObject;
			design[i].transform.localScale = new Vector3 (minDistance, minDistance, minDistance);
			//yield return null;
		}
			design[0].transform.localScale = new Vector3 (minDistance, minDistance, minDistance);
		Camera.main.orthographicSize = maxDistance + minDistance;
		yield return null;
	}

	void BruteForceRoute (int counter, Vector2[] boardToTry) {

		//Prepare exit conditions
		Vector2[] boardToExit = new Vector2[stops];
		boardToTry.CopyTo (boardToExit, 0);
		bool keepRunning = true;

		//Start main loop
		do {
			for (int i = counter; i < stops - 1 - counter; i++) {
				
				// SWAP POSSITIONS
				Vector2 tempVector2 = boardToTry [i];
				boardToTry [i] = boardToTry [i + 1];
				boardToTry [i + 1] = tempVector2;

				//CHECK SOLUTION
				tempResult = MeasureRoute (boardToTry);
				if (tempResult < bestResult) {
					//Debug.Log ("New Best: " + tempResult);
					bestResult = tempResult;
					boardToTry.CopyTo(bestBoard, 0);
				}

				//GOING DEEPER!!!
				//keepRunning = (boardToTry.SequenceEqual (boardToExit)) ? false : true;
				//if (!keepRunning) break;
				if (counter < stops - 3) BruteForceRoute (counter+1, boardToTry);
					
			}

			//CHECK FOR EXIT
			keepRunning = (boardToTry.SequenceEqual (boardToExit)) ? false : true; 

		
		} while (keepRunning);

	}

	float MeasureRoute (Vector2[] boardToMeasure){
		comboCheck++;
		float totalDistance = Vector2.Distance (Vector2.zero, boardToMeasure[0]);
		if (debugInConsole)
			Debug.Log ("Stop: " + (0) + " = " + boardToMeasure [0].ToString ());
		for (int i = 0; i < stops - 1; i++) {
			if (debugInConsole)
				Debug.Log ("Stop: " + (i + 1) + " = " + boardToMeasure [i + 1].ToString ());
			totalDistance += Vector2.Distance (boardToMeasure [i], boardToMeasure [i + 1]);
		}
		totalDistance += Vector2.Distance (boardToMeasure [stops - 1], Vector2.zero);
		return totalDistance;
	}

	void DrawSolution(){
		lRen.positionCount = stops + 2;
		lRen.SetPosition (0, Vector3.zero);
		for (int i = 1; i < stops + 1; i++) {
			lRen.SetPosition (i, (Vector3)bestBoard[i - 1]);
		}
	}
}