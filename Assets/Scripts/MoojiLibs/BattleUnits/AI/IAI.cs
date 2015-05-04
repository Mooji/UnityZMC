
namespace Mooji
{
    public interface IAI
    {
        bool joinUpdateCallBack( AI.UpdateDelegate d);
        bool joinFixedUpdateCallBack( AI.FixedUpdateDelegate d );

        bool joinLateUpdateCallBack( AI.LateUpdateDelegate d );



        void removeUpdateCallBack( AI.UpdateDelegate d);
        void removeFixedUpdateCallBack( AI.FixedUpdateDelegate d );
        void removeLateUpdateCallBack( AI.LateUpdateDelegate d );
    }
}
