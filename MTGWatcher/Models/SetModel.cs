using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MTGWatcher.Models
{
    public class SetModel
    {
        [Key]
        public int SetId {get;set;}
        public string Name { get; set; }
        public string Code { get; set; }
        public string ReleaseDate { get; set; }
        
    }

    public class JsonSet
    {
        public string name { get; set; }
        public string code { get; set; }
        public string gathererCode { get; set; }
        public string magicCardsInfoCode { get; set; }
        public string releaseDate { get; set; }
        public string border { get; set; }
        public string type { get; set; }
        public List<object> booster { get; set; }
        public string mkm_name { get; set; }
        public string mkm_id { get; set; }
        public List<JsonCard2> cards { get; set; }

    }

    public class JsonCard2
    {
        public string artist { get; set; }
        public string cmc { get; set; }
        public List<string> colorIdentity { get; set; }
        public List<string> colors { get; set; }
        public string flavor { get; set; }
        public string id { get; set; }
        public string imageName { get; set; }
        public string layout { get; set; }
        public string manaCost { get; set; }
        public string mciNumber { get; set; }
        public int multiverseid { get; set; }
        public string name { get; set; }
        public string power { get; set; }
        public string rarity { get; set; }
        public List<string> subtypes { get; set; }
        public string text { get; set; }
        public string toughness { get; set; }
        public List<string> types { get; set; }


    }

    public class JsonSetRoot
    {
        public Dictionary<string, JsonSet> Sets { set; get; }
    }
}