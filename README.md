# Ally Http Engine (AHE)
*v2.0.0*

Hi!, I'm Elnior, I created this to process faster
HTTP data of Software Agents (Browsers & Servers).

## You can see some examples:

**response.txt** content:  
```txt
HTTP/1.1 200 ready
content-type: text/txt
content-length: 218
date: this time
server: NElniorS

ertoihrtgirtyg
rtyoiurtgoiry
ertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoihrtgirtygertoih



 // //
  // //


rtgirtygertoihrtgirtyg
```

**request.txt** content:  
```txt 
GET /home/ HTTP/1.1
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
Accept-Encoding: gzip, deflate, br, zstd
Accept-Language: en-US,en;q=0.9
Cache-Control: max-age=0
Connection: keep-alive
Host: 127.0.0.2
Referer: http://127.0.0.2/
Sec-Fetch-Dest: document
Sec-Fetch-Mode: navigate
Sec-Fetch-Site: same-origin
Sec-Fetch-User: ?1
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36
sec-ch-ua: "Not:A-Brand";v="99", "Google Chrome";v="145", "Chromium";v="145"
sec-ch-ua-mobile: ?0
sec-ch-ua-platform: "Windows"


```
C# coding:
```C#
	FileStream fs = new FileStream("response.txt", FileMode.Open);
	// 13Kb
	byte[] data = new byte[13312];
	int readed = fs.Read(data, 0, data.Length);
	Ylluna ylluna = new Ylluna(data, readed);
	if (ylluna)
	{
		Console.WriteLine("Response Area:\r\n---------------------------------------------------------");
		Console.WriteLine(ylluna.httpVersionTag);
		Console.WriteLine(ylluna.statusCode);
		Console.WriteLine(ylluna.statusMessage);
		foreach (string someKey in ylluna.headers.headers.Keys)
			Console.WriteLine(") {0}: {1}", someKey, ylluna.headers[someKey]);
		Console.WriteLine("bodyInit: {0}", ylluna.bodyInit);
		ylluna.Dispose();
		fs.Close();
	}
	else
		Console.WriteLine("They are mistakes");

	fs = new FileStream("request.txt", FileMode.Open);
	readed = fs.Read(data, 0, data.Length);
	Anully anully = new Anully(data, readed);

	if (anully)
	{
		Console.WriteLine("Request Area:\r\n---------------------------------------------------------");
		Console.WriteLine(anully.method);
		Console.WriteLine(anully.httpVersionTag);
		Console.WriteLine(anully.hReference.path);
		Console.WriteLine(anully.hReference.queries);
		foreach (string someKey in anully.headers.headers.Keys)
			Console.WriteLine(") {0}: {1}", someKey, anully.headers[someKey]);
		Console.WriteLine("bodyInit: {0}", anully.bodyInit);
		anully.Dispose();
		fs.Close();
	}
	else
		Console.WriteLine("They are mistakes");
```

> You can use it to build strongs HTTP/S Agents Delegators