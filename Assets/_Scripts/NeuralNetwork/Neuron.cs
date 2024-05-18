using System.Collections.Generic;

public class Neuron {
    public int Id { get; private set; }
    public NeuronType Type { get; private set; }
    public float Value { get; set; }
    public int LayerIndex { get; set; } // �ndice da camada em que o neur�nio est�
    public int IndexInLayer { get; set; } // �ndice do neur�nio na camada
    public List<Connection> IncomingConnections { get; private set; }

    public Neuron(int id, NeuronType type, int layerIndex, int indexInLayer) {
        Id = id;
        Type = type;
        LayerIndex = layerIndex;
        IndexInLayer = indexInLayer;
        IncomingConnections = new List<Connection>();
    }
}

public enum NeuronType {
    Input,
    Output,
    Hidden
}