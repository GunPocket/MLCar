using UnityEngine;

public class NeuralNetwork : MonoBehaviour {
    private int inputSize;
    private int outputSize;

    private float[,] weightsInputToOutput;

    public NeuralNetwork(int inputSize, int outputSize) {
        this.inputSize = inputSize;
        this.outputSize = outputSize;

        weightsInputToOutput = new float[inputSize, outputSize];

        // Inicializar os pesos com valores aleatórios
        InitializeRandomWeights();
    }

    private void InitializeRandomWeights() {
        System.Random rand = new System.Random();

        for (int i = 0; i < inputSize; i++) {
            for (int j = 0; j < outputSize; j++) {
                weightsInputToOutput[i, j] = (float)rand.NextDouble() * 2 - 1; // Valores entre -1 e 1
            }
        }
    }

    public float[] FeedForward(float[] inputs) {
        if (inputs.Length != inputSize) {
            Debug.LogError("Tamanho de entrada inválido para a rede neural!");
            return null;
        }

        float[] outputs = new float[outputSize];

        // Calcular saídas com base nos pesos e entradas
        for (int j = 0; j < outputSize; j++) {
            float sum = 0;
            for (int i = 0; i < inputSize; i++) {
                sum += inputs[i] * weightsInputToOutput[i, j];
            }
            outputs[j] = sum;
        }

        return outputs;
    }

    public void DrawGizmos(Vector2 position) {
        float spacingX = 2f;
        float spacingY = 1.5f;

        // Desenhar neurônios de entrada
        for (int i = 0; i < inputSize; i++) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(position.x - spacingX, position.y + (i - inputSize / 2) * spacingY, 0f), 0.3f);
        }

        // Desenhar neurônios de saída e conexões
        for (int j = 0; j < outputSize; j++) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(position.x + spacingX, position.y + (j - outputSize / 2) * spacingY, 0f), 0.3f);

            for (int i = 0; i < inputSize; i++) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(new Vector3(position.x - spacingX, position.y + (i - inputSize / 2) * spacingY, 0f),
                                new Vector3(position.x + spacingX, position.y + (j - outputSize / 2) * spacingY, 0f));
            }
        }
    }
}
