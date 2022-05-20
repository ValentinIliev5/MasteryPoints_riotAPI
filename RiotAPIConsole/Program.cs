using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;

string championsUrl = "http://ddragon.leagueoflegends.com/cdn/12.9.1/data/en_US/champion.json"; // 12.9 is current patch
string masteryUrl = "https://eun1.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-summoner/" +
    "ID" +
    "?api_key=<API_KEY>";

// https://<region>.api.riotgames.com/lol/summoner/v4/summoners/by-name/<name>?api_key=<key> to get <your id>


var client = new WebClient();

string body = "";
body = client.DownloadString(championsUrl);

JObject championsData = JObject.Parse(body);

JObject championsInfo = JObject.Parse(championsData["data"].ToString());

Dictionary<string, string> ID_Name = new Dictionary<string, string>(); 

foreach (var item in championsInfo)
{

    ID_Name.Add(item.Value["key"].ToString(), item.Key);
}


body = client.DownloadString(masteryUrl);
JToken masteryInfo = JToken.Parse(body);

long points = 0;

foreach (var item in masteryInfo)
{
    points += int.Parse(item["championPoints"].ToString());
}

Console.WriteLine("Total of " + points + " points. \n");

foreach (var item in masteryInfo)
{
    Console.Write(ID_Name[item["championId"].ToString()] + " - " + item["championPoints"]);
    if (bool.Parse(item["chestGranted"].ToString()))
    {
        Console.WriteLine(" - Chest NOT available");
    }
    else Console.WriteLine(" - Chest available");
}


