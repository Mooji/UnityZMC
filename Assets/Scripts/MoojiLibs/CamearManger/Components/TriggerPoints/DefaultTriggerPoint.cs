using UnityEngine;

namespace Mooji
{
    public class DefaultTriggerPoint : CamInfluenceTriggerPoint
    {

        private CameraInFluenceController _camInfulenceController;

        private Vector3 _offsetVec;

        void Start()
        {

            IGhostCamRigDriver driver = GetComponentInChildren<IGhostCamRigDriver>();

            if ( null != driver )
            {

                _camInfulenceController = GetComponentInParent<CameraInFluenceController>();

                if ( null != _camInfulenceController )
                {
                    _offsetVec = getCamRig().transform.position - _camInfulenceController.ghostTarget.position;
                }

            }
        }


        override public Vector3 getCamRigPosition()
        {
            if ( _camInfulenceController == null )
                return Vector3.zero;

            return _camInfulenceController.ghostTarget.position + _offsetVec;
        }



    }
}
