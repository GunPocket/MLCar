using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork {
    private Dictionary<int, Neuron> neurons;
    private List<Connection> connections;
    private int inputSize;
    private int outputSize;
    private int hiddenSize; // Adicionando variável para armazenar o tamanho da camada oculta

    public bool structureChanged { get; set; }


    public NeuralNetwork(int inputSize, int outputSize) {
        this.inputSize = inputSize;
        this.outputSize = outputSize;
        neurons = new Dictionary<int, Neuron>();
        connections = new List<Connection>();
        hiddenSize = 0; // Inicializando o tamanho da camada oculta como 0
        CreateNetwork();
    }

    private void CreateNetwork() {
        // Criar neurônios de entrada
        for (int i = 0; i < inputSize; i++) {
            Neuron inputNeuron = new Neuron(i, NeuronType.Input, 0, i);
            neurons.Add(i, inputNeuron);
        }

        // Criar neurônios de saída
        for (int i = 0; i < outputSize; i++) {
            Neuron outputNeuron = new Neuron(inputSize + i, NeuronType.Output, 1, i);
            neurons.Add(inputSize + i, outputNeuron);
        }

        // Criar conexões entre neurônios de entrada e saída
        for (int i = 0; i < inputSize; i++) {
            for (int j = 0; j < outputSize; j++) {
                int fromNeuronId = i;
                int toNeuronId = inputSize + j;
                float weight = UnityEngine.Random.Range(-1f, 1f);
                Connection connection = new Connection(fromNeuronId, toNeuronId, weight);
                connections.Add(connection);
                neurons[toNeuronId].IncomingConnections.Add(connection);
            }
        }
    }

    public void AddHiddenNeuron() {
        int newNeuronId = neurons.Count; // ID único para o novo neurônio
        Neuron newNeuron = new Neuron(newNeuronId, NeuronType.Hidden, 1, hiddenSize); // Criar um novo neurônio oculto
        neurons.Add(newNeuronId, newNeuron);
        hiddenSize++; // Incrementar o tamanho da camada oculta

        // Conectar o novo neurônio a todos os neurônios da camada de entrada
        foreach (var inputNeuron in neurons.Values.Where(neuron => neuron.Type == NeuronType.Input)) {
            float weight = UnityEngine.Random.Range(-1f, 1f);
            Connection connection = new Connection(inputNeuron.Id, newNeuronId, weight);
            connections.Add(connection);
            newNeuron.IncomingConnections.Add(connection);
        }

        // Conectar o novo neurônio a todos os neurônios da camada de saída
        foreach (var outputNeuron in neurons.Values.Where(neuron => neuron.Type == NeuronType.Output)) {
            float weight = UnityEngine.Random.Range(-1f, 1f);
            Connection connection = new Connection(newNeuronId, outputNeuron.Id, weight);
            connections.Add(connection);
            outputNeuron.IncomingConnections.Add(connection);
        }
    }

    public float[] FeedForward(float[] inputs) {
        if (inputs.Length != inputSize) {
            Debug.LogError("Número de entradas incompatível com a rede neural.");
            return null;
        }

        for (int i = 0; i < inputSize; i++) {
            neurons[i].Value = inputs[i];
        }

        foreach (var neuron in neurons.Values) {
            if (neuron.Type != NeuronType.Input) {
                float sum = 0;
                foreach (var connection in neuron.IncomingConnections) {
                    if (neurons.ContainsKey(connection.FromNeuronId)) {
                        sum += neurons[connection.FromNeuronId].Value * connection.Weight;
                    }
                }
                neuron.Value = Sigmoid(sum);
            }
        }

        float[] outputs = new float[outputSize];
        for (int i = 0; i < outputSize; i++) {
            outputs[i] = neurons[inputSize + i].Value;
        }

        return outputs;
    }

    private float Sigmoid(float x) {
        return 1f / (1f + Mathf.Exp(-x));
    }

    public NeuralNetwork Copy() {
        NeuralNetwork copy = new NeuralNetwork(inputSize, outputSize);
        foreach (var neuron in neurons) {
            copy.neurons.Add(neuron.Key, new Neuron(neuron.Value.Id, neuron.Value.Type, neuron.Value.LayerIndex, neuron.Value.IndexInLayer));
        }
        foreach (var connection in connections) {
            copy.connections.Add(new Connection(connection.FromNeuronId, connection.ToNeuronId, connection.Weight));
        }
        return copy;
    }

    public void NotifyStructureChanged() {
        structureChanged = true;
    }

    public NeuralNetwork Crossover(NeuralNetwork other) {
        if (this.inputSize != other.inputSize || this.outputSize != other.outputSize) {
            throw new ArgumentException("As redes neurais não têm as mesmas dimensões e não podem ser cruzadas.");
        }

        NeuralNetwork offspring = new NeuralNetwork(this.inputSize, this.outputSize);

        foreach (var neuron in neurons.Values) {
            if (neuron.Type != NeuronType.Input) {
                if (other.neurons.TryGetValue(neuron.Id, out Neuron otherNeuron)) {
                    if (!offspring.neurons.ContainsKey(neuron.Id)) {
                        Neuron newNeuron = new Neuron(neuron.Id, neuron.Type, neuron.LayerIndex, neuron.IndexInLayer);
                        foreach (var connection in neuron.IncomingConnections) {
                            Connection otherConnection = otherNeuron.IncomingConnections.Find(c => c.FromNeuronId == connection.FromNeuronId);
                            if (otherConnection != null && UnityEngine.Random.Range(0f, 1f) < 0.5f) {
                                newNeuron.IncomingConnections.Add(new Connection(connection.FromNeuronId, neuron.Id, connection.Weight));
                            } else {
                                newNeuron.IncomingConnections.Add(new Connection(connection.FromNeuronId, neuron.Id, otherConnection.Weight));
                            }
                        }
                        offspring.neurons.Add(newNeuron.Id, newNeuron);
                    }
                } else {
                    throw new ArgumentException("O neurônio com ID " + neuron.Id + " não foi encontrado na outra rede neural.");
                }
            }
        }

        return offspring;
    }

    public void Mutate(float mutationRate) {
        foreach (var connection in connections) {
            if (UnityEngine.Random.value < mutationRate) {
                connection.Weight += UnityEngine.Random.Range(-0.5f, 0.5f);
            }
        }
    }

    public List<Neuron[]> GetLayers() {
        List<Neuron[]> layers = new List<Neuron[]>();

        // Adicionar a camada de entrada
        Neuron[] inputLayer = new Neuron[inputSize];
        for (int i = 0; i < inputSize; i++) {
            inputLayer[i] = neurons[i];
        }
        layers.Add(inputLayer);

        // Adicionar camadas ocultas aqui se houver

        // Adicionar a camada de saída
        Neuron[] outputLayer = new Neuron[outputSize];
        for (int i = 0; i < outputSize; i++) {
            outputLayer[i] = neurons[inputSize + i];
        }
        layers.Add(outputLayer);

        return layers;
    }

    public int GetLayerCount() {
        return GetLayers().Count;
    }

    public int GetNeuronCountInLayer(int layerIndex) {
        if (layerIndex >= 0 && layerIndex < GetLayers().Count) {
            return GetLayers()[layerIndex].Length;
        }
        return 0;
    }

    public int GetInputSize() {
        return inputSize;
    }

    public int GetHiddenLayerCount() {
        return GetLayers().Count - 2;
    }

    public int GetOutputSize() {
        return outputSize;
    }
}
