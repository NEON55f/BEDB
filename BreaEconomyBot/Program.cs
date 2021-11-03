using System;
using System.Net;
using DSharpPlus;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.Entities;

namespace BreaEconomyBot
{
    class Program
    {


        static List<string> SearchForItem(string itemToSearch)
        {
            List<string> response = new List<string>();
            string stringPart = "";

          string  url = $"https://breaeconomy.xyz/items.php?arama={itemToSearch.Replace(" ", "+")}";

            WebClient client = new WebClient();
            string str = client.DownloadString(url);





            Console.WriteLine($"Searching for \"{itemToSearch}\"...");

            int order = 0;
            //Get item name, item price, item info and last, get picture of item.
            while (order != -1)
            {
                if (str.IndexOf("itemtitle", order) == -1)
                {
                    return response;
                }

                int value = str.IndexOf("itemtitle", order);
                order = value;
                string itemname = "";
                while (str[order] != '<')
                {
                    itemname = itemname + str[order];
                    order++;
                }
                stringPart = stringPart + itemname.Split(">")[1] + "\n";

                value = str.IndexOf("\"fiyat\"", order);
                order = value;
                string itemprice = "";
                while (str[order] != '<')
                {
                    itemprice = itemprice + str[order];
                    order++;
                }
                if(itemprice.Split(">")[1].ToLower().Contains("per"))
                {
                    stringPart = stringPart + itemprice.Split(">")[1] + " World Lock(s)\n";

                }else
                {
                    stringPart = stringPart + itemprice.Split(">")[1] + " World Lock(s) per item\n";

                }



                value = str.IndexOf("kart-aciklama", order);
                order = value;
                string iteminfo = "";
                while (str[order] != '<')
                {
                    iteminfo = iteminfo + str[order];
                    order++;
                }
                stringPart = stringPart + iteminfo.Split(">")[1] + "\n";
                response.Add(stringPart);
                stringPart = "";
            }
            return response;
        }
        static void Main(string[] args)
        {
            mainAsync().GetAwaiter().GetResult();
           
        }

        static async Task mainAsync()
        {
            DiscordClient client = new DiscordClient(new DiscordConfiguration() { Token = "OTA1NDcyMjIzNDgwMDA0NzAw.YYKkuA.hCOUkEOO2wTrIVLCILyC_JxIwUk",TokenType = TokenType.Bot }) ;
            client.ConnectAsync();

            client.MessageCreated += Client_MessageCreated;

            await Task.Delay(-1);

        }

        private static Task Client_MessageCreated(DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Message.Content.ToLower().StartsWith("!search"))
            {
                //Detected message
                string searchString = e.Message.Content.Substring(8, e.Message.Content.Length - 8);

              List<string> response =  SearchForItem(searchString);


                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.Color = DiscordColor.Green;
                embed.Title = $"Searchs for \"{searchString}\"";

                int i = 0; int b = 0;
                foreach (string s in response)
                {
                    if (i == 25)
                    {
                        e.Message.RespondAsync(embed.Build());
                        i = 0;
                        embed.ClearFields();
                    }
                    else
                    {



                        embed.AddField(s.Split("\n")[0], s.Split("\n")[1] + "\n" + s.Split("\n")[2]);
                        i++;
                    }
                }

                if(b != 0)
                {
                    embed.Description = $"+{b} more results.";

                }
                e.Message.RespondAsync(embed.Build());
               
            }
            return null;
        }
    }
}
