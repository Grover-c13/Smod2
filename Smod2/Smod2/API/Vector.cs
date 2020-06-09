using System;

namespace Smod2.API
{
	public class Vector
	{
		public readonly float x;
		public readonly float y;
		public readonly float z;
		
		public Vector(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>  
		///  Shorthand for writing new Vector(0, 0, 0).
		/// </summary> 
		public static Vector Zero => new Vector(0, 0, 0);

		/// <summary>  
		///  Shorthand for writing new Vector(1, 1, 1).
		/// </summary> 
		public static Vector One => new Vector(1, 1, 1);

		/// <summary>  
		///  Shorthand for writing new Vector(0, 0, 1).
		/// </summary> 
		public static Vector Forward => new Vector(0, 0, 1);

		/// <summary>  
		///  Shorthand for writing new Vector(0, 0, -1).
		/// </summary> 
		public static Vector Back => new Vector(0, 0, -1);

		/// <summary>  
		///  Shorthand for writing new Vector(0, 1, 0).
		/// </summary> 
		public static Vector Up => new Vector(0, 1, 0);

		/// <summary>  
		///  Shorthand for writing new Vector(0, -1, 0).
		/// </summary> 
		public static Vector Down => new Vector(0, -1, 0);

		/// <summary>  
		///  Shorthand for writing new Vector(1, 0, 0).
		/// </summary> 
		public static Vector Right => new Vector(1, 0, 0);

		/// <summary>  
		///  Shorthand for writing new Vector(-1, 0, 0).
		/// </summary> 
		public static Vector Left => new Vector(-1, 0, 0);

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		public static float Distance(Vector a, Vector b) => (a - b).Magnitude;

		/// <summary>
		/// Linearly interpolates two vectors given a value from 0 to 1.
		/// </summary>
		public static Vector Lerp(Vector a, Vector b, float t)
		{
			// Clamp between 0 and 1
			t = Math.Min(t, 1);
			t = Math.Max(t, 0);

			return LerpUnclamped(a, b, t);
		}

		public static Vector LerpUnclamped(Vector a, Vector b, float t) => a + (b - a) * t;

		/// <summary>
		/// Returns the smallest values of the two vectors.
		/// </summary>
		public static Vector Min(Vector a, Vector b) => 
			new Vector(
				Math.Min(a.x, b.x), 
				Math.Min(a.y, b.y),
				Math.Min(a.z, b.z)
				);

		/// <summary>
		/// Returns the largest values of the two vectors.
		/// </summary>
		public static Vector Max(Vector a, Vector b) => 
			new Vector(
				Math.Max(a.x, b.x), 
				Math.Max(a.y, b.y), 
				Math.Max(a.z, b.z)
				);

		/// <summary>
		/// Calculates the magnitude (distance from origin) of a vector.
		/// </summary>
		public float Magnitude => (float) Math.Sqrt(SqrMagnitude);

		/// <summary>
		/// Calculates the square of the magnitude (distance from origin) of a vector.
		/// </summary>
		public float SqrMagnitude =>
			x * x +
			y * y +
			z * z;

		/// <summary>
		/// Coverts a vector's values to a max of 1.
		/// </summary>
		public Vector Normalize
		{
			get
			{
				float length = Magnitude;
				if (length > 9.99999974737875E-06) return this / length;

				return Zero;
			}
		}

		public static Vector operator +(Vector a, float b)
		{
			float length = Magnitude;
			if (length > 9.99999974737875E-06) return a*((length + b)/length);
			
			return Zero;
		}

		public static Vector operator -(Vector a, float b)
		{
			float length = Magnitude;
			if (length > 9.99999974737875E-06) return a*((length - b)/length);
			
			return Zero;
		}

		public static Vector operator *(Vector a, float b)
		{
			return new Vector(a.x * b, a.y * b, a.z * b);
		}
		public static Vector operator *(float a, Vector b) => b * a;

		public static Vector operator /(Vector a, float b)
		{
			return new Vector(a.x / b, a.y / b, a.z / b);
		}

		public static Vector operator +(Vector a, Vector b)
		{
			return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vector operator -(Vector a, Vector b)
		{
			return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Vector operator *(Vector a, Vector b)
		{
			return new Vector(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Vector operator /(Vector a, Vector b)
		{
			return new Vector(a.x / b.x, a.y / b.y, a.z / b.z);
		}

		/// <summary>
		/// Performs an value (not reference) equality check of two vectors. Use <see cref="object.Equals(object)"/> for reference checks.
		/// </summary>
		public static bool operator ==(Vector a, Vector b)
		{
			if (a is null)
			{
				return b is null;
			}

			if (b is null)
			{
				return false;
			}

			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		/// <summary>
		/// Performs an value (not reference) inequality check of two vectors. Use <see cref="object.Equals(object)"/> for reference checks.
		/// </summary>
		public static bool operator !=(Vector a, Vector b)
		{
			return !(a == b);
		}
		
		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj);
		}
		
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = x.GetHashCode();
				hashCode = (hashCode * 397) ^ y.GetHashCode();
				hashCode = (hashCode * 397) ^ z.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString() => $"({x}, {y}, {z})";
		public string ToString(string format) => $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)})";
	}
}
