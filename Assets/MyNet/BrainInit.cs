﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SimpleBrain {
    private int[] layers;
    private Activation[] activations;
    // Layer, Neuron
    private float[][] neurons;

    // the neurons before the activation function
    private float[][] zneurons;
    private float[][] biases;
    // Layer, TargetNeuron, InpNeuron
    private float[][][] weights;

    TrainingSet trainingAvg;

    bool logging = false;

    public int cycles = 0;
    public BrainData GetData () {
        var data = new BrainData (layers, activations, neurons,
            zneurons, biases, weights, trainingAvg.trainingSpeed, trainingAvg.maxEpocs, logging, cycles);
        return data;
    }

    public SimpleBrain (BrainData data) {
        this.layers = data.layers;
        this.activations = data.GetActivations ();
        this.biases = data.biases;
        this.neurons = data.neurons;
        this.zneurons = data.zneurons;
        this.weights = data.weights;
        this.cycles = data.cycles;
        trainingAvg = new TrainingSet (data.layers, data.trainingSpeed, data.epocs);
    }

    public SimpleBrain (int[] layers, Activation[] activations, float trainingSpeed, int epocs, bool log) {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++) {
            this.layers[i] = layers[i];
        }

        this.logging = log;
        this.activations = activations;
        InitNeurons ();
        InitBiases ();
        InitWeights ();

        trainingAvg = new TrainingSet (layers, trainingSpeed, epocs);
    }

    public void InitNeurons () {
        neurons = new float[layers.Length][];
        zneurons = new float[layers.Length][];
        for (int i = 0; i < layers.Length; i++) {
            float[] nlayer = new float[layers[i]];
            neurons[i] = nlayer;
            nlayer = new float[layers[i]];
            zneurons[i] = nlayer;
        }
    }

    public void InitBiases () {
        biases = new float[layers.Length][];
        for (int i = 0; i < layers.Length; i++) {
            float[] blayer = new float[layers[i]];
            for (int k = 0; k < layers[i]; k++) {
                blayer[k] = Random.value - .5f;
            }
            biases[i] = blayer;
        }
    }

    public void InitWeights () {
        weights = new float[layers.Length - 1][][];
        for (int i = 0; i < layers.Length - 1; i++) {
            float[][] nlayer = new float[layers[i + 1]][];
            for (int k = 0; k < layers[i + 1]; k++) {
                float[] wlayer = new float[layers[i]];
                for (int j = 0; j < layers[i]; j++) {
                    wlayer[j] = (Random.value - .5f) * 2f;
                }
                nlayer[k] = wlayer;
            }
            weights[i] = nlayer;
        }
    }

    public void PrintNeurons () {
        string log = "[";
        foreach (float[] r in neurons) {
            log += " { ";
            foreach (float n in r) {
                log += n + " ";
            }
            log += "}";
        }
        log += " ]";
        Debug.Log (log);
    }

    public void PrintBiases () {
        string log = "[";
        foreach (float[] r in biases) {
            log += " { ";
            foreach (float n in r) {
                log += n + " ";
            }
            log += "}";
        }
        log += "]";
        Debug.Log (log);
    }

    public void PrintWeights () {
        string log = "{";
        foreach (float[][] f in weights) {
            log += "[";
            foreach (float[] r in f) {
                log += " { ";
                foreach (float w in r) {
                    log += w + " ";
                }
                log += "}";
            }
            log += "]";
        }
        log += "}";
        Debug.Log (log);
    }

    public float getWeight (int layerm1, int to, int from) {
        return weights[layerm1][to][from];
    }
}