using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    GameObject last_opened = null;

	// Use this for initialization
	void Start () {
        last_opened = GameObject.Find("Active Upgrades");
        GameObject.Find("Idle Upgrades").SetActive(false);
        GameObject.Find("Land Upgrades").SetActive(false);
        GameObject.Find("Research Upgrades").SetActive(false);
    }

    public void OpenInnerCanvas(GameObject go) {
        if (last_opened != null) last_opened.SetActive(false);
        go.SetActive(true);
        last_opened = go;
    }

    public void OpenOuterCanvas(GameObject go) {
        if (last_opened == null) {
            go.SetActive(true);
            last_opened = go;
        } else {
            if (go == last_opened) last_opened = null;
            else {
                go.SetActive(true);
                last_opened = go;
            }
        }
    }
}
