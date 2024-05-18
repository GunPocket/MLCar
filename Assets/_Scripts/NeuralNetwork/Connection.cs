public class Connection {
    public int FromNeuronId { get; private set; }
    public int ToNeuronId { get; private set; }
    public float Weight { get; set; }

    public Connection(int fromNeuronId, int toNeuronId, float weight) {
        FromNeuronId = fromNeuronId;
        ToNeuronId = toNeuronId;
        Weight = weight;
    }
}