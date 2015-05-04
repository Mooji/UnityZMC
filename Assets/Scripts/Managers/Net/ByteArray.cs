using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mooji
{

    public enum EncodingType
    {
        utf8
    }


    public class ByteArray : MemoryStream
    {

        public void WriteByteArr( byte[] data , int receiveLength , int startIndex )
        {
            this.Write( data , int.Parse( this.Position.ToString() ) , receiveLength );
            this.Seek( startIndex , SeekOrigin.Begin );
        }

        //==================================================================================================
        public int WriteInt32( int i )
        {
            byte[] b = BitConverter.GetBytes( i );
            //this.Read( b , 0 , b.Length );
            Write( b , 0 , b.Length );
            return b.Length;
        }

        public int ReadInt()
        {
            byte[] b = new byte[sizeof( int )];
            this.Read( b , 0 , b.Length );
            return BitConverter.ToInt32( b , 0 );
        }
        //==================================================================================================

        public string ReadString( int len , EncodingType et = EncodingType.utf8 )
        {
            if ( EncodingType.utf8 == et )
            {

                byte[] _stringByteArr = new byte[len];
                this.Read( _stringByteArr , 0 , len );
                return Encoding.UTF8.GetString( _stringByteArr , 0 , len );
            }

            return null;
        }

        public int WriteString( string str , EncodingType et = EncodingType.utf8 )
        {
            if ( EncodingType.utf8 == et )
            {
                byte[] strByteArr  = Encoding.UTF8.GetBytes( str );
                byte[] strLenByteArr  = BitConverter.GetBytes( strByteArr.Length );
                Write( strLenByteArr , 0 , strLenByteArr.Length );
                Write( strByteArr , 0 , strByteArr.Length );
                return strByteArr.Length + strLenByteArr.Length;
            }
            return -1;
        }
        //==================================================================================================

        public bool ReadBoolean()
        {
            byte[] b = new byte[sizeof( bool )];
            this.Read( b , 0 , b.Length );
            return BitConverter.ToBoolean( b , 0 );
        }

        public int WriteBoolean( bool b )
        {
            byte[] boolByteArr = BitConverter.GetBytes( b );
            this.Write( boolByteArr , 0 , boolByteArr.Length );
            return boolByteArr.Length;
        }

        //==================================================================================================
        public float ReadFloat()
        {
            byte[] b = new byte[sizeof( float )];
            this.Read( b , 0 , b.Length );
            return BitConverter.ToSingle( b ,0);
        }
        public int WriteFloat(float f)
        {
            byte[] b = BitConverter.GetBytes( f );
            this.Write( b , 0 , b.Length );
            return b.Length;
        }
        //==================================================================================================
        public byte[] getWriteByteDatas(int maxLen)
        {
            this.Seek( 0 , SeekOrigin.Begin );
            byte[] b = new byte[maxLen];
            this.Read( b , 0 , b.Length );
            return b;
        }

    }





}
