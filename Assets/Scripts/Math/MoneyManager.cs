using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    public UnityEngine.UI.Text total_gold_text;
    public double deceleration, max_speed;
    public double base_multiplier;
    public double min_speed = 0;
    private double total_gold, multiplier;

    // Use this for initialization
    void Start () {
        multiplier = base_multiplier;
    }
	
	// Update is called once per frame
	void Update () {
        total_gold_text.text = "Gold: " + System.Math.Floor(total_gold).ToString();
    }

    public void AddGold(double gold) {
        total_gold += gold * multiplier;
    }

    public bool RemoveGold(double gold) {
        if (total_gold - gold < 0) return false;
        total_gold -= gold;
        return true;
    }

    public void UpdateGoldRotation(GameObject go) {
        UpdateButtonText gold_rotation_text = go.GetComponent<UpdateButtonText>();
        if (!gold_rotation_text.UpdateCosts()) return;

        gold_rotation_text.current_value = gold_rotation_text.base_value * gold_rotation_text.level;
        multiplier = base_multiplier * gold_rotation_text.current_value;
        gold_rotation_text.UpdateText(gold_rotation_text.current_value, gold_rotation_text.current_value + gold_rotation_text.base_value, gold_rotation_text.current_cost);
    }

    public void UpdateMaxRotation(GameObject go) {
        UpdateButtonText max_rotation_text = go.GetComponent<UpdateButtonText>();
        if (!max_rotation_text.UpdateCosts()) return;

        max_speed += 0.5;
        max_rotation_text.current_value = max_speed / 6.0;
        max_rotation_text.UpdateText(max_rotation_text.current_value, max_rotation_text.current_value + (0.5/6.0), max_rotation_text.current_cost);
    }

    public void UpdateResistance(GameObject go) {
        UpdateButtonText resistance_text = go.GetComponent<UpdateButtonText>();
        if (!resistance_text.UpdateCosts()) return;

        resistance_text.UpdateText(deceleration, deceleration * resistance_text.upgrade_factor, resistance_text.current_cost);
        deceleration *= resistance_text.upgrade_factor;
    }

    public void UpdateMinRotation(GameObject go) {
        if (min_speed + 0.5 > max_speed) return;
        UpdateButtonText min_rotation_text = go.GetComponent<UpdateButtonText>();
        if (!min_rotation_text.UpdateCosts()) return;

        min_speed += 0.5;
        min_rotation_text.current_value = min_speed / 6.0;
        min_rotation_text.UpdateText(min_rotation_text.current_value, min_rotation_text.current_value - (0.5 / 6.0), min_rotation_text.current_cost);
    }
}
