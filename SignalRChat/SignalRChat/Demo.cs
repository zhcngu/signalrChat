namespace SignalRChat
{
    public class Demo
    {
        public static void DD()
        {
			try
			{
                int a = 2;
                int b = 0;
                int c = a / b;
			}
			catch (Exception ex)
			{

				throw;
			}
			finally {
                Console.WriteLine(  "finally");
            }
        }
    }
}
