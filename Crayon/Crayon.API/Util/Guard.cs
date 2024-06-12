namespace Crayon.API.Util
{
    public static class Guard
    {
        public static T AgainstNull<T>(T input, string inputName)
        {
            if(input is null)
            {
                throw new ArgumentNullException(inputName);
            }

            return input;
        }
    }
}
