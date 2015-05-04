using UnityEngine;

namespace Mooji
{
    public abstract class AI : MonoBehaviour , IAI
    {

        public delegate void UpdateDelegate();
        public UpdateDelegate updateDelegate;

        public delegate void FixedUpdateDelegate();
        public FixedUpdateDelegate fixedUpdateDelegate;

        public delegate void LateUpdateDelegate();
        public LateUpdateDelegate lateUpdateDelegate;

        public void Update()
        {
            if ( updateDelegate != null && updateDelegate.GetInvocationList().Length > 0 )
            {
                updateDelegate.Invoke();
            }
        }

        public void FixedUpdate()
        {
            if ( fixedUpdateDelegate != null && fixedUpdateDelegate.GetInvocationList().Length > 0 )
            {
                fixedUpdateDelegate.Invoke();
            }
        }

        public void LateUpdate()
        {
            if ( lateUpdateDelegate != null && lateUpdateDelegate.GetInvocationList().Length > 0 )
            {
                lateUpdateDelegate.Invoke();
            }
        }

        //  =============================================================================================================================
        public bool joinLateUpdateCallBack( LateUpdateDelegate fun )
        {
            if ( lateUpdateDelegate != null )
            {
                System.Delegate[] dArr = lateUpdateDelegate.GetInvocationList();

                foreach ( var item in dArr )
                {
                    if ( !item.Equals( fun ) )
                    {

                        lateUpdateDelegate += fun;
                        return true;
                    }
                    else
                        break;
                }
            }
            else
            {
                lateUpdateDelegate += fun;
                return true;
            }

            return false;
        }


        public bool joinFixedUpdateCallBack( FixedUpdateDelegate fun )
        {
            if ( fixedUpdateDelegate != null )
            {
                System.Delegate[] dArr = fixedUpdateDelegate.GetInvocationList();

                foreach ( var item in dArr )
                {
                    if ( !item.Equals( fun ) )
                    {

                        fixedUpdateDelegate += fun;
                        return true;
                    }
                    else
                        break;
                }
            }
            else
            {
                fixedUpdateDelegate += fun;
                return true;
            }

            return false;
        }


        public bool joinUpdateCallBack( UpdateDelegate fun )
        {
            if ( updateDelegate != null )
            {
                System.Delegate[] dArr = updateDelegate.GetInvocationList();

                foreach ( var item in dArr )
                {
                    if ( !item.Equals( fun ) )
                    {

                        updateDelegate += fun;
                        return true;
                    }
                    else
                        break;
                }
            }
            else
            {
                updateDelegate += fun;
                return true;
            }

            return false;
        }

        //  =============================================================================================================================

        public void removeLateUpdateCallBack( LateUpdateDelegate fun )
        {
            if ( lateUpdateDelegate == null )
                return;

            System.Delegate[] dArr = lateUpdateDelegate.GetInvocationList();

            if ( null != dArr )
            {
                foreach ( var item in dArr )
                {
                    if ( item.Equals( fun ) )
                    {
                        lateUpdateDelegate -= fun;
                        break;
                    }
                }
            }
        }

        public void removeFixedUpdateCallBack( FixedUpdateDelegate fun )
        {
            if ( fixedUpdateDelegate == null )
                return;

            System.Delegate[] dArr = fixedUpdateDelegate.GetInvocationList();

            if ( null != dArr )
            {
                foreach ( var item in dArr )
                {
                    if ( item.Equals( fun ) )
                    {
                        fixedUpdateDelegate -= fun;
                        break;
                    }
                }
            }
        }

        public void removeUpdateCallBack( UpdateDelegate fun )
        {
            if ( updateDelegate == null )
                return;

            System.Delegate[] dArr = updateDelegate.GetInvocationList();

            if ( null != dArr )
            {
                foreach ( var item in dArr )
                {
                    if ( item.Equals( fun ) )
                    {
                        updateDelegate -= fun;
                        break;
                    }
                }
            }
        }






    }
}
