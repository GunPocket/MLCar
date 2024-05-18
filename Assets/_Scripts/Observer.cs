using UnityEngine;

public class Observer : MonoBehaviour {
    [SerializeField] private PopulationObserver populationObserver;
    [SerializeField] private FollowCarCamera followCarCamera;
    [SerializeField] private PopulationManager populationManager;

    private void Start() {
        populationObserver.Initialize(this);
        followCarCamera.Initialize(this);
    }

    public PopulationObserver GetPopulationObserver() {
        return populationObserver;
    }

    public FollowCarCamera GetFollowCarCamera() {
        return followCarCamera;
    }

    public PopulationManager GetPopulationManager() {
        return populationManager;
    }
}
