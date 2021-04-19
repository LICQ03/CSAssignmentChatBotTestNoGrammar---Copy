using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace CSAssignmentChatBot
{
    class checkPreviousOrder
    {

        public static void check(string name)
        {
            var client = new WebClient();
            var text = client.DownloadString(@"JSON\ordersHis.json");
            Dictionary<string, order> json = JsonConvert.DeserializeObject<Dictionary<string, order>>(text);
            foreach(KeyValuePair<string, order> i in json)
            {
                if(i.Key.ToLower() == name.ToLower())
                {
                }
            }
        }

    }
    class checkOrder
    {
        public Dictionary<int, string[]> orders { get; set; }
    }
    public class checkRead
    {
        Dictionary<string, order> order { get; set; }

    }
}
