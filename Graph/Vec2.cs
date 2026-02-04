using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
	public struct Vec2
	{
		public float x;
		public float y;

		public Vec2( float _x, float _y )
		{
			x = _x;
			y = _y;
		}

		public Vec2( PointF p )
		{
			x = p.X;
			y = p.Y;
		}

		public static Vec2 operator +(Vec2 a, Vec2 b)
		{
			return new Vec2( a.x + b.x, a.y + b.y );
		}

		public static Vec2 operator -(Vec2 a, Vec2 b)
		{
			return new Vec2( a.x - b.x, a.y - b.y );
		}

		public float Length()
		{
			return (float)Math.Sqrt(x*x+y*y);
		}

		public float LengthSqr()
		{
			return x*x+y*y;
		}

		public void Normalise()
		{
			float length = Length();
			if( length != 0.0f )
			{
				x/=length;
				y/=length;
			}
		}

		public Vec2 Normalised()
		{
			float length = Length();
			if( length == 0.0f )
				length = 1;

			return new Vec2( x/length, y/length );
		}

		public PointF AsPointF()
		{
			return new PointF( x, y );
		}
	}
}
