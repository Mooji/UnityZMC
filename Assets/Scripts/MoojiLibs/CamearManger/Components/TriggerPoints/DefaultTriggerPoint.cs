namespace Mooji
{
    public class DefaultTriggerPoint : CamInfluenceTriggerPoint
    {
        void Start()
        {

            IGhostCamRigDriver driver = GetComponentInChildren<IGhostCamRigDriver>();

            if ( null != driver )
            {

                CameraInFluenceController camInfulenceController = GetComponentInParent<CameraInFluenceController>();

                if ( null != camInfulenceController )
                {
                    CamDriverIntent intent = new CamDriverIntent();
                    intent.ghostTarget = camInfulenceController.ghostTarget;
                    intent.ghostCamRig = camInfulenceController.ghostRig;
                    driver.init( intent );
                }

            }
        }

    }
}
