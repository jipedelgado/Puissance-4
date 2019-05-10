using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gcs = GameControllerScript;

public class ButtonBehaviourScript : MonoBehaviour {

    //public static Text message;

    private void Start() {
        SetColorButton(Color.green);
        }


    public void ToggleIsRunningGame() {
        if (gcs.GameOver == false) {
            Text txt = transform.Find("Text").GetComponent<Text>();
            if (gcs.IsRunningGame == false) {
                txt.text = "Mettre en pause";

                Text message = gcs.MessageDisplay.GetComponent<Text>();
                message.text = "Jeu en cours";
                Debug.Log(message.text);
                }
            else {
                txt.text = "Jouer";
                Text message = gcs.MessageDisplay.GetComponent<Text>();
                message.text = "Jeu en pause";
                }
            gcs.IsRunningGame = !gcs.IsRunningGame;
            }
        }

    public void NewGame() {
        gcs.SetNewGame();
        Text message = gcs.MessageDisplay.GetComponent<Text>();
        message.text = "Nouveau Jeu";
        }

    public void ExitGame() {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit ();
#endif
        }

    public void SetColorButton(Color color) {
        Button b;
        b = GetComponent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = color;
        b.colors = cb;
        }
    }
