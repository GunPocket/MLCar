using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CarController carController;
    [SerializeField] private NeuralNetwork neuralNetwork;
    [SerializeField] private CarNeuralNetworkVisualizer carNeuralNetworkVisualizer;


    public override void OnEpisodeBegin() {
        carController.Initialize(this);
        carNeuralNetworkVisualizer.Initialize(this);


        if (carController != null) {
            carController.Initialize(this);
        } else {
            Debug.LogWarning("CarController is null in OnEpisodeBegin");
        }

        if (carNeuralNetworkVisualizer != null) {
            carNeuralNetworkVisualizer.Initialize(this);
        } else {
            Debug.LogWarning("CarNeuralNetworkVisualizer is null in OnEpisodeBegin");
        }

        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogError("Rigidbody2D is null in OnEpisodeBegin");
                return;
            }
        }

        if (carController != null) {
            carController.ResetCar();
        } else {
            Debug.LogWarning("CarController is null in OnEpisodeBegin");
        }

        if (neuralNetwork == null) {
            neuralNetwork = new NeuralNetwork(4, 2);
        }

        if (carNeuralNetworkVisualizer == null) {
            carNeuralNetworkVisualizer = GetComponent<CarNeuralNetworkVisualizer>();
            if (carNeuralNetworkVisualizer == null) {
                Debug.LogError("CarNeuralNetworkVisualizer is null in OnEpisodeBegin");
                return;
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        if (rb == null) {
            Debug.LogError("Rigidbody2D is null in CollectObservations");
            return;
        }

        // Adicionar observações
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(GetFitness());
    }

    public void OnActionReceived(float[] vectorAction) {
        if (carController != null) {
            // Convert vectorAction to car control inputs
            float acceleration = vectorAction[0];
            float steering = vectorAction[1];
            carController.ApplyControl(acceleration, steering);
        } else {
            Debug.LogError("CarController is not assigned!", this.gameObject);
        }
    }

    public void SetNeuralNetwork(NeuralNetwork newNet) {
        neuralNetwork = newNet;
        carController.SetNeuralNetwork(newNet);
        carNeuralNetworkVisualizer.SetNeuralNetwork(newNet);
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
    }
}
