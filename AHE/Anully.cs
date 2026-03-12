using System;
using System.Text;
using System.Collections.Generic;
/*
	Creation Date: 19 Wed Feb 2025
	Upgrade date: 11 Wed Mar 2026
	File version: 2.0
	file author: Elnior Loreh
	..
*/
namespace NElniorPackS
{
	internal interface IHeaders
	{
		string this [string anReference]
		{
			get;
		}
	}
	public struct Headers : IHeaders
	{
		public Dictionary<string, string> headers;
		public string this [string key]
		{
			get
			{
				foreach (string possibleKey in this.headers.Keys)
					if (possibleKey == key)
						return this.headers[key];
				return null;
			}
		}
		public Headers (Dictionary<string, string> headers)
		{
			this.headers = headers;
		}
		public override string ToString ()
		{
			return "Header Count: " + this.headers.Count;
		}
	}
	public struct HReference
	{
		public string path, queries;
		public HReference (string path, string query)
		{
			this.path = path;
			this.queries = query;
		}
	}
	public class RegularLlyna : object
	{
		public int bodyInit;
		protected bool done;
		public Headers headers;
		public static char[] hexSymbols = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
		public static implicit operator bool (RegularLlyna regularLlynaing)
		{
			return regularLlynaing.done;
		}
	}
	// Anully: It's the HTTP request parser
	public sealed class Anully : RegularLlyna, IDisposable
	{
		public readonly string method, httpVersionTag;
		public HReference hReference;
		public Exception anullyException = null;
		// Improved version:
		public Anully (byte[] data, int readed) : base ()
		{
			try
			{
				this.bodyInit = readed;
				this.hReference = new HReference(null, null);
				Dictionary<string, string> headers = new Dictionary<string, string>();
				this.headers = new Headers(headers);
				int start = 0, end = Array.IndexOf<byte>(data, 32, start + 1), next = 0;
				this.method = Encoding.UTF8.GetString(data, start, end).ToLower();
				start = end + 1;
				next = Array.IndexOf<byte>(data, 32, start);
				end = Array.IndexOf<byte>(data, 63, start);
				if (end == -1 || end >= next)
					end = next;
				else
					this.hReference.queries = Encoding.UTF8.GetString(data, end + 1, next - end - 1);
				this.hReference.path = Encoding.UTF8.GetString(data, start, end - start);
				end = next;
				start = end + 1;
				end = Array.IndexOf<byte>(data, 13, start + 1);
				this.httpVersionTag = Encoding.UTF8.GetString(data, start, end - start);
				start = end + 1;
				int pgIndex = -1;
				int indexOfCode = 0, indexOfDigit = 0;
				int digit, secondDigit;
				// URI Decoder:
				while ((pgIndex = this.hReference.path.IndexOf("%", pgIndex + 1)) != -1)
				{
					indexOfDigit = pgIndex + 2;
					for (; indexOfDigit < this.hReference.path.Length; indexOfDigit += 2)
					{
						digit = Array.IndexOf(hexSymbols, char.ToLower(this.hReference.path[indexOfDigit - 1]));
						secondDigit = Array.IndexOf(hexSymbols, char.ToLower(this.hReference.path[indexOfDigit]));
						if (digit != -1 && secondDigit != -1)
						{
							data[indexOfCode++] = Convert.ToByte((digit << 4) + secondDigit);
							indexOfDigit++;
							if (indexOfDigit < this.hReference.path.Length)
								if (this.hReference.path[indexOfDigit] != '%')
									break;
						}
						else
							break;
					}
					if (indexOfCode > 0)
						this.hReference.path = this.hReference.path.Replace(this.hReference.path.Substring(pgIndex, indexOfCode * 3), Encoding.UTF8.GetString(data, 0, indexOfCode));
					indexOfCode = 0;
				}
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
			catch (Exception dataException)
			{
				this.anullyException = dataException;
				this.done = false;
			}
		}
		public void Dispose ()
		{
			// To clean
			this.anullyException = null;
			this.hReference.path = null;
			this.hReference.queries = null;
			this.headers.headers.Clear();
			this.done = false;
		}
	}
}