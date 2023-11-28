using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement.Models
{
    public class ExpenseView
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; } = string.Empty;


        private decimal price;
        public decimal Price 
        { 
            get { return Math.Round(price, 2);}
            set { price = value; } 
        }

        public string CategoryName { get; set; } = string.Empty;

        public ExpenseView()
        {

        }

        public ExpenseView(DateTime date, decimal price, string userName, string categoryName)
        {
            Date = date;
            Price = price;
            UserName = userName;
            CategoryName = categoryName;
        }

        //Overrides the default toString implementation to output the desired comma separated value string when called.
        public override string ToString()
        {
            return $"{Id},{Date},{UserName},{Price},{CategoryName}";
        }
    }
}
