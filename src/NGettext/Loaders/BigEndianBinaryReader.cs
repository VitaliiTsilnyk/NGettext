using System;
using System.IO;
using System.Text;

namespace NGettext.Loaders
{
	/// <summary>
	/// Reads primitive data types as binary values represented in big endian byte order.
	/// </summary>
	public class BigEndianBinaryReader : BinaryReader
	{
		private byte[] _Buffer = new byte[16];

		/// <summary>
		/// Initializes a new instance of the <see cref="BigEndianBinaryReader"/> class based on the 
		/// supplied stream and using System.Text.UTF8Encoding.
		/// </summary>
		/// <param name="input"></param>
		public BigEndianBinaryReader(Stream input) : base(input)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BigEndianBinaryReader"/> class based on the
		/// supplied stream and a specific character encoding.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="encoding"></param>
		public BigEndianBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
		{
		}

		/// <summary>
		/// Reads a 2-byte signed integer from the current 
		/// stream and advances the current position of the stream by two bytes.
		/// </summary>
		/// <returns>
		/// A 2-byte signed integer read from the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public override short ReadInt16()
		{
			this._FillBuffer(2);
			return (short)(this._Buffer[1] | this._Buffer[0] << 8);
		}

		/// <summary>
		/// Reads a 2-byte unsigned integer from the current 
		/// stream using little-endian encoding and advances the position of the stream by two bytes.
		/// </summary>
		/// <returns>
		/// A 2-byte unsigned integer read from this stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public override ushort ReadUInt16()
		{
			this._FillBuffer(2);
			return (ushort)(this._Buffer[1] | this._Buffer[0] << 8);
		}

		/// <summary>
		/// Reads a 4-byte signed integer from the current 
		/// stream and advances the current position of the stream by four bytes.
		/// </summary>
		/// <returns>
		/// A 4-byte signed integer read from the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public override int ReadInt32()
		{
			this._FillBuffer(4);
			return (int)(this._Buffer[3] | this._Buffer[2] << 8 | this._Buffer[1] << 16 | this._Buffer[0] << 24);
		}

		/// <summary>
		/// Reads a 4-byte unsigned integer from the current 
		/// stream and advances the position of the stream by four bytes.
		/// </summary>
		/// <returns>
		/// A 4-byte unsigned integer read from this stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public override uint ReadUInt32()
		{
			this._FillBuffer(4);
			return (uint)(this._Buffer[3] | this._Buffer[2] << 8 | this._Buffer[1] << 16 | this._Buffer[0] << 24);
		}

		/// <summary>
		/// Reads an 8-byte signed integer from the current 
		/// stream and advances the current position of the stream by eight bytes.
		/// </summary>
		/// <returns>
		/// An 8-byte signed integer read from the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public override long ReadInt64()
		{
			this._FillBuffer(8);
			if (BitConverter.IsLittleEndian)
			{
				// We don't need to reverse bytes on big-endian machine as BitConverter respects machine endianness
				Array.Reverse(this._Buffer, 0, 8);
			}
			return BitConverter.ToInt64(this._Buffer, 0);
		}

		/// <summary>
		/// Reads an 8-byte unsigned integer from the current 
		/// stream and advances the position of the stream by eight bytes.
		/// </summary>
		/// <returns>
		/// An 8-byte unsigned integer read from this stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public override ulong ReadUInt64()
		{
			this._FillBuffer(8);
			if (BitConverter.IsLittleEndian)
			{
				// We don't need to reverse bytes on big-endian machine as BitConverter respects machine endianness
				Array.Reverse(this._Buffer, 0, 8);
			}
			return BitConverter.ToUInt64(this._Buffer, 0);
		}

		/// <summary>
		/// Reads a 4-byte floating point value from the current 
		/// stream and advances the current position of the stream by four bytes.
		/// </summary>
		/// <returns>
		/// A 4-byte floating point value read from the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public override float ReadSingle()
		{
			this._FillBuffer(4);
			if (BitConverter.IsLittleEndian)
			{
				// We don't need to reverse bytes on big-endian machine as BitConverter respects machine endianness
				Array.Reverse(this._Buffer, 0, 4);
			}
			return BitConverter.ToSingle(this._Buffer, 0);
		}

		/// <summary>
		/// Reads an 8-byte floating point value from the current 
		/// stream and advances the current position of the stream by eight bytes.
		/// </summary>
		/// <returns>
		/// An 8-byte floating point value read from the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public override double ReadDouble()
		{
			this._FillBuffer(8);
			if (BitConverter.IsLittleEndian)
			{
				// We don't need to reverse bytes on big-endian machine as BitConverter respects machine endianness
				Array.Reverse(this._Buffer, 0, 8);
			}
			return BitConverter.ToDouble(this._Buffer, 0);
		}


		private void _FillBuffer(int numBytes)
		{
			if (numBytes < 2 || numBytes > this._Buffer.Length)
			{
				throw new ArgumentOutOfRangeException("numBytes");
			}
			int bytesRead = 0;
			int n = 0;

			var stream = this.BaseStream;
			if (stream == null)
				throw new ObjectDisposedException("Base stream closed.");

			do
			{
				n = stream.Read(this._Buffer, bytesRead, numBytes - bytesRead);
				if (n == 0)
				{
					throw new EndOfStreamException("Unexpected End Of File.");
				}
				bytesRead += n;
			} while (bytesRead < numBytes);
		}
	}
}
