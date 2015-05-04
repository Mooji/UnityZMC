using UnityEngine;

namespace Mooji
{
    public class SimpleAnimatorHashID : MonoBehaviour , IAnimatorHashID
    {
        public int runFloat;
        public int turnFloat;

        public int attackFloat;
        public int underAttackFloat;
        public int dayingBool;

        void Awake()
        {
            runFloat = Animator.StringToHash("Forward");
            turnFloat = Animator.StringToHash( "Turn" );

            attackFloat         = Animator.StringToHash( "Attack" );
            underAttackFloat    = Animator.StringToHash( "UnderAtteck" );
            dayingBool          = Animator.StringToHash( "Daying" );
        }

    }
}
