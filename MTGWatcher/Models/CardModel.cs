using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTGWatcher.Models
{
    public class Card 
    {
        [Key]
        public int CardId { get; set; }
        public string Name { get; set;}
        public string Cmc { get; set; }
        public string ImageUrl { get; set; }
        public Card(string name)
        {
            Name = name;
        }
        public Card()
        {
          
        }
        public int SetId { get; set; }
    }

    public class CardDetail
    {
        public string Artist { get; set; }
        public string ConvertedManaCost { get; set; }
        public List<string> ColorIdentity { get; set; }
        public List<string> Colors { get; set; }
        public string Flavor { get; set; }
        public string ImageUrl { get; set; }
        public string Layout { get; set; }
        public string ManaCost { get; set; }
        public int multiverseid { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Rarity { get; set; }
        public List<string> Types { get; set; }
        public List<string> Subtypes { get; set; }
        public string Text { get; set; }

    }

    public class CardListViewModel : PaginationModel {

        public List<Card> Results { get; set; }
        public string Filter { get; set; }
    }

    public class CardDetailViewModel : Card
    {


    }

    public class JsonCard
    {
        public string layout { get; set; }
        public string name { get; set; }
        public string manaCost { get; set; }
        public string cmc { get; set; }
        public List<string> colors { get; set; }
        public string type { get; set; }
        public List<string> types { get; set; }
        public List<string> subtypes { get; set; }
        public string text { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        public string imageName { get; set; }
        public List<string> colorIdentity { get; set; }

      
    }

    public class JsonCardRoot
    {
        public Dictionary<string, JsonCard> Cards { set; get; }
    }
}