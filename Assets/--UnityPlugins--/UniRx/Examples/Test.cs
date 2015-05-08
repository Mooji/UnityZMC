using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using UniRx;

namespace UniRx.Examples
{
    public class Test : MonoBehaviour
    {
        public bool flag = true;
        void Start()
        {
            IObservable<long> stream =   Observable.EveryUpdate().Where( ob => this.isClick() );

            stream.Subscribe( ob => Debug.Log( "clicked !" ) );






        }


        private bool isClick()
        {
            return flag && Input.GetMouseButtonUp( 0 );
        }
    }
}