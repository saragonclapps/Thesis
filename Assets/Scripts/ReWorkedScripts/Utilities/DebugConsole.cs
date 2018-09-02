using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugConsole : MonoBehaviour
{
    public delegate void ConsoleCommand();

    public InputField inpField;
    public Text backText;
    public Scrollbar verticalScrollbar;

    private Dictionary<string, ConsoleCommand> _myCommands;
    private Dictionary<string, string> _descriptions;

    private void OnEnable()
    {
        inpField.Select();
        inpField.text = "";
    }

    private void Awake()
    {
        //instancio los diccionarios, similar a como hacemos con arrays o listas
        _myCommands = new Dictionary<string, ConsoleCommand>();
        _descriptions = new Dictionary<string, string>();
    }

    void Start ()
    {
        //agrego los comandos
        AddCommands("!help", ShowHelp, "EL BOTON ROJO");
        AddCommands("clr", ClearConsole, "Clears past actions from log");
        AddCommands("!next", LoadNextLevel, "Load next Level");
        AddCommands("!restart", RestartLevel, "Restart Level");
        AddCommands("!last", LoadPreviousLevel, "Load previous Level");
        AddCommands("!test", LoadTestLevel, "Load Test Level");
	}

    private void LoadTestLevel()
    {
        SceneManager.LoadScene("Test-Cris");
    }

    public void AddCommands(string cheat, ConsoleCommand com, string description)
    {
        _myCommands.Add(cheat, com);
        _descriptions.Add(cheat, description);
    }

    public void RemoveCommand(string cm)
    {
        _myCommands.Remove(cm);
        _descriptions.Remove(cm);
    }

    public void CheckInput()
    {
        //chequeo si el comando existe en el diccionario, si no tiro un mensaje
        if (_myCommands.ContainsKey(inpField.text))
            _myCommands[inpField.text]();
        else
            backText.text += "El comando " + inpField.text + " no existe\n";

        //borro lo escrito por el usuario
        inpField.text = "";
        //pongo el scroll abajo de todo, para que se muestre siempre lo ultimo que aparecio en el log.
        verticalScrollbar.value = 0;
    }

    private void ClearConsole()
    {
        backText.text = "";
    }

    private void ShowHelp()
    {
        string result = "";
        foreach (var elem in _descriptions)
            result += elem.Key + ": " + elem.Value + "\n";

        backText.text += result;
    }

    private void LoadNextLevel()
    {
        LevelManager.instance.NextLevel(null);
    }

    private void LoadPreviousLevel()
    {
        LevelManager.instance.PreviousLevel();
    }

    private void RestartLevel()
    {
        LevelManager.instance.RestartLevel(null);
    }
}
