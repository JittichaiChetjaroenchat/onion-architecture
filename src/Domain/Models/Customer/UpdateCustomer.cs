using System;

namespace Domain.Models.Customer
{
    public class UpdateCustomer
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}