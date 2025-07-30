using System;
using System.Text;
using NElniorPackS;
/*
	Creation Date: 20 Thu Feb 2025
	File version: 1.0
	file author: Elnior Loreh
	..
*/
namespace NElniorPackS
{
	// Ylluna: It's the HTTP/1.1 response parser
	public sealed class Ylluna : RegularLlyna
	{
		public readonly int headerLength;
		public readonly string statusMessage, htWithVersion;
		public readonly int statusCode;
		// Old implementation..
		public Ylluna (byte[] RespData, int limit) : base()
		{
			int beginIndex = limit + lastBytesOfHeaders.Length;
			try
			{
				this.body = new byte[RespData.Length - limit - lastBytesOfHeaders.Length];
				this.headerLength = RespData.Length - this.body.Length;
				if (limit > 3)
				{
					string stringResp = Encoding.UTF8.GetString(RespData, 0, limit);
					this.done = true;
					bool isFirst = true;
					string[] lines = stringResp.Split("\r\n".ToCharArray());
					string[] headerKeys = new string[0];
					string[][] headerValues = new string[0][];
					// I use limit variable again
					limit = 0;
					string list = "";
					foreach (string line in lines)
					{
						if(isFirst) 
						{
							int spaceIndex = line.IndexOf("\u0020", 0);
							// first
							this.htWithVersion = line.Substring(0, spaceIndex);

							spaceIndex = line.IndexOf("\u0020", spaceIndex + 1);
							// second
							string statusCode = line.Substring(this.htWithVersion.Length, spaceIndex - this.htWithVersion.Length);
							// And third
							this.statusMessage = line.Substring(this.htWithVersion.Length + statusCode.Length, line.Length - (this.htWithVersion.Length + statusCode.Length)).Trim();
							
							this.statusCode = Convert.ToInt32(statusCode);
							isFirst = false;
							headerKeys = new string[lines.Length - 1];
							headerValues = new string[lines.Length - 1][];
						}
						else 
						{
							int indexForWork = line.IndexOf(":", 0);
							string key = line.Substring(0, indexForWork).ToLower();
							list += key + ((limit == headerKeys.Length-1)? "" : "[..]; ");
							headerKeys[limit] = key;
							indexForWork = key.Length + 1;
							string theValue = line.Substring(indexForWork, line.Length - indexForWork);
							headerValues[limit] = theValue.Split(",".ToCharArray());
							limit++;
						}
					}
					int contentIndex = 0;
					while (contentIndex < this.body.Length)
					{
						this.body[contentIndex] = RespData[beginIndex];
						beginIndex++;
						contentIndex++;
					}
					this.headers = new Headers(headerKeys, headerValues, list);
				}
			}
			catch (Exception anException)
			{
				anException.HelpLink = null;
				this.done = false;
			}
		}
	}
}