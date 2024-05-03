# Changelog

## 03/05/2024

Previously the code on [GuildEvents.cs](/src/events/GuildEvents.cs), both GuildScheduledEventCreated and GuildScheduledEventStarted had a problem where the message was only sent when the event had a custom location set, not a VC

This problem was solved doing the following:

- Created three private methods that will check if the event was created to happen in a custom location or a voice channel and if said event has a description
- Depending on what the moderator set during the creation of the event the bot needs to output different messages. In the event creation the bot will now send an embed message, said embed is modified depending on whether the event is in a location or a voice channel and if it has a description or not
- Something similar happens when the event is started, in this case the bot will check if the event happens in a custom location or a voice channel, depending on that, the bot will send different messages
