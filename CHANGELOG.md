# Changelog

## 03/05/2024

### Creation of [JingleModule.cs](/src/modules/JingleModule.cs) and [SendAudio.cs](/src/audio/SendAudio.cs)

Created two new files, [JingleModule.cs](/src/modules/JingleModule.cs) and [SendAudio.cs](/src/audio/SendAudio.cs)

#### [JingleModule.cs](/src/modules/JingleModule.cs)

This file contains the command that will be used to join and play the Doofenshmirtz Evil Incorporated jingle, this file specifically works in order to get the channel where the user is currently on, following this the command will call the SendAudio file that is in charge of creating a stream and playing the audio

#### [SendAudio.cs](/src/audio/SendAudio.cs)

This file will obtain the path of the .mp3 file from the JingleModule to create an stream using ffmpeg that will be used to play the audio directly on the voice channel

### Changes on [GuildEvents.cs](/src/events/GuildEvents.cs)

Previously the code on [GuildEvents.cs](/src/events/GuildEvents.cs), both GuildScheduledEventCreated and GuildScheduledEventStarted had a problem where the message was only sent when the event had a custom location set, not a VC

This problem was solved doing the following:

- Created three private methods that will check if the event was created to happen in a custom location or a voice channel and if said event has a description
- Depending on what the moderator set during the creation of the event the bot needs to output different messages. In the event creation the bot will now send an embed message, said embed is modified depending on whether the event is in a location or a voice channel and if it has a description or not
- Something similar happens when the event is started, in this case the bot will check if the event happens in a custom location or a voice channel, depending on that, the bot will send different messages
