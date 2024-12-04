using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
	internal class Program
	{
		static IPAddress ServerIpAddress;
		static int ServerPort;
		static int MaxClient;
		static int Duration;
		static void Main(string[] args)
		{

		}
		static void ConnectServer()
		{
			if (BlackList.Contains(ClientToken))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Client in black list. Connection denied.");
			}

			IPEndPoint EndPoint = new IPEndPoint(ServerIpAddress, ServerPort);
			Socket Socket = new Socket(
					AddressFamily.InterNetwork,
					SocketType.Stream,
					ProtocolType.Tcp);

			try
			{
				Socket.Connect(EndPoint);
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error: " + ex.Message);
			}
		}
		static void GetStatus()
		{
			int Duration = (int)DateTime.Now.Subtract(ClientDateConnection).TotalSeconds;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"Client: {ClientToken}, time connection: {ClientDateConnection.ToString("HH:mm:ss dd.MM")}, duration: {Duration}");
		}
		static void SetCommand()
		{
			Console.ForegroundColor = ConsoleColor.Red;
			string Command = Console.ReadLine();

			if (Command == "/config")
			{
				File.Delete(Directory.GetCurrentDirectory() + "/.config");
				OnSettings();
			}
			else if (Command == "/connect") ConnectServer();
			else if (Command == "/status") GetStatus();
			else if (Command == "/help") Help();
			else if (Command == "/blacklist")
			{
				string tokenToBlacklist = Command.Substring("/blacklist ".Length).Trim();
				AddBlackList(tokenToBlacklist);
			}
		}
		static void OnSettings()
		{
			string Path = Directory.GetCurrentDirectory() + "/.config";
			string IpAddress = "";

			if (File.Exists(Path))
			{
				StreamReader streamReader = new StreamReader(Path);
				IpAddress = streamReader.ReadLine();
				ServerIpAddress = IPAddress.Parse(IpAddress);
				ServerPort = int.Parse(streamReader.ReadLine());
				MaxClient = int.Parse(streamReader.ReadLine());
				Duration = int.Parse(streamReader.ReadLine());
				streamReader.Close();

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Server address: ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(IpAddress);

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Server port: ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(ServerPort.ToString());

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Max count clients: ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(MaxClient.ToString());

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Token lifttime: ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(Duration.ToString());
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Please provide the IP address if the license server: ");
				Console.ForegroundColor = ConsoleColor.Green;
				IpAddress = Console.ReadLine();
				ServerIpAddress = IPAddress.Parse(IpAddress);

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Please specify the license server port: ");
				Console.ForegroundColor = ConsoleColor.Green;
				ServerPort = int.Parse(Console.ReadLine());

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Please indicate the largest number of clients: ");
				Console.ForegroundColor = ConsoleColor.Green;
				MaxClient = int.Parse(Console.ReadLine());

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("Specify the token lifetime: ");
				Console.ForegroundColor = ConsoleColor.Green;
				Duration = int.Parse(Console.ReadLine());

				StreamWriter streamWriter = new StreamWriter(Path);
				streamWriter.WriteLine(IpAddress);
				streamWriter.WriteLine(ServerPort.ToString());
				streamWriter.WriteLine(MaxClient.ToString());
				streamWriter.WriteLine(Duration.ToString());
				streamWriter.Close();
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("To change, write the command: ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("/config");
		}
		static void Help()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Commands to the server: ");

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("/config");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(" - set initial settings");

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("/connect");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(" - connection to the server");

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("/status");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(" - show list users");
		}
	}
}
