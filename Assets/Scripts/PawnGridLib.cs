using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PawnGridLib : MonoBehaviour {


    public struct Point : IEquatable<Point> {
        public int p;
        public int q;

        public bool Equals(Point other) {
            return other.p == p && other.q == q;
            }

        public Point(int p, int q) {
            this.p = p;
            this.q = q;
            }
        public int getP() { return p; }
        public int getQ() { return q; }


        //  l'ensemble des Point comme espace vectoriel sur Z
        static public Point Add(Point a, Point b) {
            return new Point(a.p + b.p, a.q + b.q);
            }
        public Point Add(Point a) {
            return new Point(a.p + this.p, a.q + this.q);
            }

        static public Point Scale(int k, Point point) {
            return new Point(k * point.p, k * point.q);
            }

        static public List<Point> Orientations = new List<Point>{
            new Point(0, 1),
            new Point(0, -1),
            new Point(1, 0),
            new Point(-1,0),
            new Point(1,1),
            new Point(-1,-1),
            new Point(-1, 1),
            new Point(1, -1)
        };

        // les générateurs de Orientations, par le calcul des opposés
        static public List<Point> Directions = new List<Point> {
            new Point(0, 1),
            new Point(1, 0),
            new Point(1, 1),
            new Point(1, -1)
            };

        public override string ToString() {
            return "( " + this.p + " , " + this.q + " )";
            }

        public Point Neighbor(int direction) {
            if (direction >= 0 && direction < 8) {
                return Add(this, Orientations[direction]);
                }
            else return this;
            }

        public List<Point> Neighborhood() {
            List<Point> voisinage = new List<Point>();
            foreach (Point p in Orientations) {
                voisinage.Add(Add(this, p));
                }
            return voisinage;
            }
        }

    // end of struct Point

    public struct Pawn : IEquatable<Pawn> {
        public readonly int p;
        public readonly int q;
        public float hauteur;
        public int value;
        public int Player;
        public GameObject myGO;

        public bool Equals(Pawn other) {
            return other.p == p && other.q == q;
            }

        // constructeur
        public Pawn(int p, int q, float hauteur, int player, int value, GameObject go) {
            this.p = p;
            this.q = q;
            this.hauteur = hauteur;
            this.Player = player;
            this.value = value;
            myGO = go;
            myGO.transform.position = new Vector3(this.p, hauteur, this.q);
            }

        public Pawn(int p, int q, int player, int value, GameObject go) {
            this.p = p;
            this.q = q;
            this.hauteur = .0f;
            this.Player = player;
            this.value = value;
            myGO = go;
            myGO.transform.position = new Vector3(this.p, this.hauteur, this.q);
            }

        public Pawn(int p, int q) {
            this.p = p;
            this.q = q;
            hauteur = .0f;
            Player = 0;
            value = -1;
            myGO = null;
            }

        public int getP() { return p; }
        public int getQ() { return q; }

        public void SetPlayer(int player) {
            this.Player = player;
            }
        public void SetValue(int value) {
            this.value = value;
            }
        public void SetGO(GameObject go) {
            this.myGO = go;
            }
        public void SetHauteur(float hauteur) {
            this.hauteur = hauteur;
            myGO.transform.position = new Vector3(this.p, hauteur, this.q);
            }

        public void SetColor(Color color) {
            if (myGO != null) {
                myGO.GetComponent<Renderer>().material.SetColor("_Color", color);
                }
            }

        public static Pawn nullPawn = new Pawn(-2, -2);

        //  l'ensemble des Pawn comme espace vectoriel sur Z
        static public Pawn Add(Pawn a, Pawn b) {
            return new Pawn(a.p + b.p, a.q + b.q);
            }
        public Pawn Add(Pawn a) {
            return new Pawn(a.p + this.p, a.q + this.q);
            }

        static public Pawn Scale(int k, Pawn pawn) {
            return new Pawn(k * pawn.p, k * pawn.q);
            }

        static public List<Pawn> Orientations = new List<Pawn>{
            new Pawn(0, 1),
            new Pawn(0, -1),
            new Pawn(1, 0),
            new Pawn(-1,0),
            new Pawn(1,1),
            new Pawn(-1,-1),
            new Pawn(-1, 1),
            new Pawn(1, -1)
        };

        // les générateurs de Orientations, par le calcul des opposés
        static public List<Pawn> Directions = new List<Pawn> {
            new Pawn(0, 1),
            new Pawn(1, 0),
            new Pawn(1,1),
            new Pawn(1, -1)
            };

        public override string ToString() {
            return "( " + p + " , " + q + " , " + Player + " , " + value + " )";
            }

        public Pawn Neighbor(int direction) {
            if (direction >= 0 && direction < 8) {
                return Add(this, Orientations[direction]);
                }
            else return this;
            }

        public List<Pawn> Neighborhood() {
            List<Pawn> voisinage = new List<Pawn>();
            foreach (Pawn sq in Orientations) {
                voisinage.Add(Add(this, sq));
                }
            return voisinage;
            }

        // verifie que pawn est dans dico, puis extrait le pawn indexeur de dico
        public Pawn GetPawnFrom(Dictionary<Pawn, GameObject> dico) {
            Dictionary<Pawn, GameObject>.KeyCollection keyColl = dico.Keys;
            foreach (Pawn sq in keyColl) {
                if (sq.Equals(this)) {
                    return sq;
                    }
                }
            return nullPawn;
            }

        // verifie que GO est dans dico, puis extrait le pawn indexant
        public Pawn GetPawnFrom(GameObject go, Dictionary<Pawn, GameObject> dico) {
            foreach (KeyValuePair<Pawn, GameObject> kvp in dico) {
                if (kvp.Value.Equals(go)) {
                    return kvp.Key;
                    }
                }
            return nullPawn;
            }


        // verifie que pawn est dans dico, puis extrait le pawn indexeur de dico
        // version static
        public static Pawn GetPawnFrom(Pawn pawn, Dictionary<Pawn, GameObject> dico) {
            Dictionary<Pawn, GameObject>.KeyCollection keyColl = dico.Keys;
            foreach (Pawn sq in keyColl) {
                if (sq.Equals(pawn)) {
                    return sq;
                    }
                }
            return nullPawn;
            }

        public static Pawn GetPawnFrom(Pawn pawn, List<Pawn> liste) {
            foreach (Pawn pion in liste) {
                if (pion.Equals(pawn)) {
                    return pion;
                    }
                }
            return nullPawn;
            }

        public Pawn GetPawnFrom(List<Pawn> liste) {
            foreach (Pawn pawn in liste) {
                if (pawn.Equals(this)) {
                    return pawn;
                    }
                }
            return nullPawn;
            }

        } // fin de struct Pawn ----------------------------------------------------------------

    public static List<Pawn> GetLine(Pawn racine, Pawn direction, List<Pawn> liste, int player) {
        List<Pawn> line = new List<Pawn>();
        Pawn currentRacine = racine;
        Pawn adjacent = new Pawn();
        Pawn shifted = new Pawn();
        bool run = true;
        while (run) {
            shifted = currentRacine.Add(direction);
            adjacent = shifted.GetPawnFrom(liste);
            if (!adjacent.Equals(Pawn.nullPawn) && adjacent.Player == player) {
                line.Add(adjacent);
                currentRacine = adjacent;
                }
            else run = false;
            }
        run = true;
        currentRacine = racine;
        while (run) {
            Pawn inverseDirection = Pawn.Scale(-1, direction);
            shifted = currentRacine.Add(inverseDirection);
            adjacent = shifted.GetPawnFrom(liste);
            if (!adjacent.Equals(Pawn.nullPawn) && adjacent.Player == player) {
                line.Add(adjacent);
                currentRacine = adjacent;
                }
            else run = false;
            }
        if (line.Count > 0) { line.Add(racine); }
        return line;
        }

    public static List<Point> GetLine(Point racine, Point direction, List<Point> liste, int player) {
        List<Point> line = new List<Point>();
        Point currentRacine = racine;
        Pawn adjacentPawn = new Pawn();
        Point shiftedPoint = new Point();
        bool run = true;

        while (run) {
            shiftedPoint = currentRacine.Add(direction);
            if (GameControllerScript.PlayedPoint.Contains(shiftedPoint)) {
                adjacentPawn = GameControllerScript.DicoPointPawn[shiftedPoint];
                if (adjacentPawn.Player == player) {
                    line.Add(shiftedPoint);
                   // Debug.Log("Player " + player + " value of " + racine.ToString() + " = " + line.Count + " -> " + PawnGridLib.ToString(line));

                    currentRacine = shiftedPoint;
                    }
                else run = false;
                }
            else run = false;
            }

        run = true;
        currentRacine = racine;
        Point inverseDirection = Point.Scale(-1, direction);
        while (run) {
            shiftedPoint = currentRacine.Add(inverseDirection);
            if (GameControllerScript.PlayedPoint.Contains(shiftedPoint)) {
                adjacentPawn = GameControllerScript.DicoPointPawn[shiftedPoint];
                if (adjacentPawn.Player == player) {
                    line.Add(shiftedPoint);
                    currentRacine = shiftedPoint;
                    }
                else run = false;
                }
            else run = false;
            }

        if (line.Count > 0) {
            line.Add(racine);
            }
        return line;
        }

    public static string ToString(List<Point> line) {
        string stringToReturn = "";
        foreach (Point point in line) {
            stringToReturn = stringToReturn + " ( " + point.p + " , " + point.q + ", " + " ) -";
            }
        return stringToReturn;
        }
    }




