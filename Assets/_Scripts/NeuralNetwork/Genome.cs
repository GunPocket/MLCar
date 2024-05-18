using System.Collections.Generic;
using UnityEngine;

public class Genome {
    public List<Neuron> Neurons { get; }
    public List<Connection> Connections { get; }
    public float Fitness { get; set; }
    public float CarPositionY { get; set; }

    public Genome() {
        Neurons = new List<Neuron>();
        Connections = new List<Connection>();
        Fitness = 0f;
    }

    // Função para adicionar um neurônio ao genoma
    public void AddNeuron(Neuron neuron) {
        Neurons.Add(neuron);
    }

    // Função para adicionar uma conexão ao genoma
    public void AddConnection(Connection connection) {
        Connections.Add(connection);
    }

    // Função para mutação do genoma
    public void Mutate(float mutationRate) {
        // Adicionar ou remover conexões com uma certa probabilidade de mutação
        foreach (var connection in Connections) {
            if (Random.value < mutationRate) {
                // Mutação de peso
                connection.Weight += Random.Range(-0.5f, 0.5f);
            }
        }

        // Adicionar ou remover neurônios com uma certa probabilidade de mutação
        if (Random.value < mutationRate) {
            // Adicionar um novo neurônio
            int newNeuronId = Neurons.Count + 1; // ID do novo neurônio
            Neuron newNeuron = new Neuron(newNeuronId, NeuronType.Hidden, 1, 1); // Criar um novo neurônio
            Neurons.Add(newNeuron); // Adicionar o novo neurônio ao genoma

            // Conectar o novo neurônio a uma conexão existente com uma probabilidade de 50%
            if (Random.value < 0.5f && Connections.Count > 0) {
                Connection randomConnection = Connections[Random.Range(0, Connections.Count)];
                Connection newConnection1 = new Connection(randomConnection.FromNeuronId, newNeuronId, Random.Range(-1f, 1f));
                Connection newConnection2 = new Connection(newNeuronId, randomConnection.ToNeuronId, Random.Range(-1f, 1f));
                Connections.Add(newConnection1);
                Connections.Add(newConnection2);
            }
        }

        // Remover conexões com uma certa probabilidade de mutação
        if (Random.value < mutationRate && Connections.Count > 0) {
            Connections.RemoveAt(Random.Range(0, Connections.Count));
        }

        // Remover neurônios com uma certa probabilidade de mutação
        if (Random.value < mutationRate && Neurons.Count > 1) {
            Neurons.RemoveAt(Random.Range(1, Neurons.Count)); // Não remover o primeiro neurônio (input)
        }
    }

}
