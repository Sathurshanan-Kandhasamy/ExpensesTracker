using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        private decimal price;
        public decimal Price
        {
            get { return Math.Round(price, 2); }
            set { price = value; }
        }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public Expense()
        {

        }
        public Expense(DateTime date, decimal Price, int userId, int categoryId)
        {
            Date = date;
            Price = price;
            UserId = userId;
            CategoryId = categoryId;
        }
    }
}
