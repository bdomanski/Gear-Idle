using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDriver : MonoBehaviour {

    public double deceleration;
    private bool pressed = false, on_gear = false;
    private Vector2 lastpos = Vector2.zero, newpos = Vector2.zero;
    private Vector2 gear_pos;
    private MoneyManager money_manager;
    private GearManager gear_manager;
    public int dist = 0;
    private static int MAX_CONTACTS = 8;

	// Use this for initialization
	void Start () {
        money_manager = GameObject.Find("UI").GetComponent<MoneyManager>();
        gear_manager = GameObject.Find("Environment").GetComponent<GearManager>();
        deceleration = money_manager.deceleration;
        gear_pos = (Vector2)Camera.main.WorldToScreenPoint(transform.position);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector2.zero;
        rb.inertia = 0f;

        if (name == "gear0") UpdateDistances();
    }
	
	// Update is called once per frame
	void Update () {

        // Only rotate if mouse is pressed down on the gear
        if (pressed && on_gear) {

            // Get current position and difference between current and previous
            newpos = (Vector2)Input.mousePosition;
            gear_pos = (Vector2)Camera.main.WorldToScreenPoint(transform.position);
            double angle = Vector2.SignedAngle((newpos - gear_pos).normalized, (lastpos - gear_pos).normalized) * (System.Math.PI / 180.0) * 60.0;
            if (angle > money_manager.max_speed) angle = money_manager.max_speed;
            if (angle < money_manager.max_speed * -1) angle = money_manager.max_speed * -1;
            gear_manager.global_speed = angle;

            // Update old position with new
            lastpos = newpos;

        } else {
            //Debug.Log(name + ": " + System.Math.Pow(-1, dist) + "(" + speed + ":" + gear_manager.global_speed + ")");
        }
    }

    // Breadth first search to update distances
    private void UpdateDistances() {
        Debug.Log("Updating Distances!");
        dist = 0;
        gear_manager.dist_calculated.Clear();
        gear_manager.dist_calculated.Add(name, 0);
        Queue<GearDriver> q = new Queue<GearDriver>();
        q.Enqueue(this);

        while(q.Count > 0) {
            GearDriver gd = q.Dequeue();

            CircleCollider2D collider = gd.GetComponent<CircleCollider2D>();
            Collider2D[] results = new Collider2D[MAX_CONTACTS];
            int collisions = Physics2D.OverlapCollider(collider, new ContactFilter2D(), results);

            // Update each node that hasn't been touched
            for(int i = 0; i < collisions; ++i) {

                if (!gear_manager.dist_calculated.ContainsKey(results[i].GetComponent<GearDriver>().name)) {
                    results[i].GetComponent<GearDriver>().dist = gd.dist + 1;
                    Debug.Log(results[i].GetComponent<GearDriver>().name + ": " + results[i].GetComponent<GearDriver>().dist);
                    gear_manager.dist_calculated.Add(results[i].GetComponent<GearDriver>().name, gd.dist + 1);
                    q.Enqueue(results[i].GetComponent<GearDriver>());
                }
            }
        }
    }

    public void OnMouseEnter() {
        Debug.Log("Mouse over gear");
        on_gear = true;
    }

    public void OnMouseExit() {
        Debug.Log("Mouse not over gear");
        on_gear = false;
        pressed = false;
    }

    public void OnMouseDown() {
        Debug.Log("Mouse pressed");
        if (on_gear) {
            pressed = true;
            gear_manager.clicked = true;
            UpdateDistances();
        }
    }

    public void OnMouseUp() {
        Debug.Log("Mouse released");
        gear_manager.clicked = false;
        pressed = false;
    }

    /*public void OnCollisionEnter2D(Collision2D collision) {
        if (gameObject.name == "gear0") dist = 0;
        if (gameObject.name == ui.GetComponent<GearManager>().recently_clicked) return;
        //Debug.Log(gameObject.name);
        speed = collision.gameObject.GetComponent<GearDriver>().last_rotation * -1;
    }

    public void OnCollisionStay2D(Collision2D collision) {
        if (gameObject.name == ui.GetComponent<GearManager>().recently_clicked) return;
        speed = collision.gameObject.GetComponent<GearDriver>().last_rotation * -1;
    }*/

    public void Rotate(double angle) {

        //Debug.Log(gameObject.name + ": " + angle);

        transform.Rotate(Vector3.back * (float)angle);
    }
}
