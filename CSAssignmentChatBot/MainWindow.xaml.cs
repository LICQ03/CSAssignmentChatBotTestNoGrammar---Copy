using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Speech.Synthesis;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.NameFind;
using OpenNLP.Tools.Tokenize;
using System.Diagnostics;
using OpenNLP.Tools;
using edu.stanford.nlp.simple;
using java.util;
using SharpNL;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System.Net;
using System.Web;


namespace CSAssignmentChatBot
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool exists = false;

        public int currentOrderNum = 0;

        public MainMenu page = new MainMenu();
        SpeechSynthesizer speech = new SpeechSynthesizer();

        string name; 

        public string accent = File.ReadAllText(@"accent.txt");

        private void run_cmd()
        {
            string tempFilename = Path.ChangeExtension(Path.GetTempFileName(), ".bat");
            using (StreamWriter writer = new StreamWriter(tempFilename))
            {
                writer.WriteLine("cd PythonFile");
                writer.WriteLine("python getNLP.py");
            }
            ProcessStartInfo pr = new ProcessStartInfo();
            pr.CreateNoWindow = true;
            using (Process process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = tempFilename;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                
                process.WaitForExit();
                File.Delete(tempFilename);
            }
                
            

            name = File.ReadAllText(@"PythonFile\nameDoc.txt");
        }



        public MainWindow()
        {
           

            InitializeComponent();
            //Window window = new Window();
            //window.Owner = this;
            //_NavigationFrame.NavigationService.Navigate(new previousOrdersPageSource());
            speech.SelectVoice(accent);
            /*if (File.Exists("name.txt") && new FileInfo("name.txt").Length != 0)
            {
                speech.Speak("Welcome Back");
                speech.Speak(File.ReadAllText(@"name.txt"));
                this.Content = page;
            }*/
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            // enterButton.Background = Brushes.White;
        }




        public void Button_Click(object sender, RoutedEventArgs e)
        {

            File.WriteAllText(@"PythonFile\sentenceName.txt", nameText.Text);

            run_cmd();

            // File.Create("name.txt");
            //File.WriteAllText(nameText.Text, "name.txt");
            var client = new WebClient();
            var text = client.DownloadString(@"JSON\orders.json");

            var output = JsonConvert.DeserializeObject<List<Values>>(text);
            var dictionary = output.ToDictionary(x => x.Key, y => y.Value);

            foreach (KeyValuePair<int, string> k in dictionary)
            {
                currentOrderNum += 1;
                if (k.Value == name)
                {
                    speech.Speak($"Welcome back {name}");
                    exists = true;
                   
                }
            }

            if (!exists)
            {
                // IDictionary<int, string> diction = new Dictionary<int, string>();
                List<Values> lst = new List<Values>();
                foreach (KeyValuePair<int, string> k in dictionary)
                {
                    lst.Add(new Values()
                    {

                        Key = k.Key,
                        Value = k.Value,

                    });
                }


                lst.Add(new Values()
                {
                    Key = currentOrderNum + 1,
                    Value = name,
                });

                DumpAsJson(lst);
                //File.WriteAllText(@"JSON\orders.json", JsonConvert.SerializeObject(dictionary));
                speech.Speak($"Welcome {name}");

                speech.Speak($"Hello Welcome To the chat restaurantaria Italia. here we will make you comfortable and safe as we navigate through the corona virus, my name is licquorice. This is an easy, to use App, which will allow you to easily, order food from our 5, or five star range, and we, will surely fill your appetite. Welcome honorable guest {name} ");

                //if (File.ReadAllText(@"name.txt").ToLower() == "william" || File.ReadAllText(@"name.txt").ToLower() == "will") speech.Speak("That name by the way is great");
                this.Content = page;
            }
            this.Content = page;

            // if (File.Exists("name.txt")) nameText.Text = File.ReadAllText("name.txt");


            //this.NavigationService.Navigate(typeof(MainWindow), nameText.Text);


        }
        private static void DumpAsJson(object data)
        {
            
            string json = JsonConvert.SerializeObject(data);

            using (StreamWriter file = File.CreateText(@"JSON\orders.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
            SpeechSynthesizer speech = new SpeechSynthesizer();
            
     

        }
        public void goToPreviousOrders()
        {
                
            //previousOrdersPageSource page = new previousOrdersPageSource();
            this.Content = new previousOrdersPageSource();
        }


    }
    public class Values
    {
        public int Key { get; set; }
        public string Value { get; set; }

    }
}








