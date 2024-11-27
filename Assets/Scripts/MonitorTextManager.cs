using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Mysqlx.Crud;
using TMPro;
using UnityEngine;

public class MonitorTextManager : MonoBehaviour
{
    public static MonitorTextManager Instance { get; private set;}
    [SerializeField] private TMP_Text montiorTextField;
    [SerializeField] private string monitorText;
    [SerializeField] private bool debugUpdateText;

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
            this.SetMonitorText(this.monitorText);
        }
    }

    public void SetMonitorText(string text){
        string newText = formatText(text);

        this.monitorText = newText;
        this.montiorTextField.text = newText;
    }
    public void AppendText(string text){
        string newText = this.monitorText + text;

        this.monitorText = newText;
        this.montiorTextField.text = newText;
    }

    private string formatText(string text){
        return text;
    }
    public string getText(){
        return this.monitorText;
    }
}
