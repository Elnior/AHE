using System;
using System.Text;
using NElniorPackS;
using System.Collections.Generic;
/*
	Creation Date: 20 Thu Feb 2025
	File version: 2.0
	Upgrade date: 12 Thu Mar 2026
	file author: Elnior Loreh
	..
*/
namespace NElniorPackS
{
	// Ylluna: It's the HTTP/1.1 response parser
	public sealed class Ylluna : RegularLlyna, IDisposable
	{
		public readonly string statusMessage, httpVersionTag;
		public readonly short statusCode;
		public Exception responseMistake = null;
		// Improved version:
		public Ylluna (byte[] data, int readed) : base()
		{
			try
			{
				this.bodyInit = readed;
				Dictionary<string, string> headers = new Dictionary<string, string>();
				this.headers = new Headers(headers);
				int start = 0, end = Array.IndexOf<byte>(data, 32, start + 1);
				this.httpVersionTag = Encoding.UTF8.GetString(data, start, end);
				start = end + 1;
				end = Array.IndexOf<byte>(data, 32, start);
				this.statusCode = Convert.ToInt16(Encoding.UTF8.GetString(data, start, end - start));
				start = end + 1;
				end = Array.IndexOf<byte>(data, 13, start + 1);
				this.statusMessage = Encoding.UTF8.GetString(data, start, end - start);
				start = end + 1;
				// Total
				// 58 ..(ignore 32 when is required).. [13,10] ending
				// Getting headers.
				while (start < readed)
				{
					end = Array.IndexOf<byte>(data, 58, start + 1);
					string headerKey = Encoding.UTF8.GetString(data, start, end - start).Trim().ToLower();
					start = end + 1;
					end = Array.IndexOf<byte>(data, 13, start + 1);
					string headerValue = Encoding.UTF8.GetString(data, start, end - start).ToLower();
					headers.Add(headerKey, headerValue.Trim());
					start = end + 2;
					if (start < readed)
					{
						// End of headers
						if (data[start] == 13)
						{
							this.bodyInit = start + 2;
							this.done = true;
							break;
						}
					}
				}
			}
			catch (Exception responseException)
			{
				this.responseMistake = responseException;
				this.done = false;
			}
		}
		public void Dispose ()
		{
			// To clean
			this.responseMistake = null;
			this.headers.headers.Clear();
			this.done = false;
		}
	}
}