public class LoadSecrets {
    public Func<string> getToken = () => { return DotNetEnv.Env.GetString("TOKEN"); };
    public Func<ulong> getApplicationId = () => {return Convert.ToUInt64(DotNetEnv.Env.GetString("APPLICATION_ID"));};
    public Func<ulong> getGuildId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("GUILD_ID")); };
    public Func<ulong> getTestChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID")); };
    public Func<ulong> getModChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("MOD_CHANNEL_ID")); };
    public Func<ulong> getWelcomeChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("WELCOME_CHANNEL_ID")); };
    public Func<ulong> getAnnouncementsChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("ANNOUNCEMENTS_CHANNEL_ID")); };
    public Func<ulong> getGeneralChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("GENERAL_CHANNEL_ID")); };
    public Func<ulong> getIdeasChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("IDEAS_CHANNEL_ID")); };
    public Func<ulong> getDoofAiChannelId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("DOOF_AI_CHANNEL_ID")); };
    public Func<ulong> getTestRoleId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_ROLE_ID")); };
    public Func<ulong> getModRoleId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("INATOR_ROLE_ID")); };
    public Func<ulong> getDoofRoleId = () => { return Convert.ToUInt64(DotNetEnv.Env.GetString("DOOF_ROLE_ID")); };
    public Func<string> getHuggingFaceApi = () => { return DotNetEnv.Env.GetString("HUGGING_FACE_API_TOKEN"); };
}