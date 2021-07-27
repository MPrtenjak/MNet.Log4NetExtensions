using log4net.Util;
using System.IO;

namespace MNet.Log4NetExtensions
{
  public class OneLineMessagePatternConvertor : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      var loggingEvent = state as log4net.Core.LoggingEvent;
      if (loggingEvent == null)
        return;

      writer.Write(loggingEvent.RenderedMessage.Replace("\r", " ").Replace("\n", " "));
    }
  }
}