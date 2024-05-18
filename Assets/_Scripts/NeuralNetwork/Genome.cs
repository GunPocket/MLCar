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

    // Fun��o para adicionar um neur�nio ao genoma
    public void AddNeuron(Neuron neuron) {
        Neurons.Add(neuron);
    }

    // Fun��o para adicionar uma conex�o ao genoma
    public void AddConnection(Connection connection) {
        Connections.Add(connection);
    }

    // Fun��o para muta��o do genoma
    public void Mutate(float mutationRate) {
        // Adicionar ou remover conex�es com uma certa probabilidade de muta��o
        foreach (var connection in Connections) {
            if (Random.value < mutationRate) {
                // Muta��o de peso
                connection.Weight += Random.Range(-0.5f, 0.5f);
            }
        }

        // Adicionar ou remover neur�nios com uma certa probabilidade de muta��o
        if (Random.value < mutationRate) {
            // Adicionar um novo neur�nio
            int newNeuronId = Neurons.Count + 1; // ID do novo neur�nio
            Neuron newNeuron = new Neuron(newNeuronId, NeuronType.Hidden, 1, 1); // Criar um novo neur�nio
            Neurons.Add(newNeuron); // Adicionar o novo neur�nio ao genoma

            // Conectar o novo neur�nio a uma conex�o existente com uma probabilidade de 50%
            if (Random.value < 0.5f && Connections.Count > 0) {
                Connection randomConnection = Connections[Random.Range(0, Connections.Count)];
                Connection newConnection1 = new Connection(randomConnection.FromNeuronId, newNeuronId, Random.Range(-1f, 1f));
                Connection newConnection2 = new Connection(newNeuronId, randomConnection.ToNeuronId, Random.Range(-1f, 1f));
                Connections.Add(newConnection1);
                Connections.Add(newConnection2);
            }
        }

        // Remover conex�es com uma certa probabilidade de muta��o
        if (Random.value < mutationRate && Connections.Count > 0) {
            Connections.RemoveAt(Random.Range(0, Connections.Count));
        }

        // Remover neur�nios com uma certa probabilidade de muta��o
        if (Random.value < mutationRate && Neurons.Count > 1) {
            Neurons.RemoveAt(Random.Range(1, Neurons.Count)); // N�o remover o primeiro neur�nio (input)
        }
    }

}
