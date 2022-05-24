using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;

Console.Write("Nickname: ");
string nickname = Console.ReadLine();
Console.WriteLine();

string accInfoUrl = "https://eun1.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + nickname + "?api_key=<api_key>";
//eun1 is for EUNE server. https://leagueoflegends.fandom.com/wiki/Servers



var client = new WebClient();

string body = "";
body = client.DownloadString(accInfoUrl);

JToken idToken = JToken.Parse(body);


string championsUrl = "http://ddragon.leagueoflegends.com/cdn/12.9.1/data/en_US/champion.json"; // 12.9 is current patch
string masteryUrl = "https://eun1.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-summoner/" +
    idToken["id"].ToString() +
    "?api_key=<api_key>";

 
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
    Console.Write(ID_Name[item["championId"].ToString()] + " - " + "Level "  +item["championLevel"] + " - "+ item["championPoints"]);
    if (bool.Parse(item["chestGranted"].ToString()))
    {
        Console.WriteLine(" - Chest NOT available");
    }
    else Console.WriteLine(" - Chest available");
}


