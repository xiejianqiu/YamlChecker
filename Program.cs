namespace YAMLCheckerWin
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = args[0];
            var logSavePath = args[1];
            bool printlog = false;
            if (args.Length >= 3)
            {
                printlog = "1" == args[2];
            }
            AppFacade.StartUp(filePath, logSavePath, printlog);
        }
    }
}
