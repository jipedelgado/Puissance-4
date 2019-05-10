using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using gcs = GameControllerScript;
using Point = PawnGridLib.Point;

public class PionBehaviourScript : MonoBehaviour {
    private int xPosition;
    private int zPosition;
    //private bool gameOn;

    void Start() {
        xPosition = (int) gameObject.transform.position.x;
        zPosition = (int) gameObject.transform.position.z;
        }

    private void OnMouseDown() {
        if (gcs.IsRunningGame == true && gcs.GameOver == false) {
            Point myPoint = new Point(xPosition, zPosition);
            if (gcs.PlayablePoint.Contains(myPoint)) {
                SetColor(gcs.humanColor);
                gcs.Play(myPoint, gcs.human);
                if (gcs.TestWinner(myPoint) == gcs.human) {
                    Debug.Log("human wins ! ");
                    List<Point> LongestLine = gcs.LongestLine(myPoint, gcs.human);
                    gcs.SetColor(LongestLine, Color.green);
                    Text message = gcs.MessageDisplay.GetComponent<Text>();
                    message.text = "Vous avez gagné !";
                    gcs.GameOver = true;
                    }
                if (gcs.GameOver == false) {
                    Point bestPlayPoint = gcs.ComputerBestPlay();
                    gcs.Play(bestPlayPoint, gcs.computer);
                    if (gcs.TestWinner(bestPlayPoint) == gcs.computer) {
                        Debug.Log("computer wins ! ");
                        List<Point> LongestLine = gcs.LongestLine(bestPlayPoint, gcs.computer);
                        gcs.SetColor(LongestLine, Color.green);
                        Text message = gcs.MessageDisplay.GetComponent<Text>();
                        message.text = "L'ordinateur a gagné !";
                        gcs.GameOver = true;
                        }
                    }
                }
            }
        }

    private void SetColor(Color color) {
        GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }
