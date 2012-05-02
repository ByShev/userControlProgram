using System;
using System.Net;
using System.IO;
using System.Threading;
using Ionic.Zip;
using System.Net.Mail;
using System.Timers;
using Timer = System.Timers.Timer;

namespace userControlProgram
{
    static class MailSender
    {
        private static string _path;
        private static MailAddress _email;
        public static void MainSenderFunc(string path, MailAddress email, int interval)
        {
            _path = path;
            _email = email;

            Timer timerSender = new Timer(interval);
            timerSender.Start();
            timerSender.Elapsed += OnTimerTick;
        }

        private static bool Package(string zipName)
        {
            try
            {
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.AddDirectory(_path);
                    zip.Save(zipName);
                }
            }
            catch (Exception ex)
            {
                var log = new StreamWriter(_path + "\\report", true);
                log.WriteLine("Error in package files. Message: '" + ex.Message + "'\n");
                log.Flush();
                log.Close();
                return false;
            }
            return true;
        }

        private static void SendPackage(object zipName)
        {
            // send mail
            try
            {
                var client = new SmtpClient();
                client.Host = "smtp.yandex.ru";
                client.Credentials = new NetworkCredential("logger.usercontrol@yandex.ru", "1234567890987654321");

                var mailMessage = new MailMessage
                                      {
                                          From = new MailAddress("logger.usercontrol@yandex.ru", "Administrator"),
                                          Sender = new MailAddress("logger.usercontrol@yandex.ru", "Administrator")
                                      };
                mailMessage.To.Add(_email);
                mailMessage.Subject = "Log form " + DateTime.Now.DayOfWeek + ", " + DateTime.Now.Month + " " +
                                      DateTime.Now.Day + "; " + DateTime.Now.Hour + " hours.";
                mailMessage.Body = @"";
                mailMessage.IsBodyHtml = false;
                mailMessage.Priority = MailPriority.High;

                var attachment = new Attachment(zipName.ToString(), System.Net.Mime.MediaTypeNames.Application.Zip);

                mailMessage.Attachments.Add(attachment);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                var log = new StreamWriter(_path + "\\report", true);
                log.WriteLine("Error in sending files. Message: '" + ex.Message + "'\n");
                log.Flush();
                log.Close();
            }

            // delete package
            
            var deleteThread = new Thread(DeleteFun);
            deleteThread.Start(zipName);
        }

        private static void OnTimerTick(object o, ElapsedEventArgs e)
        {
            var senderTread = new Thread(ManagerThread);
            string zipName = "C:\\Users\\Public\\log_" + DateTime.Now.Day + "d"+ DateTime.Now.Hour + "h" +
                         DateTime.Now.Minute + "m" + ".zip";
            senderTread.Start(zipName);
        }

        private static void ManagerThread(object zipName)
        {
            if (!Package(zipName.ToString())) return;
            var sender = new Thread(SendPackage);
            sender.Start(zipName);
        }

        private static void DeleteFun(object zipName)
        {
            try
            {
                var dir = new DirectoryInfo(_path);
                var files = dir.GetFiles();

                foreach (var fileInfo in files)
                {
                    File.Delete(fileInfo.FullName);
                }

                dir = new DirectoryInfo("C:\\Users\\Public\\");
                files = dir.GetFiles();

                foreach (var fileInfo in files)
                {
                    if (fileInfo.Name.Contains("log_"))
                        File.Delete(fileInfo.FullName);
                }
            }
            catch (Exception ex)
            {
                var log = new StreamWriter(_path + "\\report", true);
                log.WriteLine("Error in deliting files. Message: '" + ex.Message + "'\n");
                log.Flush();
                log.Close();
            }
        }
    }
}
