using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using FuzzySharp;
using FuzzySharp.PreProcess;
using System.Speech.Synthesis;
using System.Threading;


namespace CSAssignmentChatBot
{
    /// <summary>
    /// Interaction logic for previousOrdersPageSource.xaml
    /// </summary>
    public partial class previousOrdersPageSource : Window
    {
        public string question;
        public bool choosingOrder = false;
        public List<string> orderNumbers = new List<string>();
        public bool go = true;
        public List<string> orderedFoodsTooAdd = new List<string>();
        public int priceTotal;
        public IDictionary<List<string>, int> diction = new Dictionary<List<string>, int>();

        public string name = File.ReadAllText(@"PythonFile\nameDoc.txt");

        public previousOrdersPageSource()
        {
            InitializeComponent();
            Window window = new Window();
          

            var client = new WebClient();
            var text = client.DownloadString(@"JSON\ordersHis.json");
            Dictionary<string, order> json = JsonConvert.DeserializeObject<Dictionary<string, order>>(text);
            foreach (KeyValuePair<string, order> i in json)
            {
                if (i.Key == name)
                {
                    foreach (KeyValuePair<int, string[]> getFoods in i.Value.orders)
                    {
                        orderNumbers.Add(getFoods.Key.ToString());
                        orderPreviousListBox.Items.Add(new prevOrderListBoxClass { orderNumber = getFoods.Key, foods = String.Join(", ", getFoods.Value.ToList()), totalOrderedPrice = $"${getFoodPrices.getTotalPrice(getFoods.Value.ToList())}" });

                    }
                }
            }
            openedPage();



        }
        public void openedPage()
        {
            question = "Type anything to go back to the main menu!";
            speak(question);

        }
        public void answered(string answer)
        {


            prevOrderButton.Visibility = Visibility.Hidden;
            prevOrderButton2.Visibility = Visibility.Visible;
            prevOrderTextBox.Text = "";
            speak("Going back to the main menu!");
            wait(2000);





        }
        private async void wait(int seconds)
        {
            await Task.Delay(seconds);
            leave();



        }
        public void leave()
        {
            this.Close();


        }



        public void speak(string value)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice(MainMenu.accent);
            var current = speech.GetCurrentlySpokenPrompt();
            if (current != null) speech.SpeakAsyncCancel(current);
            speech.SpeakAsync(value);
            questionText.Text = value;
        }
        public void chooseOrder(string answer)
        {
            if (FuzzySharp.Process.ExtractOne(answer, orderNumbers).Score >= 60)
            {
                foreach (string i in orderNumbers)
                {
                    MainMenu.orderedFoods.Add(i);

                    MainMenu.setModeCom("orders");

                }
                var client = new WebClient();
                var text = client.DownloadString(@"JSON\ordersHis.json");
                Dictionary<string, order> json = JsonConvert.DeserializeObject<Dictionary<string, order>>(text);
                foreach (KeyValuePair<string, order> i in json)
                {
                    if (i.Key == name)
                    {
                        foreach (KeyValuePair<int, string[]> getFoods in i.Value.orders)
                        {


                            orderedFoodsTooAdd = getFoods.Value.ToList();
                            

                        }

                    }

                }
                
                /*
                speak("Ok I will update your order now");
                File.WriteAllText(@"orderedFoodsTxtFile.txt", String.Join(" ", orderedFoodsTooAdd));
                Thread.Sleep(2000);
                speak("Done!");
                MainMenu.setModeCom("orders");
                //_mainFrame.Navigate(new MainMenu());
                */
                this.Close();

            }

        }

        public void PrevOrderButton_Click(object sender, RoutedEventArgs e)
        {
            answered(prevOrderTextBox.Text);
            prevOrderTextBox.Text = "";
            prevOrderButton.IsEnabled = false;
            

        }
        public void PrevOrderButton2_Click(object sender, RoutedEventArgs e)
        {
            chooseOrder(prevOrderTextBox.Text);
        }

    }
    class previousOrders
    {
        public Dictionary<string, order> ordersFromJson { get; set; }
    }
    class prevOrderListBoxClass
    {
        public int orderNumber { get; set; }
        public string foods { get; set; }
        public string totalOrderedPrice { get; set; }
        // public int totalCost { get; set; }
    }
}

