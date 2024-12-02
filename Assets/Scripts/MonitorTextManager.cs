using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Org.BouncyCastle.Crypto.Digests;
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
    [SerializeField] private bool isAdminMode = false;
    [SerializeField] private float awaitingInputDisplayUnderscorePeriod;
    [SerializeField] private UnityEvent<string> currentInputListeners;
    private bool willEatInput = false;
    private bool awaitingInputDisplayUnderscore = false;
    private bool flippingInputCoroutineStarted = false;

    // Start is called before the first frame update
    void Awake()
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
        // if(isDebugMode){
        //     this.SetMonitorText(this.savedMonitorText);
        // }
        if(isTakingInput)
            this.HandleInputs();
        if(isMirroringInput){
            if(this.isTakingInput){
                if(awaitingInputDisplayUnderscore){
                    this.AppendText(this.inputText + "_", false);
                    this.StartCoroutine(this.FlipDisplayInputUnderscore());
                }
                else {
                    this.AppendText(this.inputText, false);
                    this.StartCoroutine(this.FlipDisplayInputUnderscore());
                }
            } else
                this.AppendText(this.inputText, false);
        }
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
    public void ClearTerminal(bool saveText = true){
        this.SetMonitorText("", saveText);
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

    public void SetAdminMode(bool value){
        this.isAdminMode = value;
    }

    private IEnumerator FlipDisplayInputUnderscore(){
        if(flippingInputCoroutineStarted)
            yield break;

        flippingInputCoroutineStarted = true;
        yield return new WaitForSeconds(awaitingInputDisplayUnderscorePeriod);
        awaitingInputDisplayUnderscore = !awaitingInputDisplayUnderscore;
        flippingInputCoroutineStarted = false;
    }

    private void HandleInputs()
    {
        foreach (char c in Input.inputString)
        {
            if(!char.IsControl(c))
                this.inputText += c;
        }
        // if (string.IsNullOrEmpty(this.inputText))
        //     return;
        // Debug.Log(this.inputText.Length);
        //if (Input.GetKeyDown(KeyCode.Q))
        //    this.inputText += "Q";
        //if(Input.GetKeyDown(KeyCode.W))
        //    this.inputText += "W";
        //if(Input.GetKeyDown(KeyCode.E))
        //    this.inputText += "E";
        //if(Input.GetKeyDown(KeyCode.R))
        //    this.inputText += "R";
        //if(Input.GetKeyDown(KeyCode.T))
        //    this.inputText += "T";
        //if(Input.GetKeyDown(KeyCode.Y))
        //    this.inputText += "Y";
        //if(Input.GetKeyDown(KeyCode.U))
        //    this.inputText += "U";
        //if(Input.GetKeyDown(KeyCode.I))
        //    this.inputText += "I";
        //if(Input.GetKeyDown(KeyCode.O))
        //    this.inputText += "O";
        //if(Input.GetKeyDown(KeyCode.P))
        //    this.inputText += "P";
        //if(Input.GetKeyDown(KeyCode.A))
        //    this.inputText += "A";
        //if(Input.GetKeyDown(KeyCode.S))
        //    this.inputText += "S";
        //if(Input.GetKeyDown(KeyCode.D))
        //    this.inputText += "D";
        //if(Input.GetKeyDown(KeyCode.F))
        //    this.inputText += "F";
        //if(Input.GetKeyDown(KeyCode.G))
        //    this.inputText += "G";
        //if(Input.GetKeyDown(KeyCode.H))
        //    this.inputText += "H";
        //if(Input.GetKeyDown(KeyCode.J))
        //    this.inputText += "J";
        //if(Input.GetKeyDown(KeyCode.K))
        //    this.inputText += "K";
        //if(Input.GetKeyDown(KeyCode.L))
        //    this.inputText += "L";
        //if(Input.GetKeyDown(KeyCode.Z))
        //    this.inputText += "Z";
        //if(Input.GetKeyDown(KeyCode.X))
        //    this.inputText += "X";
        //if(Input.GetKeyDown(KeyCode.C))
        //    this.inputText += "C";
        //if(Input.GetKeyDown(KeyCode.V))
        //    this.inputText += "V";
        //if(Input.GetKeyDown(KeyCode.B))
        //    this.inputText += "B";
        //if(Input.GetKeyDown(KeyCode.N))
        //    this.inputText += "N";
        //if(Input.GetKeyDown(KeyCode.M))
        //    this.inputText += "M";

        //if(Input.GetKeyDown(KeyCode.Space))
        //    this.inputText += " ";
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isAdminMode)
                this.inputText += "\n";
            else
                this.SubmitInput();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (isAdminMode)
                this.willEatInput = true;
            this.SubmitInput();
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && this.inputText.Length > 0)
        {
            this.inputText = this.inputText.Substring(0, this.inputText.Length - 1);  
        }

        //if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
        //    if(Input.GetKeyDown(KeyCode.BackQuote))
        //        this.inputText += "~";
        //    if(Input.GetKeyDown(KeyCode.Alpha1))
        //        this.inputText += "!";
        //    if(Input.GetKeyDown(KeyCode.Alpha2))
        //        this.inputText += "@";
        //    if(Input.GetKeyDown(KeyCode.Alpha3))
        //        this.inputText += "#";
        //    if(Input.GetKeyDown(KeyCode.Alpha4))
        //        this.inputText += "$";
        //    if(Input.GetKeyDown(KeyCode.Alpha5))
        //        this.inputText += "%";
        //    if(Input.GetKeyDown(KeyCode.Alpha6))
        //        this.inputText += "^";
        //    if(Input.GetKeyDown(KeyCode.Alpha7))
        //        this.inputText += "&";
        //    if(Input.GetKeyDown(KeyCode.Alpha8))
        //        this.inputText += "*";
        //    if(Input.GetKeyDown(KeyCode.Alpha9))
        //        this.inputText += "(";
        //    if(Input.GetKeyDown(KeyCode.Alpha0))
        //        this.inputText += ")";
        //    if(Input.GetKeyDown(KeyCode.Minus))
        //        this.inputText += "_";
        //    if(Input.GetKeyDown(KeyCode.Equals))
        //        this.inputText += "+";
        //    if(Input.GetKeyDown(KeyCode.LeftBracket))
        //        this.inputText += "{";
        //    if(Input.GetKeyDown(KeyCode.RightBracket))
        //        this.inputText += "}";
        //    if(Input.GetKeyDown(KeyCode.Backslash))
        //        this.inputText += "|";
        //    if(Input.GetKeyDown(KeyCode.Semicolon))
        //        this.inputText += ":";
        //    if(Input.GetKeyDown(KeyCode.Quote))
        //        this.inputText += "\"";
        //    if(Input.GetKeyDown(KeyCode.Comma))
        //        this.inputText += "<";
        //    if(Input.GetKeyDown(KeyCode.Period))
        //        this.inputText += ">";
        //    if(Input.GetKeyDown(KeyCode.Slash))
        //        this.inputText += "?";
        //} else {
        //    if(Input.GetKeyDown(KeyCode.BackQuote))
        //        this.inputText += "`"; 
        //    if(Input.GetKeyDown(KeyCode.Alpha0))
        //        this.inputText += "0";
        //    if(Input.GetKeyDown(KeyCode.Alpha1))
        //        this.inputText += "1";
        //    if(Input.GetKeyDown(KeyCode.Alpha2))
        //        this.inputText += "2";
        //    if(Input.GetKeyDown(KeyCode.Alpha3))
        //        this.inputText += "3";
        //    if(Input.GetKeyDown(KeyCode.Alpha4))
        //        this.inputText += "4";
        //    if(Input.GetKeyDown(KeyCode.Alpha5))
        //        this.inputText += "5";
        //    if(Input.GetKeyDown(KeyCode.Alpha6))
        //        this.inputText += "6";
        //    if(Input.GetKeyDown(KeyCode.Alpha7))
        //        this.inputText += "7";
        //    if(Input.GetKeyDown(KeyCode.Alpha8))
        //        this.inputText += "8";
        //    if(Input.GetKeyDown(KeyCode.Alpha9))
        //        this.inputText += "9";
        //    if(Input.GetKeyDown(KeyCode.LeftBracket))
        //        this.inputText += "[";
        //    if(Input.GetKeyDown(KeyCode.RightBracket))
        //        this.inputText += "]";
        //    if(Input.GetKeyDown(KeyCode.Minus))
        //        this.inputText += "-";
        //    if(Input.GetKeyDown(KeyCode.Equals))
        //        this.inputText += "=";
        //    if(Input.GetKeyDown(KeyCode.Period))
        //        this.inputText += ".";
        //    if(Input.GetKeyDown(KeyCode.Slash))
        //        this.inputText += "/";
        //    if(Input.GetKeyDown(KeyCode.Backslash))
        //        this.inputText += "\\";
        //    if(Input.GetKeyDown(KeyCode.Semicolon))
        //        this.inputText += ";";
        //    if(Input.GetKeyDown(KeyCode.Quote))
        //        this.inputText += "\'";
        //    if(Input.GetKeyDown(KeyCode.Comma))
        //        this.inputText += ",";
        //}

        //if(Input.GetKeyDown(KeyCode.KeypadPlus))
        //    this.inputText += "+";
        //if(Input.GetKeyDown(KeyCode.KeypadMinus))
        //    this.inputText += "-";
        //if(Input.GetKeyDown(KeyCode.KeypadPeriod))
        //    this.inputText += ".";
        //if(Input.GetKeyDown(KeyCode.KeypadMultiply))
        //    this.inputText += "*";
        //if(Input.GetKeyDown(KeyCode.KeypadDivide))
        //    this.inputText += "/";
        //if(Input.GetKeyDown(KeyCode.Keypad0))
        //    this.inputText += "0";
        //if(Input.GetKeyDown(KeyCode.Keypad1))
        //    this.inputText += "1";
        //if(Input.GetKeyDown(KeyCode.Keypad2))
        //    this.inputText += "2";
        //if(Input.GetKeyDown(KeyCode.Keypad3))
        //    this.inputText += "3";
        //if(Input.GetKeyDown(KeyCode.Keypad4))
        //    this.inputText += "4";
        //if(Input.GetKeyDown(KeyCode.Keypad5))
        //    this.inputText += "5";
        //if(Input.GetKeyDown(KeyCode.Keypad6))
        //    this.inputText += "6";
        //if(Input.GetKeyDown(KeyCode.Keypad7))
        //    this.inputText += "7";
        //if(Input.GetKeyDown(KeyCode.Keypad8))
        //    this.inputText += "8";
        //if(Input.GetKeyDown(KeyCode.Keypad9))
        //    this.inputText += "9";

    }
}
