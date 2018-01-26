using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace mmcguffhw.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            var response = "You sent: " + activity.Text + " which was " + length + " characters";

            response = response.Truncate(500);

            await context.PostAsync($"{response}");

            LogDatabase.WriteToDatabase
                (
                      conversationid:   activity.Conversation.Id
                    , username: "ElderBot"
                    , channel:  activity.ChannelId
                    , message:  response
                );            

            context.Wait(MessageReceivedAsync);
        }
    }
}