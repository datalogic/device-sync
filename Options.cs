using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceSync
{
	class Consts
	{
		public const String AppName = "device-sync";
	}
	[Verb("push", HelpText = "Push files to a device")]
	class PushOptions
	{
		// TODO: or should we use the 'adb' model and introduce a new keyword "device-sync connect 192.168.1.1"???
		// have to store current IP as a shell variable though?

		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Value(0, HelpText = "source file to push to device", MetaName = "Source", Required = true)]
		public string Source { get; set; }

		[Value(1, HelpText = "destination where file  should be copied to on the device.", MetaName = "Destination", Required = true)]
		public string Destination { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Push file in current directory over USBLAN", 
					new PushOptions { Source = "source.txt", Destination = "/Temp/dest.txt" });
			}
		}
	}

	[Verb("pull", HelpText = "Pull files from a device")]
	class PullOptions
	{
		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Value(0, HelpText = "source file to push to device", MetaName = "Source", Required = true)]
		public string Source { get; set; }

		[Value(1, HelpText = "destination where file should be copied locally. Default is current directory", MetaName = "Destination", Required = false)]
		public string Destination { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Pull file over USBLAN",
					new PullOptions { Source = "/Temp/source.txt", Destination = "c:\\temp\\dest.txt" });
			}
		}
	}

	[Verb("start", HelpText = "Start a process on the device")]
	class StartOptions
	{
		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Value(0, HelpText = "Path to the program to execute on device", MetaName = "cmd", Required = true)]
		public string Cmd { get; set; }

		[Value(1, HelpText = "Parameters to send to exe. If using multiple parameters, seperate with a space.", MetaName = "parms", Required = false)]
		public string Parms { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Start Audio Control Panel",
					new StartOptions { Cmd = "/Windows/ctlpnl.exe", Parms = "/Windows/Audio.cpl" });
			}
		}
	}

	[Verb("find", HelpText = "Search a directory on the device for directories and files whose names satisfy the mask.")]
	class FindOptions
	{
		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Value(0, HelpText = "Path to the directory on device to search inside.", MetaName = "directory", Required = true)]
		public string Directory { get; set; }

		[Value(1, HelpText = "Name or pattern to search", MetaName = "mask", Required = true)]
		public string Mask { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Find all text files in \\Windows",
					new FindOptions { Directory = "/Windows/", Mask = "*.txt" });
			}
		}
	}

	[Verb("mkdir", HelpText = "Create a new directory on the device.")]
	class MkdirOptions
	{
		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Value(0, HelpText = "Path to the directory on device where the directory should be made", MetaName = "directory", Required = true)]
		public string Directory { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Make a \\Temp directory",
					new FindOptions { Directory = "/Temp" });
			}
		}
	}

	[Verb("touch", HelpText = "Sets the last modified time of a file on the device")]
	class TouchOptions
	{
		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Option("timestamp", HelpText = "Timestamp to set on the file")]
		public DateTime Timestamp { get; set; }

		[Value(0, HelpText = "Path to the file on the device.", MetaName = "directory", Required = true)]
		public string Directory { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Touch file, using current date and time",
					new TouchOptions { Directory = "/Temp/file.txt" });
				yield return new Example("Touch file, using a specific date and time",
					new TouchOptions { Directory = "/Temp/file.txt", Timestamp = new DateTime(2018, 12, 25) });
			}
		}
	}

	[Verb("delete", HelpText = "Delete a specified file from the device")]
	class DeleteOptions
	{
		[Option("ip", HelpText = "IP address of device.")]
		public string IP { get; set; }

		[Value(0, HelpText = "Path to the file on the device.", MetaName = "directory", Required = true)]
		public string Directory { get; set; }

		[Usage(ApplicationAlias = Consts.AppName)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("Delete a file over USBLAN",
					new DeleteOptions { Directory = "/Temp/file.txt" });
			}
		}
	}

}
