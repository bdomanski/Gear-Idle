using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    public UnityEngine.UI.Text rotationDisplay;
    public Hashtable dist_calculated = new Hashtable();
    public double global_speed = 0;
    public bool clicked = false;
    private GameObject ui;
    private MoneyManager money_manager;

    private void Start() {
        ui = GameObject.Find("UI");
        money_manager = ui.GetComponent<MoneyManager>();
    }

    private void Update() {
        rotationDisplay.text = "Rotations / s: " + System.Math.Abs(System.Math.Round(global_speed / 6.0, 2));

        // Update each gear's speed
        foreach (DictionaryEntry pair in dist_calculated) {
            GearDriver gd = GameObject.Find(pair.Key.ToString()).GetComponent<GearDriver>();
            gd.Rotate(global_speed * System.Math.Pow(-1, gd.dist));
            money_manager.AddGold(System.Math.Abs(global_speed));
        }

        if(!clicked) UpdateSpeed();
    }

    private void UpdateSpeed() {
        if(global_speed <= money_manager.min_speed) {
            if (global_speed + money_manager.deceleration > money_manager.min_speed) global_speed = money_manager.min_speed;
            else global_speed += money_manager.deceleration;
        }
        else {
            if (global_speed - money_manager.deceleration < money_manager.min_speed) global_speed = money_manager.min_speed;
            else global_speed -= money_manager.deceleration;
        }
    }
}
