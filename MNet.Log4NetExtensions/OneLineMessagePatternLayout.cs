using log4net.Layout;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNet.Log4NetExtensions
{
  public class OneLineMessagePatternLayout : PatternLayout
  {
    public OneLineMessagePatternLayout()
    {
      AddConverter(new ConverterInfo { Name = "one_line_message", Type = typeof(OneLineMessagePatternConvertor) });
    }
  }
}