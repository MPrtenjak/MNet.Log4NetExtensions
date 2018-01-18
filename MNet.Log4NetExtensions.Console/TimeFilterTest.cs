using log4net.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MNet.Log4NetExtensions.Console
{
  [TestClass]
  public class TimeFilterTest
  {
    [TestMethod]
    public void When_InTimeStamp_Then_EachNEventLogged()
    {
      foreach (var size in new int[] { 1, 2, 3, 5, 6, 7, 9 })
      {
        var filter = new TimeFilter
        {
          TimeSpan = TimeSpan.FromMinutes(60),
          BufferSize = size
        };

        for (int i = 1; i < 15; i++)
        {
          var result = filter.Decide(GetLoggingEventWithOffset(0));

          if ((i % size) == 0)
            Assert.AreEqual(FilterDecision.Neutral, result);
          else
            Assert.AreEqual(FilterDecision.Deny, result);

          Assert.AreEqual(filter.CurrentQueueSize, i % size);
        }
      }
    }

    [TestMethod]
    public void When_NotInTimeStamp_Then_EventsAreLost()
    {
      var filter = new TimeFilter
      {
        TimeSpan = TimeSpan.FromSeconds(60),
        BufferSize = 3
      };

      List<dynamic> testing = new List<dynamic>()
      {
        // queue is empty ==> add new element
        new { offset = 0, expectedResult = FilterDecision.Deny, expectedSize = 1 },
        // queue is not empty  ==> add new element
        new { offset = 10, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        // timeStamp exceded ==> first element from queue is lost
        new { offset = 61, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        // queue is full ==> process and empty queue
        new { offset = 62, expectedResult = FilterDecision.Neutral, expectedSize = 0 },
        // all five in one timestamp
        new { offset = 71, expectedResult = FilterDecision.Deny, expectedSize = 1 },
        new { offset = 72, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        new { offset = 73, expectedResult = FilterDecision.Neutral, expectedSize = 0 },
        new { offset = 74, expectedResult = FilterDecision.Deny, expectedSize = 1 },
        new { offset = 75, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        // this one must success
        new { offset = 74 + 59, expectedResult = FilterDecision.Neutral, expectedSize = 0 },

        new { offset = 74, expectedResult = FilterDecision.Deny, expectedSize = 1 },
        new { offset = 75, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        // this one must fail
        new { offset = 74 + 61, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        // this one must fail
        new { offset = 74 + 122, expectedResult = FilterDecision.Deny, expectedSize = 1 },
        new { offset = 74 + 123, expectedResult = FilterDecision.Deny, expectedSize = 2 },
        new { offset = 74 + 124, expectedResult = FilterDecision.Neutral, expectedSize = 0 },
      };

      for (int i = 0; i < testing.Count; i++)
      {
        dynamic element = testing[i];

        var result = filter.Decide(GetLoggingEventWithOffset(element.offset));
        Assert.AreEqual(element.expectedResult, result);
        Assert.AreEqual(element.expectedSize, filter.CurrentQueueSize);
      }
    }

    private static log4net.Core.LoggingEvent GetLoggingEventWithOffset(int offsetSeconds)
    {
      return new log4net.Core.LoggingEvent(
          new log4net.Core.LoggingEventData
          {
            TimeStampUtc = defaultTestDate.AddSeconds(offsetSeconds)
          });
    }

    private static readonly DateTime defaultTestDate = new DateTime(2000, 01, 01);
  }
}
