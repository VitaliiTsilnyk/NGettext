using System;
using System.IO;

namespace NGettext.Loaders
{
	public sealed class ReadOnlyStreamWrapper : Stream
	{
		public Stream BaseStream { get; private set; }

		private bool _IsClosed = false;

		public ReadOnlyStreamWrapper(Stream baseStream)
		{
			if (baseStream == null)
				throw new ArgumentNullException("baseStream");

			this.BaseStream = baseStream;
		}

		public override void Flush()
		{
			throw new InvalidOperationException("Stream is in read-only mode.");
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			this._CheckIsClosed();
			return this.BaseStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			throw new InvalidOperationException("Stream is in read-only mode.");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			this._CheckIsClosed();
			return this.BaseStream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new InvalidOperationException("Stream is in read-only mode.");
		}

		public override bool CanRead
		{
			get { return !this._IsClosed && this.BaseStream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return !this._IsClosed && this.BaseStream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override long Length
		{
			get
			{
				throw new InvalidOperationException("Stream is in read-only mode.");
			}
		}

		public override long Position
		{
			get
			{
				this._CheckIsClosed();
				return this.BaseStream.Position;
			}
			set
			{
				this._CheckIsClosed();
				this.BaseStream.Position = value;
			}
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this._CheckIsClosed();
			return base.BeginRead(buffer, offset, count, callback, state);
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new InvalidOperationException("Stream is in read-only mode.");
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			this._CheckIsClosed();
			return base.EndRead(asyncResult);
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new InvalidOperationException("Stream is in read-only mode.");
		}

		public override int ReadByte()
		{
			this._CheckIsClosed();
			return base.ReadByte();
		}

		public override void WriteByte(byte value)
		{
			throw new InvalidOperationException("Stream is in read-only mode.");
		}

		public override void Close()
		{
			if (!this._IsClosed)
			{
				this._IsClosed = true;
			}
		}

		private void _CheckIsClosed()
		{
			if (this._IsClosed)
				throw new ObjectDisposedException(null, "Stream closed.");
		}
	}
}