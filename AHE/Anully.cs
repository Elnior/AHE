using System;
using System.Net;
using System.Text;
using System.Reflection;
/*
	Creation Date: 19 Wed Feb 2025
	Upgrate date: 18 Fri Jul 2025
	File version: 1.2
	file author: Elnior Loreh
	..
*/
namespace NElniorPackS
{
	internal interface IHeaders
	{
		string[] this [string anReference]
		{
			get;
		}
	}
	public struct Headers : IHeaders
	{
		public string[] headerKeys;
		private string[][] headerValues;
		private readonly string list;
		public string[] this [string key]
		{
			get
			{
				string[] col = new string[0];
				int position = Array.IndexOf<string>(this.headerKeys, key);
				if (position != -1)
					col = this.headerValues[position];
				return col;
			}
		}
		public Headers (string[] headerKeys, string[][] headerValues, string list)
		{
			this.headerKeys = headerKeys;
			this.headerValues = headerValues;
			this.list = list;
		}
		public override string ToString ()
		{
			return this.list;
		}
	}
	public class RegularLlyna : object
	{
		// fields
		public byte[] body;
		protected bool done;
		public Headers headers;
		public static byte[] lastBytesOfHeaders = { 13, 10, 13, 10 };
		// methods
		public static int indexOf (byte[] all, byte unit, int position)
		{
			int index = -1;
			for (; position < all.Length; position++)
			{
				if (all[position] == unit)
				{
					index = position;
					break;
				}
			}
			return index;
		}
		public static int indexOfGroup (byte[] total, byte[] grouped)
		{
			int index = -1;
			if (grouped.Length <= total.Length)
			{
				for (int pos = 0; pos < total.Length; pos++)
				{
					int successIndex = 0;
					while (successIndex < grouped.Length && (pos + successIndex) < total.Length)
					{
						if (total[pos + successIndex] == grouped[successIndex])
						{
							if (successIndex+1 == grouped.Length)
								return pos;
						}
						else
							break;
						successIndex++;
					}
				}
			}
			return index;
		}
		public static string ReplaceAll (string original, string part, string newPart)
		{
			string replaced = original.Replace(part, newPart);
			if (replaced.IndexOf(part, 0) != -1)
				return ReplaceAll(replaced, part, newPart);
			else
				return replaced;
		}
		public static implicit operator bool (RegularLlyna regularLlynaing)
		{
			return regularLlynaing.done;
		}
	}
	// Anully: It's the HTTP request parser
	public sealed class Anully : RegularLlyna
	{
		public readonly int headerLength;
		public readonly string htWithVersion;
		public readonly string method, path;
		// Improved implementation is available :).
		public Anully (byte[] ReqData, int limit) : base ()
		{
			try
			{
				this.body = new byte[0];
				if (limit > 6)
				{
					bool isFirst = true;
					string[] headerKeys = new string[0];
					string[][] headerValues = new string[0][];
					
					string list = "";

					int headerLength = 0;
					// Where is 13
					int dx = indexOf(ReqData, 13, headerLength);
					byte[] eachLine;
					int start, start2;
					while (dx != -1)
					{
						eachLine = new byte[dx - headerLength];
						start = 0;
						start2 = headerLength;
						for (; start < eachLine.Length; start++)
						{
							eachLine[start] = ReqData[start2];
							start2++;
						}
						// Oh!, It's getting from headerLength variable to dx variable
						headerLength = dx + 2;

						// --(Important Block to procced headers)-- 0101010101010101..
						{
							string line = Encoding.UTF8.GetString(eachLine, 0, eachLine.Length);
							if(isFirst) 
							{
								int spaceIndex = line.IndexOf("\u0020", 0);
								// first
								this.method = line.Substring(0, spaceIndex);
								spaceIndex = line.IndexOf("\u0020", spaceIndex + 1);
								// second
								string pth = line.Substring(this.method.Length, spaceIndex - this.method.Length);
								// And third
								this.htWithVersion = line.Substring(this.method.Length + pth.Length, line.Length - (this.method.Length + pth.Length)).Trim();
								
								this.path = ReplaceAll(pth, "%20", "\u0020");
								isFirst = false;
								this.done = true;
							}
							else 
							{
								int indexForWork = line.IndexOf(":", 0);
								string key = line.Substring(0, indexForWork).ToLower();
								indexForWork = key.Length + 1;
								string theValue = line.Substring(indexForWork, line.Length - indexForWork);
								// upgrading keys and values
								string[] total = new string[headerKeys.Length + 1];
								string[][] total2 = new string[total.Length][];
								int Position = 0;
								for (; Position < headerKeys.Length; Position++)
								{
									total[Position] = headerKeys[Position];
									total2[Position] = headerValues[Position];

								}
								list += key +" ";
								total[Position] = key;
								total2[Position] = theValue.Split(",".ToCharArray());
								headerKeys = total;
								headerValues = total2;
							}
						}

						dx = indexOf(ReqData, 13, headerLength);
						if (ReqData.Length > dx)
						{
							if (ReqData.Length > (dx + 1))
							{
								if (ReqData[dx + 2] == 13)
								{
									dx += 4;
									headerLength = dx;
									if (dx < ReqData.Length)
									{
										this.body = new byte[ReqData.Length - dx];
										int ix = 0;
										for (; dx < ReqData.Length; dx++)
										{
											this.body[ix] = ReqData[dx];
											ix++;
										}
									}
									else {}
									break;
								}
							}
						}
					}
					this.headerLength = headerLength;
					this.headers = new Headers(headerKeys, headerValues, list);
				}
				else
					throw new Exception("<Null>");
			}
			catch (Exception anException)
			{
				anException.HelpLink = null;
				this.done = false;
			}
		}
	}
}