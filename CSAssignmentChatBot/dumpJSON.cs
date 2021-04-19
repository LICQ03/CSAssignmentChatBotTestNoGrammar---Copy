using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;
namespace CSAssignmentChatBot
{
    class dumpJSON
    {

        public static void DumpAsJson(Dictionary<string, order> data)
        {
            string json = JsonConvert.SerializeObject(data);

            using (StreamWriter file = System.IO.File.CreateText(@"JSON\ordersHis.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
            // SpeechSynthesizer speech = new SpeechSynthesizer();
        }
        public void get()
        {
            var client = new WebClient();
            var text = client.DownloadString(@"JSON\ordersHis.json");
            Dictionary<string, order> json = JsonConvert.DeserializeObject<Dictionary<string, order>>(text);
        }
    }
    class order
    {
        public Dictionary<int, string[]> orders { get; set; }
    }
    public class read
    {
        Dictionary<string, order> order { get; set; }

    }
}
