using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Mysqlx.Crud;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MonitorTextManager : MonoBehaviour
{
    public static MonitorTextManager Instance { get; private set;}
    [SerializeField] private TMP_Text montiorTextField;
    [SerializeField] private string savedMonitorText;
    [SerializeField] private string lastMonitorText;
    [SerializeField] private string inputText;
    [SerializeField] private bool isTakingInput = false;
    [SerializeField] private bool isMirroringInput = false;
    [SerializeField] private bool debugUpdateText;
    [SerializeField] private UnityEvent<string> currentInputListeners;
    private bool willEatInput = false;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(this);
    }
    private void OnDestroy() {
        if(Instance == this)
            Instance = null;
    }
    public void Update(){
        if(debugUpdateText){
            debugUpdateText = false;
            this.SetMonitorText(this.savedMonitorText);
        }
        if(isTakingInput)
            this.HandleInputs();
        if(isMirroringInput)
            this.AppendText(this.inputText, false);
    }

    public void SetMonitorText(string text, bool saveText = true){
        string newText = FormatText(text);

        this.lastMonitorText = this.savedMonitorText;
        if(saveText)
            this.savedMonitorText = newText;
        this.montiorTextField.text = newText;
    }
    public void AppendText(string text, bool saveText = true){
        string newText = this.savedMonitorText + text;

        this.lastMonitorText = this.savedMonitorText;
        if(saveText)
            this.savedMonitorText = newText;
        this.montiorTextField.text = newText;
    }

    public void RevertToSavedText(bool savePrev = true){
        string savedText = this.savedMonitorText;
        if(savePrev)
            this.savedMonitorText = this.montiorTextField.text;
        this.SetMonitorText(savedText, false);
    }

    public void RevertToLastText(bool saveText = true){
        this.SetMonitorText(this.lastMonitorText, saveText);
    }

    public void AwaitUserInput(UnityAction<string> callbackFunction, bool eatInput = true){
        this.currentInputListeners.AddListener(callbackFunction);
        this.isTakingInput = true;
        this.isMirroringInput = true;

        // Done so that the input is eaten if any of the awaiting methods are set to eat inputs
        if(eatInput) willEatInput = true;
    }

    private void SubmitInput(){
        this.AppendText(this.inputText);
        this.isTakingInput = false;
        this.isMirroringInput = false;

        this.currentInputListeners.Invoke(this.inputText);
        this.currentInputListeners.RemoveAllListeners();

        if(this.willEatInput) this.inputText = "";
    }

    private string FormatText(string text){
        return text;
    }
    public string GetText(){
        return this.savedMonitorText;
    }

    private void HandleInputs(){
        if(Input.GetKeyDown(KeyCode.Q))
            this.inputText += "Q";
        if(Input.GetKeyDown(KeyCode.W))
            this.inputText += "W";
        if(Input.GetKeyDown(KeyCode.E))
            this.inputText += "E";
        if(Input.GetKeyDown(KeyCode.R))
            this.inputText += "R";
        if(Input.GetKeyDown(KeyCode.T))
            this.inputText += "T";
        if(Input.GetKeyDown(KeyCode.Y))
            this.inputText += "Y";
        if(Input.GetKeyDown(KeyCode.U))
            this.inputText += "U";
        if(Input.GetKeyDown(KeyCode.I))
            this.inputText += "I";
        if(Input.GetKeyDown(KeyCode.O))
            this.inputText += "O";
        if(Input.GetKeyDown(KeyCode.P))
            this.inputText += "P";
        if(Input.GetKeyDown(KeyCode.A))
            this.inputText += "A";
        if(Input.GetKeyDown(KeyCode.S))
            this.inputText += "S";
        if(Input.GetKeyDown(KeyCode.D))
            this.inputText += "D";
        if(Input.GetKeyDown(KeyCode.F))
            this.inputText += "F";
        if(Input.GetKeyDown(KeyCode.G))
            this.inputText += "G";
        if(Input.GetKeyDown(KeyCode.H))
            this.inputText += "H";
        if(Input.GetKeyDown(KeyCode.J))
            this.inputText += "J";
        if(Input.GetKeyDown(KeyCode.K))
            this.inputText += "K";
        if(Input.GetKeyDown(KeyCode.L))
            this.inputText += "L";
        if(Input.GetKeyDown(KeyCode.Z))
            this.inputText += "Z";
        if(Input.GetKeyDown(KeyCode.X))
            this.inputText += "X";
        if(Input.GetKeyDown(KeyCode.C))
            this.inputText += "C";
        if(Input.GetKeyDown(KeyCode.V))
            this.inputText += "V";
        if(Input.GetKeyDown(KeyCode.B))
            this.inputText += "B";
        if(Input.GetKeyDown(KeyCode.N))
            this.inputText += "N";
        if(Input.GetKeyDown(KeyCode.M))
            this.inputText += "M";

        if(Input.GetKeyDown(KeyCode.Space))
            this.inputText += " ";
        if(Input.GetKeyDown(KeyCode.Return))
            this.SubmitInput();
            // this.inputText += "\n";
        if(Input.GetKeyDown(KeyCode.Backspace))
            this.inputText = this.inputText.Substring(0, this.inputText.Length - 1);
    }
}
