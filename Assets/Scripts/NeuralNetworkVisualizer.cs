using UnityEngine;

public class NeuralNetworkVisualizer : MonoBehaviour {
    public NeuralNetwork neuralNetwork;
    public Vector2 position;

    void OnDrawGizmos() {
        if (neuralNetwork != null) {
            neuralNetwork.DrawGizmos(position);
        }
    }
}
