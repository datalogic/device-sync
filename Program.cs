using datalogic_ce_sync;
using CommandLine;
using System;
using System.Text;
using System.Collections.Generic;

namespace DeviceSync
{

	class Program
	{
		static void Main(string[] args)
		{
			var result = Parser.Default.ParseArguments<
				PushOptions, PullOptions, StartOptions, FindOptions, MkdirOptions, TouchOptions, DeleteOptions>(args)
				.MapResult(
				(PushOptions opts) => RunPushAndReturnExitCode(opts),
				(PullOptions opts) => RunPullAndReturnExitCode(opts),
				(StartOptions opts) => RunStartAndReturnExitCode(opts),
				(FindOptions opts) => RunFindAndReturnExitCode(opts),
				(MkdirOptions opts) => RunMkdirAndReturnExitCode(opts),
				(TouchOptions opts) => RunTouchAndReturnExitCode(opts),
				(DeleteOptions opts) => RunDeleteAndReturnExitCode(opts),
				errs => 1);
		}

		private static object RunPushAndReturnExitCode(PushOptions opts)
		{
			Console.WriteLine("PushFileToDevice(" + opts.Source + ", " + opts.Destination + ", true)");

			try
			{
				bool res = USBLAN.PushFileToDevice(opts.Source, opts.Destination, true);
				if (!res)
				{
					Console.WriteLine("An error occurred.");
					return 1;
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("push complete.");
			return 0;
		}

		private static object RunPullAndReturnExitCode(PullOptions opts)
		{
			Console.WriteLine("PullFileFromDevice(" + opts.Source + ", " + opts.Destination + ", true)");

			try
			{
				bool res = USBLAN.PullFileFromDevice(opts.Source, opts.Destination, true);
				if (!res)
				{
					Console.WriteLine("An error occurred.");
					return 1;
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("pull complete.");
			return 0;
		}

		private static object RunStartAndReturnExitCode(StartOptions opts)
		{
			Console.WriteLine("StartProcess(" + opts.Cmd + ", " + opts.Parms + ")");
			try
			{
				bool res = USBLAN.StartProcess(opts.Cmd, opts.Parms);
				if (!res)
				{
					Console.WriteLine("An error occurred.");
					return 1;
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("start process complete.");
			return 0;
		}

		private static object RunFindAndReturnExitCode(FindOptions opts)
		{
			Console.WriteLine("FindDirectories(" + opts.Directory + ", " + opts.Mask + ")");

			try
			{
				IEnumerable<SimpleFileInfo> dirs = USBLAN.FindDirectories(opts.Directory, opts.Mask);
				foreach (SimpleFileInfo d in dirs)
				{
					Console.WriteLine("d " + d.Name);
				}

				IEnumerable<SimpleFileInfo> files = USBLAN.FindFiles(opts.Directory, opts.Mask);
				foreach (SimpleFileInfo f in files)
				{
					Console.WriteLine("f " + f.Name);
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("find complete.");
			return 0;
		}

		private static object RunMkdirAndReturnExitCode(MkdirOptions opts)
		{
			Console.WriteLine("CreateDirectory(" + opts.Directory + ")");

			try
			{
				bool res = USBLAN.CreateDirectory(opts.Directory);
				if (!res)
				{
					Console.WriteLine("An error occurred.");
					return 1;
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("directory created.");
			return 0;
		}


		private static object RunTouchAndReturnExitCode(TouchOptions opts)
		{
			if (opts.Timestamp.CompareTo(new DateTime(1, 1, 1)) == 0)
			{
				// No date & time value was passed in (optional parameter)
				Console.WriteLine("using current date and time!");
				opts.Timestamp = DateTime.Now;
			}

			Console.WriteLine("SetFileDateTime(" + opts.Directory + ", " + opts.Timestamp + ")");

			try
			{
				bool res = USBLAN.SetFileDateTime(opts.Directory, opts.Timestamp); 
				if (!res)
				{
					Console.WriteLine("An error occurred.");
					return 1;
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("touch complete.");
			return 0;
		}

		private static object RunDeleteAndReturnExitCode(DeleteOptions opts)
		{
			Console.WriteLine("DeleteFile(" + opts.Directory + ")");

			try
			{
				bool res = USBLAN.DeleteFile(opts.Directory);
				if (!res)
				{
					Console.WriteLine("An error occurred.");
					return 1;
				}
			}
			catch (Exception e)
			{
				PrintException(e);
				return 1;
			}

			Console.WriteLine("file deleted");
			return 0;
		}

		private static void PrintException(Exception e)
		{
			if (e is AggregateException)
			{
				Console.WriteLine(AggregateExceptionString((AggregateException)e));
			}
			else
			{
				Console.WriteLine(e.Message);
			}
		}

		private static String AggregateExceptionString(AggregateException aggEx)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (Exception exInnerException in aggEx.Flatten().InnerExceptions)
			{
				Exception exNestedInnerException = exInnerException;
				do
				{
					if (!string.IsNullOrEmpty(exNestedInnerException.Message))
					{
						stringBuilder.AppendLine(exNestedInnerException.Message);
					}
					exNestedInnerException = exNestedInnerException.InnerException;
				}
				while (exNestedInnerException != null);
			}

			return stringBuilder.ToString();
		}

	}
}
