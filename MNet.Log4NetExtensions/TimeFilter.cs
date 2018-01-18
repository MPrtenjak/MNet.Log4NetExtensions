using System;
using System.Linq;
using System.Collections.Generic;

using log4net.Core;
using log4net.Filter;

namespace MNet.Log4NetExtensions
{
  public class TimeFilter : FilterSkeleton
  {
    public TimeSpan TimeSpan { get; set; }
    public int BufferSize { get; set; }
    public int CurrentQueueSize { get
      {
        return this.queuedEvents.Count;
      }
    }

    public TimeFilter()
    {
      this.TimeSpan = TimeSpan.FromMinutes(1);
      this.BufferSize = 5;
    }

    public override FilterDecision Decide(LoggingEvent loggingEvent)
    {
      var logTime = loggingEvent.TimeStampUtc;
      var filterTimeStamp = logTime - this.TimeSpan;

      while ((this.queuedEvents.Count > 0) && (this.queuedEvents.Peek() < filterTimeStamp))
        this.queuedEvents.Dequeue();

      this.queuedEvents.Enqueue(logTime);

      if (this.queuedEvents.Count >= this.BufferSize)
      {
        this.queuedEvents.Clear();
        return FilterDecision.Neutral;
      }

      return FilterDecision.Deny;
    }

    private Queue<DateTime> queuedEvents = new Queue<DateTime>();
  }
}
