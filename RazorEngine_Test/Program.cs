using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace RazorEngine_Test
{
    static class Program
    {
        private static readonly UnitOfWork UnitOfWork = new UnitOfWork();

        // set up a free account at https://mailtrap.io and change the configuration settings in the app.config
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && HelpRequired(args[0]))
                {
                    DisplayHelp();
                }
                else if (args.Length >= 1)
                {
                    foreach (string arg in args)
                    {
                        Logger.Info($"Operation {arg} executing.");
                        ExecuteOperation(arg);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Details: {e.Message}\r\nStack: {e.StackTrace}");
            }
        }

        private static bool HelpRequired(string param)
        {
            return param == "-h" || param == "--help" || param == "?" || param == "help";
        }

        private static void ExecuteOperation(string arg)
        {
            switch (arg)
            {
                case "sendE":
                    SendTestEmail();
                    break;
                case "sendM":
                    SendTestMailMessage();
                    break;
                case "sendEx":
                    UnitOfWork.SendTestErrorEmail();
                    break;
                case "openLog":
                    OpenErrorLog();
                    break;
                case "version":
                    Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                    break;
            }
        }

        private static void SendTestEmail()
        {
            try
            {
                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("58b5ebb8113684", "8189272b98d556"),
                    EnableSsl = true
                };
                client.Send("from@example.com", "to@example.com", "Hello world", "testbody");
                Console.WriteLine("Sent");
            }
            catch (Exception e)
            {
                Logger.Error($"Error Details: {e.Message}\r\nStack: {e.StackTrace}");
                Console.WriteLine("Exception encountered. See log.");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void SendTestMailMessage()
        {
            try
            {
                TemplatedErrorEmail.SendFinal("testbody", "Hello world");
                Console.WriteLine("Sent");
            }
            catch (Exception e)
            {
                Logger.Error($"Error Details: {e.Message}\r\nStack: {e.StackTrace}");
                Console.WriteLine("Exception encountered. See log.");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void OpenErrorLog()
        {
            try
            {
                using (Process exeProcess = Process.Start("notepad.exe", "RazorEngine_Test.log"))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch(Exception e)
            {
                Logger.Error($"Error Details: {e.Message}\r\nStack: {e.StackTrace}");
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Options:");
            Console.WriteLine("\t-h, --help, ?, or help\tShows Help Screen.");
            Console.WriteLine("\tversion\t\tDisplay Version Number.");
            Console.WriteLine("\tsendE\t\tSend out SmtpClient email using mailtrap.io");
            Console.WriteLine("\tsendM\t\tSend out MailMessage email using mailtrap.io with configuration settings ");
            Console.WriteLine("\tsendEx\t\tSend out RazorEngine Template Email");
            Console.WriteLine("\topenLog\t\tOpen the logfile in Notepad");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Enter a command to continue...");
            string operation = Console.ReadLine();
            ExecuteOperation(operation);
        }
    }
}
