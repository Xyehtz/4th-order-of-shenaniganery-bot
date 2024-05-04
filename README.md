# 4th Order of Shenaniganery Discord Bot

***This is the official repository of the 4th Order of Shenaniganery bot.***

## Who are we

The 4th Order of Shenaniganery is a club from Sheridan College, Ontario, Canada.

The purpose of this bot is to improve the Discord server of the club, giving it a unique feeling that represents what we look forward to as a club.

This is a small yet very powerful bot that will give members of the club a unique experience on the online side of the journey of being a member of a club such as ours.

## Want to join our club?

**If you are a student of Sheridan College** and want to join this amazing club, click [here](https://sheridancollege.campuslabs.ca/engage/organization/4thorder) (remember that you must sign in with your Sheridan account first) or follow these steps:

1. Go to [Sheridan Student Union Clubs Corner](https://sheridancollege.campuslabs.ca/engage/).
2. Select the "Find Organizations" option.
3. Search for "4th Order of Shenaniganery", click and select the "Join" option.
4. Look at the club description, at the end you will find the Discord server link.
5. When you enter the club our bot will greet you.

## The bot

The bot is created on C# using the .NET framework and Discord.Net, the bot contains a series of simple yet fun commands that represent the personality of the Phineas and Ferb character, Dr. Heinz Doofenshmirtz, for this reason, you will see that a lot of the answers that the bot will give are based on this (very interesting) character.

The bot contains the following commands:

- !askDoof: Our most interesting command and the command that truly makes the bot unique. This command will answer questions (check our [Code of Conduct](CODE_OF_CONDUCT.md) regarding what is allowed) that the user has, using a locally installed instance of Llama3 (with [Ollama](https://github.com/ollama/ollama)) that will act as Dr. Heinz Doofenshmirtz to give funny and unique answers.
- !jingle: This small command will play the jingle of Doofenshmirtz Evil Incorporated in a voice channel to which a user is connected.
- !idea: This command will automatically format messages that contain ideas for club meetings and more, the users are free to share any type of idea that they have.
- !ping: This command just answers with Pong and is a way for everyone to check that the bot is working properly.

*At the moment two more commands are in the works, they will be added with the official launch of the bot*

The bot also contains a series of event handlers that will process events mostly related to users, messages and events of the Server, each one of them will interact with the users by answering certain messages or even sending announcements to all of the members, you can check all the events inside of the [events folder](./src/events/).

## Contributing

Check [CONTRIBUTING](CONTRIBUTING.md) to learn how you can contribute to this project. You don't need to be a Sheridan College student to be part of this

## Code of Conduct

The Sheridan College students and members of the club must comply with the rules set in the Code of Conduct in order to comply with the **Sheridan College Academic Integrity Policy**. You can check the Code of Conduct [here](CODE_OF_CONDUCT.md)

## License

Check [LICENSE](LICENSE.md)

## Latest Changes

To know the latest changes related to this project please check [CHANGELOG](CHANGELOG.md)
