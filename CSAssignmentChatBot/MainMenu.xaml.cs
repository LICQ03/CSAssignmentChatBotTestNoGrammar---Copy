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
using System.ComponentModel;

using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Speech.Synthesis;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using FuzzySharp;
using Newtonsoft.Json;
using FuzzySharp.PreProcess;
using System.Net;
using System.Web;
using System.Windows.Media.Animation;

namespace CSAssignmentChatBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public string questionAnswer;
        public string answeredRequest;
        public string currentQuestion;
        public bool beenThrough = false;
        public bool questionRight;

        public bool go = false;
        public IDictionary<string, int> AllfoodPrices = new Dictionary<string, int>();
        public IDictionary<string, int> foodPrices = new Dictionary<string, int>();
        public IDictionary<string, int> foodPricesEntree = new Dictionary<string, int>();
        public IDictionary<string, int> foodPricesDessert = new Dictionary<string, int>();
        IDictionary<string, int> orderedFoodDiction = new Dictionary<string, int>();

        Dictionary<int, string[]> totalOrdered = new Dictionary<int, string[]>();

        int changeDetection = 0;
        int counter = 0;
        bool menuStatus = false;
        string lineUpNow = "";
        static string myfile = @"food.txt";
        public string numOrder = "";
        public string currentMode;
        string textString = "";
        bool orderStatus = false;

        List<string> questionAnswerList = new List<string>();
        List<string> questionFalseList = new List<string>();

        public int orderNumVal;

        public List<string> foodCounter = new List<string>();
        public bool isPrevious;

        // SpeechSynthesizer speech = new SpeechSynthesizer();

        //my fuzzy logic concept
        public static List<string> finish = new List<string>() { "end", "finish", "stop", "pause", "finished", "done", "leave", "escape", "exit" };
        public static List<string> yes = new List<string>() { "yep", "yes", "ok", "okay", "maybe", "perfect", "sure" };
        public static List<string> show = new List<string>() { "show", "appear", "indicate" };
        public static List<string> delete = new List<string>() { "delete", "disable", "out", "take", "remove", "removed", "destroy" };
        public static List<string> disable = new List<string>() { "off", "disable", "close", "shut" };
        public static List<string> bye = new List<string>() { "goodbye", "bye", "seeya", "exit" };
        public static List<string> all = new List<string>() { "everything", "all" };
        public static List<string> open = new List<string>() { "see", "view", "open", "show", "access" };
        public static List<string> begin = new List<string>() { "begin", "start" };

        public string orderedFood;
        //SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

        GrammarBuilder startOrder = new GrammarBuilder("order");
        GrammarBuilder hello = new GrammarBuilder("hello");
        GrammarBuilder hi = new GrammarBuilder("hi");
        GrammarBuilder openMenu = new GrammarBuilder("menu");

        GrammarBuilder disableMenu = new GrammarBuilder("disable menu");
        GrammarBuilder openOrder = new GrammarBuilder("open order");
        GrammarBuilder viewOrder = new GrammarBuilder("view order");
        GrammarBuilder showOrder = new GrammarBuilder("show order");
        GrammarBuilder speakMenu = new GrammarBuilder("tell me the menu");

        public static List<string> orderedFoods = new List<string>();

        public string name = "";

        //GrammarBuilder time = new GrammarBuilder("whats the time");

        public static string accent = File.ReadAllText(@"accent.txt");
        public string[] foodsEntree = File.ReadAllLines(@"foodEntree.txt");
        public string[] foodsDessert = File.ReadAllLines(@"foodDessert.txt");
        public string[] foods = File.ReadAllLines(@"food.txt");

        public List<string> foodList = new List<string>();
        public List<string> functionList = new List<string>();
        public bool finishStartup = false;
        public bool stopSpeech = false;
        string[] foodOrder = new string[] { };
        public int countReplay = 0;
        List<string> said = new List<string> { };
        public int countThroughArray = 0;
        public int priceTotal;

        // public Choices foodToOrder = new Choices(GrammarBuilder[] {foods});
        private async void waitUp()
        {
            List<string> commandsBegin = new List<string>() { "Open Orders", "Open Entree Dessert or Main Menu", "Look at previous orders", "Or Begin Your Order" };

            await Task.Delay(1000);
            aiReply("Would you like to:");
            await Task.Delay(2000);

            foreach (string i in commandsBegin)
            {
                aiReply(i);
                Thread.Sleep(2000);
            }
            aiReply("Once you begin your order you will be prompted to view your previous orders");
            await Task.Delay(5000);
            aiReply("Have fun");
        }


        void OnLoad(object sender, RoutedEventArgs e)
        {
            currentMode = "functions";
            Window window = new Window();
            //if(File.Exists(@"name.txt")) name = File.ReadAllText(@"name.txt");
            waitUp();
         
            name = File.ReadAllText(@"PythonFile\nameDoc.txt");

            if (!File.Exists(@"accent.txt"))
            {

                File.Create(@"accent.txt");
                File.WriteAllText(@"accent", "Microsoft Elsa Desktop");

            }
            if (!File.Exists(@"name.txt")) File.Create(@"name.txt");

            foreach (string i in foods)
            {

                lineUpNow = i;
                lineUpNow = i.Remove(i.IndexOf(":") + 1);
                lineUpNow = lineUpNow.Replace(":", "");
                var money = Convert.ToInt16(i.Substring(i.IndexOf(":") + 1));
                foodPrices.Add(lineUpNow, money);

                AllfoodPrices.Add(lineUpNow, money);

                // foods[countThroughArray] = lineUpNow;
                foodList.Add(lineUpNow);
                countThroughArray += 1;

            }
            foreach (string i in foodsEntree)
            {

                lineUpNow = i;
                lineUpNow = i.Remove(i.IndexOf(":") + 1);
                lineUpNow = lineUpNow.Replace(":", "");
                var money = Convert.ToInt16(i.Substring(i.IndexOf(":") + 1));
                foodPricesEntree.Add(lineUpNow, money);
                AllfoodPrices.Add(lineUpNow, money);


                // foods[countThroughArray] = lineUpNow;
                foodList.Add(lineUpNow);
                countThroughArray += 1;

            }
            foreach (string i in foodsDessert)
            {

                lineUpNow = i;
                lineUpNow = i.Remove(i.IndexOf(":") + 1);
                lineUpNow = lineUpNow.Replace(":", "");
                var money = Convert.ToInt16(i.Substring(i.IndexOf(":") + 1));
                foodPricesDessert.Add(lineUpNow, money);

                AllfoodPrices.Add(lineUpNow, money);

                // foods[countThroughArray] = lineUpNow;
                foodList.Add(lineUpNow);
                countThroughArray += 1;

            }

            foreach (KeyValuePair<string, int> i in foodPricesEntree)
            {
                string upperStart = i.Key;
                upperStart = char.ToUpper(upperStart[0]) + i.Key.Substring(1);
                // STT.Text = i.Key + i.Value;

                List<Menu> menuItems = new List<Menu>();

                menuItems.Add(new Menu() { food = upperStart, price = $"${i.Value.ToString()}" });
                MenuListEntree.Items.Add(menuItems);
                MenuList.Items.Add(menuItems);



            }
            //foodPrices.ToList().ForEach(x => STT.Text = x.Value + x.Key);
            foreach (KeyValuePair<string, int> i in foodPrices)
            {
                string upperStart = i.Key;
                upperStart = char.ToUpper(upperStart[0]) + i.Key.Substring(1);
                // STT.Text = i.Key + i.Value;
                List<Menu> menuItems = new List<Menu>();
                menuItems.Add(new Menu() { food = upperStart, price = $"${i.Value.ToString()}" });
                MenuListMain.Items.Add(menuItems);
                MenuList.Items.Add(menuItems);


            }

            foreach (KeyValuePair<string, int> i in foodPricesDessert)
            {
                string upperStart = i.Key;
                upperStart = char.ToUpper(upperStart[0]) + i.Key.Substring(1);
                // STT.Text = i.Key + i.Value;

                List<Menu> menuItems = new List<Menu>();

                menuItems.Add(new Menu() { food = upperStart, price = $"${i.Value.ToString()}" });
                MenuListDessert.Items.Add(menuItems);
                MenuList.Items.Add(menuItems);


            }

        }
        public void openFoodMenu(object sender, RoutedEventArgs e)
        {

            menuPage.Visibility = Visibility.Visible;

        }
        public void toggleMenuOff(object sender, RoutedEventArgs e)
        {
            openMenuButton.Content = "Open Menu";

            menuPage.Visibility = Visibility.Hidden;
            menuStatus = false;

        }
        public void toggleMenuOn(object sender, RoutedEventArgs e)
        {
            openMenuButton.Content = "Close Menu";
            menuPage.Visibility = Visibility.Visible;
            menuStatus = true;

        }
        public void toggleOrderOn(object sender, RoutedEventArgs e)
        {
            openOrderButton.Content = "Close Orders";
            orders.Visibility = Visibility.Visible;
            orderStatus = true;


        }
        public void toggleOrderOff(object sender, RoutedEventArgs e)
        {
            openOrderButton.Content = "Open Orders";
            orders.Visibility = Visibility.Hidden;
            orderStatus = false;

        }
        public void toggleMenuOnEntree(object sender, RoutedEventArgs e)
        {
            openMenuEntree.Content = "Close Entrees";
            menuPageEntree.Visibility = Visibility.Visible;
            menuStatus = true;
        }
        public void toggleMenuOffEntree(object sender, RoutedEventArgs e)
        {
            openMenuEntree.Content = "Open Entrees";
            menuPageEntree.Visibility = Visibility.Hidden;
            menuStatus = false;
        }
        private void toggleMenuOffDessert(object sender, RoutedEventArgs e)
        {
            openMenuDessert.Content = "Open Dessert";
            menuPageDessert.Visibility = Visibility.Hidden;
            menuStatus = false;
        }
        private void toggleMenuOnDessert(object sender, RoutedEventArgs e)
        {
            openMenuDessert.Content = "Close Dessert";
            menuPageDessert.Visibility = Visibility.Visible;
            menuStatus = true;
        }
        private void toggleMenuOffMain(object sender, RoutedEventArgs e)
        {
            openMenuMain.Content = "Open Main";
            menuPageMain.Visibility = Visibility.Hidden;
            menuStatus = false;
        }
        private void toggleMenuOnMain(object sender, RoutedEventArgs e)
        {
            openMenuMain.Content = "Close Main";
            menuPageMain.Visibility = Visibility.Visible;
            menuStatus = true;
        }

        private void SettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            Thickness margin = settingsButton.Margin;
            settingsPage.Visibility = Visibility.Visible;

            settingsButton.Margin = margin;

        }

        private void SettingsButton_Unchecked(object sender, RoutedEventArgs e)
        {

            settingsPage.Visibility = Visibility.Hidden;
            Thickness margin = settingsButton.Margin;

            settingsButton.Margin = margin;
        }
        private void AccentChoose_John_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"accent", "Microsoft David Desktop");
            accent = "Microsoft David Desktop";
        }

        private void AccentChoose_Jana_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"accent", "Microsoft Elsa Desktop");
            accent = "Microsoft Elsa Desktop";
        }



        public MainMenu()
        {

            InitializeComponent();

            if (!File.Exists(@"order.txt"))
            {
                File.Create(@"order.txt");
            }



            /* if (name != null)
             {
                 finishStartup = true;
                 STT.Text = name;
             }
            if (!finishStartup)
             {

                 

             }
             */
        }


        public void askSpeech(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SetOutputToDefaultAudioDevice();
            speech.Speak("Hello World");
        }
        public void askSpeechRecognition(object sender, RoutedEventArgs e)
        {

        }
        public void speechTask(string said)
        {

            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice(accent);

            STT.Text = speakInput.Text;


            string Result = said;
            string spokenWords = said;

            if (currentMode == "functions")
            {
                go = false;


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "main") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Main Courses");

                    menuStatus = true;
                    menuPageMain.Visibility = Visibility.Hidden;
                    go = true;
                }


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "entree") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Entree Courses");

                    menuStatus = true;
                    menuPageEntree.Visibility = Visibility.Hidden;
                    go = true;
                }


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "dessert") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Dessert Courses");

                    menuStatus = true;
                    menuPageDessert.Visibility = Visibility.Hidden;
                    go = true;
                }


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "menu") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Main Menu");

                    menuStatus = true;
                    menuPage.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "main") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Main Courses");

                    menuStatus = true;
                    menuPageMain.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "main") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Main Courses");

                    menuStatus = false;
                    menuPageMain.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "entree") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Entree Courses");

                    menuStatus = true;
                    menuPageEntree.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "entree") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Entree Courses");

                    menuStatus = false;
                    menuPageEntree.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "dessert") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Dessert Courses");

                    menuStatus = true;
                    menuPageDessert.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "dessert") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Dessert Courses");

                    menuStatus = false;
                    menuPageDessert.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "menu") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Main Menu");

                    menuStatus = true;
                    menuPage.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "menu") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Main Menu");

                    menuStatus = false;
                    menuPage.Visibility = Visibility.Hidden;
                    go = true;
                }

                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "order") >= 60 && !orderStatus && !go)
                {
                    aiReply("Opening Orders");
                    orders.Visibility = Visibility.Visible;
                    orderStatus = true;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "order") >= 60 && orderStatus && !go)
                {
                    aiReply("Close Orders");

                    orders.Visibility = Visibility.Hidden;
                    orderStatus = false;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, begin).Score >= 60 && Fuzz.PartialRatio(Result, "order") >= 60 && !go)
                {
                    currentMode = "orders";
                    aiReply("You can now choose to fill your plate with the worlds most gracious food cousine.");
                    Thread.Sleep(5000);
                    //foodCounter.Clear();
                    checkPrevious(name);
                    if (isPrevious)
                    {
                        aiReply("It seems as though you have successfully made a previous order.");
                        
                        askQuestion("Would you like to load a previous order?", new List<string> { "yes", "no", "yep", "sure" }, new List<string> { "no", "nope", "nah", "false" });
                    }
                    go = true;
                }

                if (FuzzySharp.Process.ExtractOne(Result, finish).Score >= 60 && !go)
                {
                    aiReply($"It was a pleasure serving you {name}");
                    Thread.Sleep(2000);
                    if (orders.Items.Count <= 0) aiReply($"it was a shame {name} that you couldn't order any food, maybe next time! "); Thread.Sleep(5000);

                    Environment.Exit(0);
                    go = true;
                }
                if(FuzzySharp.Fuzz.PartialRatio(Result, "previous orders") >= 60)
                {
                    askQuestion("Would you like to load a previous order?", new List<string> { "yes", "no", "yep", "sure" }, new List<string> { "no", "nope", "nah", "false" });
                }
                go = false;
                
            }
           
            if (currentMode == "orders")
            {

                go = false;
                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "main") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Main Courses");

                    menuStatus = true;
                    menuPageMain.Visibility = Visibility.Hidden;
                    go = true;
                }


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "entree") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Entree Courses");

                    menuStatus = true;
                    menuPageEntree.Visibility = Visibility.Hidden;
                    go = true;
                }


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "dessert") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Dessert Courses");

                    menuStatus = true;
                    menuPageDessert.Visibility = Visibility.Hidden;
                    go = true;
                }


                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "menu") >= 60 && !menuStatus && !go)
                {
                    aiReply("Closing Main Menu");

                    menuStatus = true;
                    menuPage.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "main") >= 60 && menuStatus && !go)
                {
                    aiReply("Opening Main Courses");

                    menuStatus = true;
                    menuPageMain.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "main") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Main Courses");

                    menuStatus = false;
                    menuPageMain.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "entree") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Entree Courses");

                    menuStatus = true;
                    menuPageEntree.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "entree") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Entree Courses");

                    menuStatus = false;
                    menuPageEntree.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "dessert") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Dessert Courses");

                    menuStatus = true;
                    menuPageDessert.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "dessert") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Dessert Courses");

                    menuStatus = false;
                    menuPageDessert.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "menu") >= 60 && !menuStatus && !go)
                {
                    aiReply("Opening Main Menu");

                    menuStatus = true;
                    menuPage.Visibility = Visibility.Visible;
                    go = true;

                }

                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "menu") >= 60 && menuStatus && !go)
                {
                    aiReply("Closing Main Menu");

                    menuStatus = false;
                    menuPage.Visibility = Visibility.Hidden;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, open).Score >= 60 && Fuzz.PartialRatio(Result, "order") >= 60 && !orderStatus && !go)
                {
                    openOrderButton.Content = "Open Orders";
                    aiReply("Open Order");
                    //reply.Text = reply.Text + "\n" + "Opening Orders";
                    orders.Visibility = Visibility.Visible;
                    orderStatus = true;
                    go = true;
                }
                if (FuzzySharp.Process.ExtractOne(Result, disable).Score >= 60 && Fuzz.PartialRatio(Result, "order") >= 60 && orderStatus && !go)
                {
                    openOrderButton.Content = "Close Orders";
                    reply.Text = reply.Text + "\n" + "Close Orders";
                    orders.Visibility = Visibility.Hidden;
                    orderStatus = false;
                    go = true;
                }

                if (FuzzySharp.Process.ExtractOne(Result, foodList).Score >= 60 && FuzzySharp.Fuzz.PartialRatio(Result, "order") >= 60 && !go)
                {

                    string eResult = FuzzySharp.Process.ExtractOne(Result, foodList).Value;
                    string firstLetter = char.ToUpper(eResult[0]) + eResult.Substring(1);
                    counter++;
                    //orderedFoodDiction.Add(firstLetter, counter);
                    //numOrder = orderedFoodDiction[firstLetter].ToString();
                    List<User> items = new List<User>();
                    //orderedFoodDiction.Add($"{firstLetter}", foodPrices[eResult]);

                    foodCounter.Add(firstLetter);

                    items.Add(new User() { orderedFood = $"{firstLetter}", cost = $"${AllfoodPrices[eResult]}", orderNumber = orders.Items.Count + 1 });
                    orders.Items.Add(items);
                    aiReply($"Successfully added, {firstLetter} to order");
                    Thread.Sleep(4000);
                    List<string> possibleComplimentsDessert = new List<string>() { "What a fantastic Dessert I must say", "This sweetens your taste buds, more than anything","mmmmmmmmmmmmm yum" };
                    foreach(KeyValuePair<string, int> i in foodPricesDessert)
                    {
                        if(i.Key == firstLetter.ToLower())
                        {
                            var random = new Random();
                            int index = random.Next(possibleComplimentsDessert.Count());

                            aiReply(possibleComplimentsDessert[index]);
                        }
                    }
                    List<string> possibleComplimentMain = new List<string>() { "What a fantastic dish to fill up on", "most recomended", "Now this is a meal that will surely make you remember this meal", "mmmmmmmmmmmmm yum", "yes sirrrry, this is making me hungry", "I remember my mama used to cook this dish!" };
                    foreach (KeyValuePair<string, int> i in foodPrices)
                    {
                        if (i.Key == firstLetter.ToLower())
                        {
                            var random = new Random();
                            int index = random.Next(possibleComplimentMain.Count());

                            aiReply(possibleComplimentMain[index]);
                        }
                    }
                    List<string> possibleComplimentEntree = new List<string>() { "What a fantastic dish to help you start your feast", "most recomended", "Now this is a starter that will surely make you remember this meal", "mmmmmmmmmmmmm yum", "yes sirrrry, this is making me hungry", "I remember my mama used to cook this entree... aaaah memories!" };
                    foreach (KeyValuePair<string, int> i in foodPricesEntree)
                    {
                        if (i.Key == firstLetter.ToLower())
                        {
                            var random = new Random();
                            int index = random.Next(possibleComplimentEntree.Count());

                            aiReply(possibleComplimentEntree[index]);
                        }
                    }
                    go = true;

                }
                if (FuzzySharp.Process.ExtractOne(Result, delete).Score >= 60)
                {
                    if (FuzzySharp.Process.ExtractOne(Result, foodList).Score >= 60)
                    {
                        aiReply("Select the item you want removed, and then click the delete button");
                        orders.Visibility = Visibility.Visible;
                    }
                }


                if (FuzzySharp.Process.ExtractOne(Result, finish).Score >= 60 && FuzzySharp.Fuzz.PartialRatio(Result, "order") >= 60 && !go)
                {
                    if (orders.Items.Count >= 3)
                    {

                        //rooter.Add(name, new order { orders = totalOrdered });
                        finishOrder();


                    }

                    else aiReply("Oh no, you havent gotten three, or more orders. to continue you must have over, three. dishes ordered, please order more dishes");
                    go = true;
                }
                go = false;





            }

        }

        private async void finishOrder()
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice("Microsoft David Desktop");

            aiReply("Thank you for making a purchase from Italia Restaurante");
            await Task.Delay(4000);
            aiReply("Just confirming you have ordered");
            await Task.Delay(2000);
            foreach (string i in foodCounter)
            {

                aiReply(i);
                await Task.Delay(1500);

            }
            int totalToSay = getFoodPrices.getTotalPrice(foodCounter);
            //make it so they can cancel
            await Task.Delay(500);
            aiReply("That comes to a total of ");
            await Task.Delay(2000);
            speech.SpeakAsync($"{totalToSay.ToString()} Dollars");
            reply.Text = $"{ reply.Text }\n${ totalToSay.ToString() }";
            await Task.Delay(2000);
            questionBlock2.Visibility = Visibility.Visible;
            aiReply("Do you want to continue with this order?");
            await Task.Delay(2000);
            qblock2();

        }
        private void qblock2()
        {
            //IDictionary<string, order> roote = new Dictionary<string, order>();


            questionBlockQ2.Text = "Do you want to continue with this order?";

        }

        private async void qblock2Right()
        {
            var client = new WebClient();
            var text = client.DownloadString(@"JSON\ordersHis.json");
            Dictionary<string, order> json = JsonConvert.DeserializeObject<Dictionary<string, order>>(text);
            int upTo = 1;
            foreach (KeyValuePair<string, order> valuePair in json)
            {
                foreach (KeyValuePair<int, string[]> keyValue in valuePair.Value.orders)
                {
                    if (valuePair.Key == name)
                    {

                    }
                }
            }
            var rooter = new Dictionary<string, order>();
            //if (totalOrdered.ContainsKey(upTo)) upTo++;
            //totalOrdered.Add(upTo, foodCounter.ToArray());
            foreach (KeyValuePair<string, order> i in json)
            {
                rooter.Add(i.Key, i.Value);
                //else totalOrdered.Add(upTo++, foodCounter.ToArray());
            }

            if (json.ContainsKey(name))
            {

                totalOrdered.Clear();
                totalOrdered.Add(upTo, foodCounter.ToArray());
                var obj = new order { orders = totalOrdered };
                foreach (KeyValuePair<string, order> i in json)
                {
                    if (i.Key == name)
                    {
                        foreach (KeyValuePair<int, string[]> numCheck in i.Value.orders)
                        {
                            upTo = upTo + 1;
                        }
                    }

                }

                rooter[name].orders.Add(upTo, foodCounter.ToArray());


            }
            else
            {
                totalOrdered.Add(upTo++, foodCounter.ToArray());
                rooter.Add(name, new order { orders = totalOrdered });

            }
            aiReply("I will now update this fine selection of food onto your profile, so that we will be able to find it next time");
            dumpJSON.DumpAsJson(rooter);
            await Task.Delay(8000);
            aiReply("Done!");

            foodCounter.Clear();
            orders.Items.Clear();
            currentMode = "functions";
        }
        private void QuestionButton2_Click(object sender, RoutedEventArgs e)
        {
            if (questionReply2.Text == null)
            {
                aiReply("I do not understand please say yes or no");
            }
            if (FuzzySharp.Process.ExtractOne(questionReply2.Text, yes).Score >= 60)
            {
                qblock2Right();
            }
            if (FuzzySharp.Process.ExtractOne(questionReply2.Text, yes).Score < 60)
            {
                aiReply("That is okay, I have reset you current order data.");
                Thread.Sleep(3000);
                foodCounter.Clear();
                orders.Items.Clear();
            }
            questionBlock2.Visibility = Visibility.Hidden;

        }
        public static void updateCurrentOrderCom(List<string> foods)
        {

            Instance.updateCurrentOrder(foods);
        }
        public static MainMenu Instance
        {
            get { return instance; }
        }

        private static MainMenu instance = new MainMenu();
        public void updateCurrentOrder(List<string> foods)
        {
            MainMenu m = new MainMenu();
            foreach (string i in foods)
            {
                string Result = i;
                string eResult = i.ToLower();
                string firstLetter = char.ToUpper(eResult[0]) + eResult.Substring(1);
                counter++;
                //orderedFoodDiction.Add(firstLetter, counter);
                //numOrder = orderedFoodDiction[firstLetter].ToString();



                List<User> items = new List<User>();
                //orderedFoodDiction.Add($"{firstLetter}", foodPrices[eResult]);

                foodCounter.Add(firstLetter);

                items.Add(new User() { orderedFood = $"{firstLetter}", cost = $"${AllfoodPrices[eResult]}", orderNumber = orders.Items.Count + 1 });
                orders.Items.Add(items);
                aiReply($"Successfully added, {firstLetter} to order");
            }

        }


        public void askQuestion(string question, List<string> questionAnswers, List<string> falseAnswer)
        {
            questionAnswerList = questionAnswers;
            questionFalseList = falseAnswer;
            questionBlock.Visibility = Visibility.Visible;
            questionBlockQ.Text = question;
            currentQuestion = questionBlockQ.Text;
            questionBlock.Focus();

        }

        public static void setModeCom(string mode)
        {
            MainMenu m = new MainMenu();
            m.setMode(mode);
        }
        public void setMode(string mode)
        {
            currentMode = mode;
        }


        public void getPreviousOrders()
        {

        }

        public void checkPrevious(string name)
        {
            var client = new WebClient();
            var text = client.DownloadString(@"JSON\ordersHis.json");
            Dictionary<string, order> json = JsonConvert.DeserializeObject<Dictionary<string, order>>(text);
            foreach (KeyValuePair<string, order> i in json)
            {
                if (i.Key.ToLower() == name.ToLower())
                {
                    isPrevious = true;
                    break;
                }
            }
        }
        public void deleteItem(object sender, RoutedEventArgs e)
        {
            orders.Items.RemoveAt(orders.SelectedIndex);
        }
        private async void aiReply(string val)
        {

            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice(accent);
            var current = speech.GetCurrentlySpokenPrompt();

            if (current != null) speech.SpeakAsyncCancel(current);
            speech.SpeakAsync(val);


            reply.Text = $"{ reply.Text }\n{ val }";
        }
        public void speak(object sender, RoutedEventArgs e)
        {

            string Result = speakInput.Text.ToLower();
            speechTask(Result);

            /*
            foreach (string i in spokenWords.Split(' '))
            {
                if (show.Contains(i.ToLower()) && spokenWords.Split(' ').Contains("order") && !orderStatus)
                {
                    openOrderButton.Content = "Close Orders";
                    reply.Text = reply.Text + "\n" + "Closing Orders";
                    orders.Visibility = Visibility.Visible;
                    orderStatus = true;
                }
                if (disable.Contains(i.ToLower()) && spokenWords.Split(' ').Contains("order") && orderStatus)
                {
                    openOrderButton.Content = "Open Orders";
                    reply.Text = reply.Text + "\n" + "Opening Orders";
                    orders.Visibility = Visibility.Hidden;
                    orderStatus = false;
                }
                if (FuzzySharp.Process.ExtractOne(i, disable).Score >= 60 && menuStatus)
                {
                    speech.Speak("Closing Menu");
                    reply.Text = reply.Text + "\n" + "Closing Menu";
                    menuStatus = false;
                    menuPage.Visibility = Visibility.Hidden;
                    break;
                }
                if (FuzzySharp.Process.ExtractOne(i, open).Score >= 60 && !menuStatus)
                {
                    speech.Speak("Opening Menu");
                    reply.Text = reply.Text + "\n" + "Opening Menu";
                    menuStatus = true;
                    menuPage.Visibility = Visibility.Visible;

                }
                if (spokenWords.Split(' ').Contains("order") && !show.Contains(i))
                {
                    if(currentMode != "orders") reply.Text = $"{reply.Text}\nStarting Order\nNow begin to add food from\n our menu to your order";
                    currentMode = "orders";


                }
                if (bye.Contains(i.ToLower()))
                {
                    File.Delete(@"name.txt");
                    System.Environment.Exit(0);

                }*/

            /*

            if (currentMode == "orders")
            {



                //string firstLetter = char.ToUpper(orderable[0]) + e.Result.Text.Split(' ').Skip(1).FirstOrDefault().Substring(1);
                foreach (string i in Result.Split(' '))
                {
                    if (show.Contains(i.ToLower()) && Result.Split(' ').Contains("order") && !orderStatus)
                    {
                        openOrderButton.Content = "Close Orders";
                        orders.Visibility = Visibility.Visible;
                        reply.Text = $"{reply.Text}\nClosing Orders";
                        orderStatus = true;
                    }
                    if (i.ToLower() == "menu" && menuStatus)
                    {
                        speech.Speak("Closing Menu");
                        reply.Text = $"{reply.Text}\nClosing Menu";
                        menuStatus = false;
                        menuPage.Visibility = Visibility.Hidden;
                    }
                    if (i.ToLower() == "menu" && !menuStatus)
                    {
                        speech.Speak("Opening Menu");
                        reply.Text = $"{reply.Text}\nOpening Menu";
                        menuStatus = true;
                        menuPage.Visibility = Visibility.Visible;

                    }
                    if (finish.Contains(i.ToLower()))
                    {
                        speech.Speak("You are about to finish your order");
                        reply.Text = $"{reply.Text}\nYou are about to finish your order";

                        if (orders.Items.Count >= 3)
                        {
                            speech.Speak("You have ordered. ");
                            reply.Text = $"{reply.Text}\nYou have ordered";

                            foreach (string food in orderedFoods)
                            {
                                reply.Text = $"{reply.Text}\n{food}";
                                speech.Speak(food);
                                priceTotal = foodPrices[food] + priceTotal;

                            }

                            speech.Speak("Total cost is ");

                            speech.SelectVoice("Microsoft David Desktop");
                            speech.Speak(priceTotal.ToString());
                            speech.SelectVoice(accent);
                            speech.Speak("dolars");
                            reply.Text = $"{reply.Text}\nTotal cost is ${priceTotal}";
                            speech.Speak($"I have put this order under the name of, { name } ");
                            reply.Text = $"{reply.Text}\nI have put this order under the name of {name}";

                            currentMode = "functions";
                        }

                        else speech.Speak("Oh no, you havent gotten three, or more orders. to continue you must have over, three. dishes ordered, please order more dishes");
                    }
                    // close menu command

                    if (foodList.Contains(i))
                    {

                        string eResult = i;
                        string firstLetter = char.ToUpper(eResult[0]) + eResult.Substring(1);
                        counter++;
                        //orderedFoodDiction.Add(firstLetter, counter);
                        //numOrder = orderedFoodDiction[firstLetter].ToString();
                        List<User> items = new List<User>();
                        speech.SpeakAsync($"You Have Successfully Added { firstLetter } to Order");
                        //orderedFoodDiction.Add($"{firstLetter}", foodPrices[eResult]);
                        orderedFoods.Add(eResult);

                        items.Add(new User() { orderedFood = $"{firstLetter}", cost = $"${foodPrices[eResult]}", orderNumber = orders.Items.Count + 1 });
                        orders.Items.Add(items);


                    }

                    if (delete.Contains(i.ToLower()))
                    {
                        foreach (string num in Result.Split(' '))
                        {
                            if (all.Contains(num))
                            {
                                orders.Items.Clear();
                            }
                            foreach (object line in orders.Items)
                            {
                                //STT.Text = line.ToString();
                            }
                        }
                    }
                    */






        }

        /*DateTime now = DateTime.Now;
        SpeechSynthesizer speech = new SpeechSynthesizer();
        speech.SelectVoice(accent);
        speech.Speak($"Added {  speakInput.Text.Split(' ').Skip(1).FirstOrDefault() } to order");
        reply.Text = $"{reply.Text}\nAdded {  speakInput.Text.Split(' ').Skip(1).FirstOrDefault() } to order";
        if (STT.Text.ToLower() == "what's the time".Replace("'", ""))
        {
            reply.Text = "Current Time: " + now.ToShortTimeString();
            speech.Speak("Current Time: ");
            speech.Speak(now.ToShortTimeString());
            //STT.Text = now.ToShortTimeString();
        }
        if (speakInput.Text != "")
        {
            string upperStart = speakInput.Text.Split(' ').Skip(1).FirstOrDefault();
            upperStart = char.ToUpper(upperStart[0]) + speakInput.Text.Split(' ').Skip(1).FirstOrDefault().Substring(1);

            List<User> items = new List<User>();

            items.Add(new User() { orderedFood = upperStart, orderedNumber = (numOrder += 1).ToString(), cost = $"${ foodPrices[speakInput.Text.Split(' ').Skip(1).FirstOrDefault()]}" });

            orders.Items.Add(items);
            textString = speakInput.Text;
            speakInput.Text = "";
            speechRecognition(textString, e);
        }
        */

        public void EXIT(object sender, ExitEventArgs e)
        {
            File.Delete(@"name.txt");
        }
        public void New(object sender, RoutedEventArgs e)
        {
            STT.Text = "";
        }
        public void speechRecognition(object sender, RoutedEventArgs e)
        {
            var appendTofile = from line in File.ReadLines(myfile) select line;
            string[] foodArray = foodList.ToArray();
            Choices food = new Choices(foodArray);
            Grammar Foodgrammar = new Grammar(food);
            Foodgrammar.Name = "Foodgrammar";
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer.LoadGrammar(Foodgrammar);
            recognizer.LoadGrammar(new DictationGrammar());
            recognizer.SetInputToDefaultAudioDevice();
            
            recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(speech_Detected);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speech_recognized);

            if (!stopSpeech) recognizer.RecognizeAsync(RecognizeMode.Multiple);
            //if (currentMode != "orders") recognizer.RecognizeAsyncStop(); recognizer.RecognizeAsyncCancel();

        }
        public void speech_Detected(object sender, SpeechDetectedEventArgs e)
        {

        }
        public void speech_recognized(object sender, SpeechRecognizedEventArgs e)
        {
            speechTask(e.Result.Text.ToLower());
        }
        public void setOrder()
        {
            currentMode = "orders";
            var appendTofile = from line in File.ReadLines(myfile) select line;

            //Choices food = new Choices(new string[] { "spaghetti", "lasagne", "pizza", "pizza napoletana" });
            string[] foodArray = foodList.ToArray();

            //order food text
            Choices food = new Choices(foodArray);
            GrammarBuilder foodSelection = new GrammarBuilder();
            GrammarBuilder order = new GrammarBuilder("order");
            order.Append(food);
            GrammarBuilder setOrder = new GrammarBuilder("set order to");
            setOrder.Append(food);
            GrammarBuilder and = new GrammarBuilder("and");
            and.Append(food);
            GrammarBuilder pleaseOrder = new GrammarBuilder("could i please order");
            pleaseOrder.Append(food);
            GrammarBuilder couldIOrder = new GrammarBuilder("could i order");
            couldIOrder.Append(food);
            GrammarBuilder wantToOrder = new GrammarBuilder("i want to order");
            couldIOrder.Append(food);
            Choices bothChoicesOrder = new Choices(new GrammarBuilder[] { order, setOrder, pleaseOrder, couldIOrder, wantToOrder });

            //$"Hi {File.ReadAllLines(@"name.txt")}" Main Function Text




            Grammar grammar = new Grammar(food);
            grammar.Name = "orderFood";
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(speech_Detected);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speech_order);
            if (!stopSpeech) recognizer.RecognizeAsync(RecognizeMode.Single);
        }
        public void speech_order(object sender, SpeechRecognizedEventArgs e)
        {
            /*
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice(accent);
            string orderable = e.Result.Text.ToLower();

            string Result = e.Result.Text.ToLower();
            // string spokenWords = e.Result.Text.ToLower();

            STT.Text = e.Result.Text;

            string spokenWords = e.Result.Text.ToLower();

            //string firstLetter = char.ToUpper(orderable[0]) + e.Result.Text.Split(' ').Skip(1).FirstOrDefault().Substring(1);
            foreach (string i in Result.Split(' '))
            {
                if (show.Contains(i.ToLower()) && Result.Split(' ').Contains("order") && !orderStatus)
                {
                    openOrderButton.Content = "Close Orders";
                    orders.Visibility = Visibility.Visible;
                    orderStatus = true;
                }
                if (i.ToLower() == "menu" && menuStatus)
                {
                    speech.Speak("Closing Menu");
                    menuStatus = false;
                    menuPage.Visibility = Visibility.Hidden;
                }
                if (i.ToLower() == "menu" && !menuStatus)
                {
                    speech.Speak("Opening Menu");
                    menuStatus = true;
                    menuPage.Visibility = Visibility.Visible;

                }
                if (finish.Contains(i.ToLower()))
                {
                    speech.Speak("You are about to finish your order");
                    if (orders.Items.Count >= 3)
                    {
                        speech.Speak("You have ordered. ");
                        foreach (string food in orderedFoods)
                        {
                            speech.Speak(food);
                            priceTotal = foodPrices[food] + priceTotal;

                        }

                        speech.Speak("Total cost is ");
                        speech.SelectVoice("Microsoft David Desktop");
                        speech.Speak(priceTotal.ToString());
                        speech.SelectVoice(accent);
                        speech.Speak("dolars");
                        speech.Speak($"I have put this order under the name of, { name } ");
                        currentMode = "functions";
                    }
                    else speech.Speak("Oh no, you havent gotten three, or more orders. to continue you must have over, three. dishes ordered, please order more dishes");
                }
                // close menu command

                if (foodList.Contains(i.ToLower()))
                {

                    string eResult = i;
                    string firstLetter = char.ToUpper(eResult[0]) + eResult.Substring(1);
                    counter++;
                    //orderedFoodDiction.Add(firstLetter, counter);
                    //numOrder = orderedFoodDiction[firstLetter].ToString();
                    List<User> items = new List<User>();
                    speech.SpeakAsync($"You Have Successfully Added { firstLetter } to Order");
                    //orderedFoodDiction.Add($"{firstLetter}", foodPrices[eResult]);
                    orderedFoods.Add(eResult);

                    items.Add(new User() { orderedFood = $"{firstLetter}", cost = $"${foodPrices[eResult]}", orderNumber = orders.Items.Count + 1 });
                    orders.Items.Add(items);

            */
        }




        //speechRecognition();
        /*if (e.Result.Text.ToLower() == "stop")
        {
            stopSpeech = true;
            speech.Speak("You have complete your order");
            orderText.Text = String.Join("\n", foodOrder);
        }
        else
        {
            STT.Text = e.Result.Text.ToString();
            speech.Speak(e.Result.Text);
            accuracy.Text = e.Result.Confidence.ToString();
            reply.Text = reply.Text + "\n" + e.Result.Text;
            foodOrder.Append(e.Result.Text);
            orderText.Text = orderText.Text + "\n" + e.Result.Text.Split(' ').Skip(1).FirstOrDefault();
            orderedFood = e.Result.Text;
            //string upperStart = e.Result.Text.Split(' ').Skip(1).FirstOrDefault();
            orderedFood = char.ToUpper(orderedFood[0]) + e.Result.Text.Split(' ').Skip(1).FirstOrDefault().Substring(1);

            List<User> items = new List<User>();
            items.Add(new User() { orderedFood = orderedFood, orderedNumber = (numOrder += 1).ToString(), cost = $"${ foodPrices[e.Result.Text.Split(' ').Skip(1).FirstOrDefault()]}" });
            orders.Items.Add(items);
            speech.Speak(orderText.Text);

            //List<User> items = new List<User>();
            //items.Add(new User() { orderedFood = upperStart, orderedNumber = (numOrder += 1).ToString(), cost = $"${ foodPrices[e.Result.Text.Split(' ').Skip(1).FirstOrDefault()]}" });
            //orders.Items.Add(items);
            */

        /* using (var file = new StreamWriter(@"order.txt"))
         {
             file.WriteLine(items[0]);
         }*/





        public void save_File(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("create.txt")) File.Create("create.txt");
            if (File.Exists("create.txt")) File.WriteAllText("create.txt", STT.Text);
        }
        public void open_File(object sender, RoutedEventArgs e)
        {
            if (File.Exists("create.txt")) STT.Text = File.ReadAllText("create.txt");
            else
            {
                reply.Text = "There's no such saved file, please save a file first";
            }
        }
        public void get_Name()
        {
            //if(!File)
        }

        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice(accent);
            //speech.GetInstalledVoices("");
            if (e.Result.Text.ToString().ToLower() == "stop")
            {
                System.Environment.Exit(1);

            }


            speech.SetOutputToDefaultAudioDevice();
            speech.Speak("Hello World");
            STT.Text = e.Result.Text.ToString();

            speech.Speak(e.Result.Text);
        }
        private void STT_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void SpeakInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(speakButtonEnter);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }
        public void ForceOpenMenu()
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SelectVoice(accent);
            aiReply("Opening the Menu");
            ToggleButtonAutomationPeer peer = new ToggleButtonAutomationPeer(openMenuButton);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();

        }

        private void Button_MouseEnterQuestionButton(object sender, RoutedEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = Colors.Orange;
            animation.To = Colors.Gray;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            //this.questionButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void QuestionButton_Click(object sender, RoutedEventArgs e)
        {

            processAnswer(questionAnswerList, questionFalseList);


        }
        public void processAnswer(List<string> answers, List<string> falseAnswer)
        {

            questionAnswer = questionReply.Text;
            answers = questionAnswerList;

            if (FuzzySharp.Process.ExtractOne(questionAnswer, answers).Score >= 60)
            {
                answeredRequest = FuzzySharp.Process.ExtractOne(questionAnswer, answers).Value;
                questionReply.Text = questionAnswer;
                questionRight = true;
                questionBlockQ.Text = "";
                questionBlock.Visibility = Visibility.Hidden;
                getQuestion(questionRight);
            }
            if (FuzzySharp.Process.ExtractOne(questionAnswer, falseAnswer).Score >= 60)
            {
                questionRight = false;
                currentMode = "orders";
            }
            else
            {

                questionBlockQ.Text = "Please Answer The Question";
                wait();
            }



        }
        public void getQuestion(bool answeredRight)
        {
            if (answeredRight && currentQuestion == "Would you like to load a previous order?")
            {
                currentMode = "orders";
                //previousOrdersPageSource wnd = new previousOrdersPageSource();
                // this.NavigationService.Navigate(new Uri("previousOrdersPageSource.xaml", ur));
                //_MainMenu.Navigate(new previousOrdersPageSource());
                previousOrdersPageSource n = new previousOrdersPageSource();
                n.Show();


            }
            else currentMode = "orders";
        }
        /*
        FileSystemWatcher fs;
        string watchFolder = Directory.GetCurrentDirectory();
        private void startWatching()
        {
            fs = new FileSystemWatcher(watchFolder, "*.*");

            fs.EnableRaisingEvents = true;
            fs.IncludeSubdirectories = true;
            fs.Created += new FileSystemEventHandler(newfile);
            fs.Changed += new FileSystemEventHandler(fs_Changed);

        }
        private void newfile(object fsfile, FileSystemEventArgs e)
        {

        }
        
        public void fs_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "orderedFoodsTxtFile.txt")
            {
                if (File.Exists(@"orderedFoodsTxtFile.txt"))
                {
                    string fileContainer = File.ReadAllText(@"orderedFoodsTxtFile.txt");


                    foreach (string i in fileContainer.Split(' '))
                    {
                        string Result = i;
                        string eResult = i.ToLower();
                        string firstLetter = char.ToUpper(eResult[0]) + eResult.Substring(1);
                        counter++;
                        //orderedFoodDiction.Add(firstLetter, counter);
                        //numOrder = orderedFoodDiction[firstLetter].ToString();



                        List<User> items = new List<User>();
                        //orderedFoodDiction.Add($"{firstLetter}", foodPrices[eResult]);

                        foodCounter.Add(firstLetter);

                        //items.Add(new User() { orderedFood = $"{firstLetter}", cost = $"${foodPrices[eResult]}", orderNumber = orders.Items.Count + 1 });
                        items.Add(new User() { orderedFood = $"{firstLetter}", cost = $"$4", orderNumber = orders.Items.Count + 1 });
                        orders.Items.Add(items);
                        aiReply($"Successfully added, {firstLetter} to order");
                    }
                }
            }
        }
        */


        public void prevOrdersWinClosing(object sender, CancelEventArgs e)
        {

        }

        private async void wait()
        {
            await Task.Delay(4000);
            questionBlockQ.Text = currentQuestion;


        }
        public void previousOrdersOpen()
        {

        }


    }


    public class User
    {
        public string orderedFood { get; set; }
        public string cost { get; set; }
        public int orderNumber { get; set; }

    }
    public class Menu
    {
        public string food { get; set; }
        public string price { get; set; }

    }
    public class History
    {
        public string userName { get; set; }
        public List<string> food { get; set; }
    }
    public class orderClass
    {
        public int orderNum { get; set; }
        public List<string> food { get; set; }

    }


}



/*public void askSpeechRecognition(object sender, RoutedEventArgs e)
{

    using (SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
    {

        recognizer.LoadGrammar(new DictationGrammar());

        recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
        recognizer.SetInputToDefaultAudioDevice();
        recognizer.RecognizeAsync(RecognizeMode.Single);
        STT.Text = "hi";




    }

       /* if (e.Result.Text.ToLower() == "what's the time") {

            reply.Text = "Current Time: " + now.ToShortTimeString();
            speech.Speak("Current Time: ");
            speech.Speak(now.ToShortTimeString());
            STT.Text = now.ToShortTimeString();                 
        }  */








