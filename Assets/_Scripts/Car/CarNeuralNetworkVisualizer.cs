using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CarNeuralNetworkVisualizer : MonoBehaviour {
    private NeuralNetwork neuralNetwork;
    private CarAgent carAgent;

    public float spacingX = 2f;
    public float spacingY = 1.5f;

    public bool ShowNetwork = false;
    public bool ShowCarData = false;
    public bool ShowVectors = false;

    public void Initialize(CarAgent carAgent) {
        this.carAgent = carAgent;
        neuralNetwork = carAgent.GetNeuralNetwork();
    }

    void OnDrawGizmos() {
        if (neuralNetwork == null || carAgent == null) return;

        if (ShowNetwork) {
            DrawNeuralNetwork(neuralNetwork);
        }

        if (ShowCarData) {
            DrawCarData();
        }

        if (ShowVectors) {
            DrawVectors();
        }
    }

    void DrawNeuralNetwork(NeuralNetwork neuralNetwork) {
        List<Neuron[]> layers = neuralNetwork.GetLayers();
        if (layers == null) return;

        for (int layer = 0; layer < layers.Count; layer++) {
            for (int neuronIndex = 0; neuronIndex < layers[layer].Length; neuronIndex++) {
                Neuron neuron = layers[layer][neuronIndex];
                Vector3 neuronPosition = new Vector3(transform.position.x + layer * spacingX, transform.position.y + neuronIndex * spacingY, transform.position.z);

                Color neuronColor = Color.white;
                if (layer > 0 && layer < layers.Count - 1) {
                    neuronColor = Color.green; // Neurônios ocultos
                }

                if (layer > 0) {
                    foreach (Connection connection in neuron.IncomingConnections) {
                        if (connection.Weight > 0) {
                            neuronColor = Color.Lerp(neuronColor, Color.blue, connection.Weight);
                        } else {
                            neuronColor = Color.Lerp(neuronColor, Color.red, -connection.Weight);
                        }

                        // Desenhar conexão com neurônio anterior
                        Neuron prevNeuron = GetNeuronById(layers[layer - 1], connection.FromNeuronId);
                        if (prevNeuron != null) {
                            Vector3 prevNeuronPosition = new Vector3(transform.position.x + (layer - 1) * spacingX, transform.position.y + Array.IndexOf(layers[layer - 1], prevNeuron) * spacingY, transform.position.z);
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawLine(prevNeuronPosition, neuronPosition);
                        }
                    }
                }
                Gizmos.color = neuronColor;
                Gizmos.DrawSphere(neuronPosition, 0.3f);
            }
        }
    }

    void DrawCarData() {
        Vector3 carPosition = carAgent.transform.position;
        Vector3 velocity = carAgent.GetComponent<Rigidbody2D>().velocity;
        float speed = velocity.magnitude/10;

        // Desenhar dados do carro (sem vetores)
#if UNITY_EDITOR
        Handles.Label(carPosition + Vector3.up * 2, $"Speed: {speed:F2}");
        float dragCoefficient = carAgent.GetComponent<Rigidbody2D>().drag;
        Handles.Label(carPosition + Vector3.up * 1.5f, $"Drag Coefficient: {dragCoefficient:F2}");
#endif
    }

    void DrawVectors() {
        Vector3 carPosition = carAgent.transform.position;
        Vector3 velocity = carAgent.GetComponent<Rigidbody2D>().velocity;

        // Desenhar vetor de velocidade
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(carPosition, carPosition + velocity);
#if UNITY_EDITOR
        Handles.Label(carPosition + velocity, $"Speed: {velocity.magnitude:F2}");
#endif

        // Desenhar vetor de força do drag
        float dragCoefficient = carAgent.GetComponent<Rigidbody2D>().drag;
        Vector3 dragForce = -dragCoefficient * velocity.normalized;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(carPosition, carPosition + dragForce);
#if UNITY_EDITOR
        Handles.Label(carPosition + dragForce, $"Drag Force: {dragForce.magnitude:F2}");
#endif
    }

    Neuron GetNeuronById(Neuron[] neurons, int id) {
        if (neurons == null) return null;

        foreach (Neuron neuron in neurons) {
            if (neuron.Id == id) {
                return neuron;
            }
        }
        return null;
    }

    public void SetNeuralNetwork(NeuralNetwork nn) {
        neuralNetwork = nn;
    }
}
