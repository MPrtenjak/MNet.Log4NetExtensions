using log4net;
using log4net.Config;

namespace MNet.Log4NetExtensions.Console
{
  class Program
  {
    static void Main(string[] args)
    {
      XmlConfigurator.Configure();

      for (int i = 0; i < 7; i++)
      {
        log.Debug("this is debug message");
        log.Info("this is info message");
        log.Warn("this is warn message");
        log.Error("this is error message");
        log.Fatal("this is fatal message");
      }
    }

    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
  }
}
