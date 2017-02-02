using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTGWatcher.Models
{
    public class Product
    {
        public string idProduct;
        public string idMertaproduct;
        public string countReprints;
        public string enName;
        public Category category;
        public string website;
        public string image;
        public string gameName;
        public string categoryName;
        public string number;
        public string rarity;
        public string expansionName;
        public object links;
        public Expansion expansion;
        public PriceGuide priceGuide;
        public List<Reprint> reprint;

    }

    public class Category
    {

        public string idCategory;
        public string categoryName;
    }

    public class Expansion
    {
        public string idExpansion;
        public string enName;
        public string expansionIcon;
    }

    public class PriceGuide
    {
        public string SELL;
        public string LOW;
        public string LOWEX;
        public string LOWFOIL;
        public string AVG;
        public string TREND;

    }

    public class Reprint
    {
        public string idProduct;
        public string expansion;
        public string expIcon;
    }

    public class ProductList
    {
        public string productsfile;
        public string mime;
        public object links;

    }

    public class ProductFileItem
    {
        public string idProduct;
        public string Name;
        public string CategoryID;
        public string Category;
        public string ExpansionID;
        public string DateAdded;
    }
}