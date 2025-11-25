[System.Serializable]
public class StationaryMover : IRegionMover
{
    public void Tick(Region region) { }
    public IRegionMover Clone() => (StationaryMover)MemberwiseClone();
}