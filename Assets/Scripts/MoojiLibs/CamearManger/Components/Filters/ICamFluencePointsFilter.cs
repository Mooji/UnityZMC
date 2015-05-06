
namespace Mooji
{
    public interface ICamFluencePointsFilter
    {
        bool filter( CamFilterIntent cfi , out CamFilterResultInfo info);
    }
}
