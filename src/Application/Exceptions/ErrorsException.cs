namespace Application.Exceptions
{
    public class ErrorsException : System.Exception
    {
        public string[] Errors { get; private set; }

        public ErrorsException(string [] errors)
        {
            Errors = errors;
        }
    }
}