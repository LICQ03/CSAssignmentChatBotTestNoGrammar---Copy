using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CSAssignmentChatBot
{
    class getFoodPrices
    {

        public static int getTotalPrice(List<string> orderedFood)
        {
            int totalOrder = 0;
            string lineUpNow = "";
            string[] foods = File.ReadAllLines(@"food.txt");
            string[] foodsDessert = File.ReadAllLines(@"foodDessert.txt");
            string[] foodsEntree = File.ReadAllLines(@"foodEntree.txt");
            List<string> allFoods = new List<string>();
            
            

            IDictionary<string, int> foodPrices = new Dictionary<string, int>();
            IDictionary<string, int> foodPricesDessert = new Dictionary<string, int>();
            IDictionary<string, int> foodPricesEntree = new Dictionary<string, int>();
            IDictionary<string, int> AllfoodPrices = new Dictionary<string, int>();
            List<string> foodList = new List<string>();
            int countThroughArray = 0;
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
            foreach(string i in orderedFood)
            {
                foreach(KeyValuePair<string, int> foodCheck in AllfoodPrices)
                {
                    if (foodCheck.Key == i.ToLower()) totalOrder = totalOrder + foodCheck.Value;
                }
            }
            return totalOrder;

            
        }
    }
}

