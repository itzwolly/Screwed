using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointerDisplay : MonoBehaviour {

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _indicator;

    Vector3 _targetPosOnScreen;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _targetPosOnScreen = _camera.WorldToScreenPoint(_target.transform.position);

        if (onScreen(_targetPosOnScreen)) {
            //Some code to destroy indicator or make it invisible
            return;
        } else {
            Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            float angle = Mathf.Atan2(_targetPosOnScreen.y - center.y, _targetPosOnScreen.x - center.x) * Mathf.Rad2Deg;

            float coef = 0;
            if (Screen.width > Screen.height) {
                coef = Screen.width / Screen.height;
            } else {
                coef = Screen.height / Screen.width;
            }

            float degreeRange = 360f / (coef + 1);
            if (angle < 0) {
                angle = angle + 360;
            }

            int edgeLine = 0;
            if (angle < degreeRange / 4f) {
                edgeLine = 0;
            } else if (angle < 180 - degreeRange / 4f) {
                edgeLine = 1;
            } else if (angle < 180 + degreeRange / 4f) {
                edgeLine = 2;
            } else if (angle < 360 - degreeRange / 4f) {
                edgeLine = 3;
            } else {
                edgeLine = 0;
            }

            _indicator.transform.position = Camera.main.ScreenToWorldPoint(intersect(edgeLine, center, _targetPosOnScreen) + new Vector3(0, 0, 10));
            _indicator.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    bool onScreen(Vector2 input) {
        return !(input.x > Screen.width || input.x < 0 || input.y > Screen.height || input.y < 0);
    }

    Vector3 intersect(int edgeLine, Vector3 line2point1, Vector3 line2point2) {
        float[] A1 = { -Screen.height, 0, Screen.height, 0 };
        float[] B1 = { 0, -Screen.width, 0, Screen.width };
        float[] C1 = { -Screen.width * Screen.height, -Screen.width * Screen.height, 0, 0 };

        float A2 = line2point2.y - line2point1.y;
        float B2 = line2point1.x - line2point2.x;
        float C2 = A2 * line2point1.x + B2 * line2point1.y;

        float det = A1[edgeLine] * B2 - A2 * B1[edgeLine];

        return new Vector3((B2 * C1[edgeLine] - B1[edgeLine] * C2) / det, (A1[edgeLine] * C2 - A2 * C1[edgeLine]) / det, 0);
    }
}
