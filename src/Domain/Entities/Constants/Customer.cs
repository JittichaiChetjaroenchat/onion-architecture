namespace Domain.Entities.Constants
{
    public class Customer
    {
        public const string TableName = "customers";

        public static class Column
        {
            public const string Id = "id";

            public const string CreatedOn = "created_on";

            public const string UpdatedOn = "updated_on";

            public const string Name = "name";

            public const string Age = "age";
        }
    }
}