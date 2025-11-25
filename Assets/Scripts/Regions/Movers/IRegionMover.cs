public interface IRegionMover
{
    void Tick(Region region);
    IRegionMover Clone();
}