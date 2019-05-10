using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Pawn = PawnGridLib.Pawn;
using Point = PawnGridLib.Point;

public class GameControllerScript : MonoBehaviour {
    public int nbLignes;
    public int nbColonnes;
    public GameObject PawnPrefab;

    public static int computer = -1;
    public static int human = 1;
    public static int board = 2;
    public static int playable = 3;

    public static float hauteur = .5f;
    public static Color humanColor = Color.red;
    public static Color computerColor = Color.yellow;
    public static Color playableColor = Color.white;
    public static Color boardColor = Color.black;
    public static List<Point> BoardPoint;

    public static Dictionary<int, Color> Couleurs;
    public static List<Point> PlayablePoint;
    public static List<Point> PlayedPoint;
    public static Dictionary<Point, Pawn> DicoPointPawn;

    public static bool IsRunningGame;
    public static bool GameOver;
    public static GameObject MessageDisplay;

    // Use this for initialization
    void Start() {

        MessageDisplay = GameObject.Find("Message");

        BoardPoint = new List<Point>();
        Couleurs = new Dictionary<int, Color> {
                { human, humanColor },
                { computer, computerColor },
                { board, boardColor },
                { playable, playableColor }
            };

        // initialisation de DicoPointPawn et PlayablePoint
        DicoPointPawn = new Dictionary<Point, Pawn>();
        PlayablePoint = new List<Point>();
        PlayedPoint = new List<Point>();

        // création de Boardpoint
        for (int ligne = 0; ligne < nbColonnes; ligne++) {
            for (int colonne = 0; colonne < nbLignes; colonne++) {
                Point point = new Point(ligne, colonne);
                BoardPoint.Add(point);
                }
            }

        // création de DicoPointPawn
        foreach (Point point in BoardPoint) {
            GameObject go = (GameObject) Instantiate(PawnPrefab);
            Pawn pawn = new Pawn(point.p, point.q, 1f, board, 0, go);
            SetColor(pawn, Couleurs[board]);
            DicoPointPawn.Add(point, pawn);
            }

        // création de la liste des Points Playable
        foreach (Point point in BoardPoint) {
            if (point.q == 0) {
                PlayablePoint.Add(point);
                Pawn pawn = DicoPointPawn[point];
               // SetColor(pawn, Color.white);
                }
            }
        IsRunningGame = true;
        GameOver = false;
        }
    // end of start()

    public static void SetNewGame() {

        // remise des pions de Board à leur couleur de base
        foreach (Point point in BoardPoint) {
            Pawn pawn = DicoPointPawn[point];
            SetColor(pawn, boardColor);
            }

        // purge des coups déjà joués
        PlayedPoint.Clear();

        // purge de la liste des Points Playable
        PlayablePoint.Clear();

        // création de la liste des Points Playable
        foreach (Point point in BoardPoint) {
            if (point.q == 0) {
                PlayablePoint.Add(point);
                Pawn pawn = DicoPointPawn[point];
                //SetColor(pawn, Color.white);
                }
            }
        IsRunningGame = true;
        GameOver = false;
        }

    public static string ToString(Point point) {
        string stringToReturn = "";
        if (DicoPointPawn.ContainsKey(point)) {
            Pawn pawn = DicoPointPawn[point];
            stringToReturn = stringToReturn + " ( " + point.p + " , " + point.q + ", " + pawn.Player + " ) -";
            }
        return stringToReturn;
        }

    public static void SetColor(GameObject go, Color color) {
        if (go != null) {
            go.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }

    public static void SetColor(Pawn pawn, Color color) {
        if (pawn.myGO != null) {
            pawn.myGO.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }

    public static void SetColor(List<Point> liste, Color color) {
        foreach (Point point in liste) {
            if (DicoPointPawn.ContainsKey(point)) {
                Pawn pawn = DicoPointPawn[point];
                SetColor(pawn, color);
                }
            }
        }

    public static Point PointBestPlay(int player) {
        Point pointToBeReturned = new Point();
        int MaxValue = 0;
        foreach (Point point in PlayablePoint) {
            int value = Value(point, player);
            if (value >= MaxValue) {
                pointToBeReturned = point;
                MaxValue = value;
                }
            }
        return pointToBeReturned;
        }

    public static void Play(Point point, int player) {
        if (PlayablePoint.Contains(point)) {
            int UP = 0;
            Point pointAudessus = point.Neighbor(UP);
            PlayablePoint.Remove(point);
            if (BoardPoint.Contains(pointAudessus)) {
                PlayablePoint.Add(pointAudessus);
                }

            // mise à jour de DicoPointPawn
            Pawn pawn = DicoPointPawn[point];
            pawn.SetColor(Couleurs[player]);
            pawn.SetPlayer(player);
            DicoPointPawn.Remove(point);
            DicoPointPawn.Add(point, pawn);

            /*
             * // visualisation de PlayablePoint, pour debug
            foreach (Point p in PlayablePoint) {
                DicoPointPawn[p].SetColor(Couleurs[playable]);
                }
            */

            // mise à jour de PlayedPoint
            PlayedPoint.Add(point);
            }
        }

    public static int TestWinner(Point point) {
        // recherche d'une ligne de 4 Squares contigus affectés au même joueur
        int toBeReturned = 0;
        Pawn pawn = DicoPointPawn[point];
        if (Value(point, pawn.Player) > 3) {
            toBeReturned = pawn.Player;
            }
        return toBeReturned;
        }

    public static List<Point> LongestLine(Point point, int player) {
        // retourne la  plus grande chaine contenant point, pour le joueur player
        int value = 0;
        List<Point> toBeReturned = new List<Point>();
        foreach (Point direction in Point.Directions) {
            List<Point> ligne = PawnGridLib.GetLine(point, direction, PlayedPoint, player);
            if (ligne.Count > value) {
                toBeReturned = ligne;
                }
            }
        return toBeReturned;
        }

    public static int Value(Point point, int player) {
        // retourne la longueur de la plus grande chaine contenant point
        int value = 0;
        foreach (Point direction in Point.Directions) {
            List<Point> ligne = PawnGridLib.GetLine(point, direction, PlayedPoint, player);
            if (ligne.Count > value) {
                value = ligne.Count;
                }
            }
        return value;
        }

    public static Point ComputerBestPlay() {
        Point toBeReturned = new Point();
        Point pointBestPlayComputer = PointBestPlay(computer);
        Point pointBestPlayHuman = PointBestPlay(human);
        int valueComputer = Value(pointBestPlayComputer, computer);
        int valueHuman = Value(pointBestPlayHuman, human);

        // vérifier si  >= ne rend pas la tactique de computer +/- agressive
        if (valueComputer >= valueHuman) {
            toBeReturned = pointBestPlayComputer;
            }
        else {
            toBeReturned = pointBestPlayHuman;
            }
        return toBeReturned;
        }

    }


