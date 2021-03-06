﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    public int[] layers;

    public float[] input;

    public GameObject circle;

    public List<Transform> points = new List<Transform>();
    public LineRenderer line;
    private Camera cam;

    SimpleBrain brain;

    //public float[] output;

    Neuron neuron;
    void Start()
    {
        Activation[] activations = new Activation[layers.Length];
        for (int i = 0; i < activations.Length; i++) {
            activations[i] = i == activations.Length - 1 ? Activation.Sigmoid : Activation.Tanh;
        }
        brain = new SimpleBrain(layers, activations, 0.05f, 100, true);
        brain.PrintBiases();
        brain.Predict(input);
        brain.PrintNeurons();

        cam = Camera.main;

        //Debug.Log(cam.ViewportToWorldPoint(new Vector2(.1f, .5f)));


        neuron = new Neuron();
        //Debug.Log("predict " + neuron.Predict(1));
        //Debug.Log("Cost " + neuron.Train(1, 0));

        //Debug.Log("--------------------------");
        //Debug.Log("predict " + neuron.Predict(1));
        //Debug.Log("Cost " + neuron.Train(1, 0));

        brain.TrainTEST(input, new float[]{ 0.5f, .5f});
        brain.PrintNeurons();
        //brain.Train(input, output);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Transform point = Instantiate(circle).transform;
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            point.position = pos;

            Debug.Log("???" + pos);

            points.Add(point);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("--------------------------");
            brain.TrainTEST(new float[] { 0.5f, 0.5f }, new float[] { .5f, .5f });
            //var x = Random.value;
            //Debug.Log(x + " - predict " + neuron.Predict(x));
        }
        if (Input.GetKey(KeyCode.S)) {
            Debug.Log("--------------------------");
            var x = Random.value;
            var op = x > .5f ? 1 : 0;
            var v2 = op > 0 ? 0 : 1;
            brain.TrainTEST(new float[] { x, 1 - x }, new float[] { v2, op });
        }

        if (Input.GetKey(KeyCode.W)) {
            var x = Random.value;
            neuron.Train(x, x > .5f ? 0 : 1);
        }

        //DrawGraph();

        //Train();
    }

    const int size = 10;
    void DrawGraph() {
        line.positionCount = size + 1;
        float x = 0;
        for (int i = 0; i <= size; i++, x+= 1f / size) {
            var prediction = brain.Predict(new float[] { x });
            //Vector2 point = new Vector2(x, (prediction[0] + 1) * .5f);
            var y = prediction[0];
            Vector2 point = new Vector2(x, y);
            Vector2 pos = cam.ViewportToWorldPoint(point);

            //Debug.Log(y);
            line.SetPosition(i, pos);
        } 
    }

    void Train() {
        foreach (Transform point in points) {
            //brain.Train()
            var target = PosToViewPort(point.position);
            float[] input = new float[] { target.x };
            float[] output = new float[] { target.y };
            brain.TrainTEST(input, output);
        }
    }

    public Vector2 PosToScreenRatio(Vector2 pos) {
        return pos / (OrthographicBounds(cam) * .5f);
    }

    public Vector2 PosToViewPort(Vector2 pos) {
        return cam.WorldToViewportPoint(pos);
    }

    public static Vector2 OrthographicBounds(Camera camera) {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        return new Vector3(cameraHeight * screenAspect, cameraHeight);
    }
}
