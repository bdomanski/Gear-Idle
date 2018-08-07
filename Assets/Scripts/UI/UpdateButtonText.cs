using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateButtonText : MonoBehaviour {

    public UnityEngine.UI.Text button_text;
    public double base_cost;
    public double base_value;
    public double cost_factor;
    public double upgrade_factor;
    public int level = 1;
    public double current_cost, current_value;
    private MoneyManager money_manager;

    // Use this for initialization
    void Start () {
        current_cost = base_cost;
        current_value = base_value;
        money_manager = GameObject.Find("UI").GetComponent<MoneyManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool UpdateCosts() {
        // Try to remove gold
        if (!money_manager.RemoveGold(System.Math.Ceiling(current_cost))) {
            Debug.Log("Not enough gold!");
            return false;
        }
        Debug.Log("Upgrading...");

        ++level;
        current_cost *= cost_factor;
        return true;
    }

    public void UpdateText(double current, double next, double cost) {
        button_text.text = "Level: " + level + "\nValue: " + System.Math.Round(current, 2) + " (" + System.Math.Round(next - current, 3) + ")\nCost: " + System.Math.Ceiling(cost);
    }
}
