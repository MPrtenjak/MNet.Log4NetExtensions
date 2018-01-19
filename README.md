# MNet.Log4NetExtensions

This library is based on https://github.com/KentorIT/Log4NetExtensions library, but serves another purpose. With this filter you can set some specific time frame and filter will forward log only if enough messages are received in that specific time frame.

This means that if you set time frame to 1 minute and buffer to 5 messages. Filter will pass 6th message, if it is received less than 1 minute after first message. After processing the message, buffer is cleared!

## How is this useful?

When web apps are running at night, there can be some errors. If one user is having a problem in the middle of the night, I will not get up. But if there are 5 errors in 1 hour, then there is some kind of a problem and I will have to investigate it.

And that is the whole purpose of this filter. I setup file log which is logging all errors and SMS log which will fire only it 5 messages are received within 1 hour range.

```xml
<appender name="FileAppenderAll" type="log4net.Appender.FileAppender">
  <file value="app_all.log" />
  <appendToFile value="false" />
  <layout type="log4net.Layout.PatternLayout">
    <conversionPattern value="%date %level %logger - %message%newline" />
  </layout>
</appender>

<appender name="SMSAppender" type="log4net.Appender.SmtpAppender">
  <to value="xxx" />
  <from value="xxx" />
  <subject value="xxx" />
  <smtpHost value="xxx" />
  <layout type="log4net.Layout.PatternLayout">
    <conversionPattern value="%date %level %logger - %message%newline" />
  </layout>
  <filter type="MNet.Log4NetExtensions.TimeFilter, MNet.Log4NetExtensions">
    <TimeSpan value="01:00:00"/>
    <BufferSize value="5"/>
  </filter>
</appender>
```

## Example

Filter is set as:

__TimeSpan__ = _10 minutes_

__BufferSize__ = _3 messages_

| Time (min.) | Message | Current buffer | Action | Description |
|---------------:|:-------:|----------------|--------|-------------|
|1 | M1 | M1 | None | First message in buffer |
|5 | M2 | M1, M2 | None | Second message in buffer |
|13| M3 | M2, M3 | None | First message in out of the buffer because it is older than 10 minutes |
|14| M4 | <empty> | __Fire__ | __There were 3 mesages (M2, M3, M4) in time span of 10 minutes so filter is activated, message M4 is written to log and buffer is cleared__ |
|20 | M5 | M5 | None | First message in buffer |
|22 | M6 | M5, M6 | None | Second message in buffer |
|35 | M7 | M7 | None | Messages M5 and M6 are too old so they are thrown out of the buffer |

... and so on ...

## nuget

You can download binaries from nuget:  https://www.nuget.org/packages/MNet.Log4NetExtensions/
