///////////////////////////////////////////////////////////////////////
//                             Quadruplet                            //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

using System;

namespace Miron.Web.MbCompression
{
    internal sealed class Quadruplet<TFirst, TSecond, TThird, TFourth>
    {

        #region // Private membres
        private readonly TFirst _first;
        private readonly TSecond _second;
        private readonly TThird _third;
        private readonly TFourth _forth;
        #endregion


        #region // Constructor
        public Quadruplet(TFirst first, TSecond second, TThird third, TFourth forth)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (second == null)
            {
                throw new ArgumentNullException("third");
            }
            if (second == null)
            {
                throw new ArgumentNullException("forth");
            }
            _first = first;
            _second = second;
            _third = third;
            _forth = forth;
        }
        #endregion


        #region // Properties
        public TFirst First
        {
            get
            {
                return _first;
            }
        }

        public TSecond Second
        {
            get
            {
                return _second;
            }
        }

        public TThird Third
        {
            get
            {
                return _third;
            }
        }

        public TFourth Forth
        {
            get
            {
                return _forth;
            }
        }
        #endregion


        #region // Public methods

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            Quadruplet<TFirst, TSecond, TThird, TFourth> other = obj as Quadruplet<TFirst, TSecond, TThird, TFourth>;
            return (other != null) &&
                (other._first.Equals(_first)) && (other._second.Equals(_second)) && (other._third.Equals(_third)) && (other._forth.Equals(_forth));
        }


        public override int GetHashCode()
        {
            int a = _first.GetHashCode();
            int c = _third.GetHashCode();

            int ab = ((a << 5) + a) ^ _second.GetHashCode();
            int cd = ((c << 5) + a) ^ _third.GetHashCode();

            return ((ab << 5) + ab) ^ cd.GetHashCode();
        }

        #endregion

    }
}