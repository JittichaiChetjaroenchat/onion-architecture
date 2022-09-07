using System;

namespace Domain.Models.Customer
{
    public class GetCustomer
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}