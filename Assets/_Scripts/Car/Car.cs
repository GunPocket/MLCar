using System;
using UnityEngine;

public class Car : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CarController carController;
    [SerializeField] private CarAgent carAgent;
    [SerializeField] private NeuralNetwork neuralNetwork;
    [SerializeField] private CarNeuralNetworkVisualizer carNeuralNetworkVisualizer;
    /*
    private void Start() {
        Initialize();
    }

    public void Initialize() {
        if (carController != null) {
            carController.Initialize(this);
        } else {
            Debug.LogWarning("No car controller on car", this.gameObject);
        }

        if (carAgent != null) {
            carAgent.Initialize(this);
        } else {
            Debug.LogWarning("No car agent on car", this.gameObject);
        }

        if (carNeuralNetworkVisualizer != null) {
            carNeuralNetworkVisualizer.Initialize(this);
        } else {
            Debug.LogWarning("No car neural network visualizer on car", this.gameObject);
        }
    }

    public void SetNeuralNetwork(NeuralNetwork neuNet) {
        neuralNetwork = neuNet;
    }

    public NeuralNetwork GetNeuralNetwork() {
        return neuralNetwork;
    }

    public CarController GetCarController() {
        return carController;
    }

    public Rigidbody2D GetRigidBody() {
        return rb;
    }

    public float GetFitness() {
        return carController.GetFitness();
    }

    public void ResetCar() {
        if (carController != null) {
            carController.ResetCar();
        } else {
            Debug.LogWarning("CarController is null in ResetCar");
        }
    }*/
}
