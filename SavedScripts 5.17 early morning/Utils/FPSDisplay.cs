using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {
    [SerializeField] private Text _text;
    private float deltaTime = 0.0f;

    private void Start() {
        
    }

    void Update() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void FixedUpdate() {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string fpsDisplay = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        if (fps < 60) {
            _text.color = new Color(0.85f, 0, 0);
        } else {
            _text.color = new Color(0, 0.85f, 0);
        }

        _text.text = fpsDisplay;
    }
}