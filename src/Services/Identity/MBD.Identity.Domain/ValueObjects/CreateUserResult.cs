namespace MBD.Identity.Domain.ValueObjects
{
    public class CreateUserResult
    {
        public string Message { get; private set; }
        public bool IsSuccessful { get; private set; }

        private CreateUserResult(string message, bool isSuccessful)
        {
            Message = message;
            IsSuccessful = isSuccessful;
        }

        public static class CreateUserResultFactory
        {
            public static CreateUserResult Success(string message)
            {
                return new CreateUserResult(message, true);
            }

            public static CreateUserResult Fail(string error)
            {
                return new CreateUserResult(error, false);
            }
        }
    }
}