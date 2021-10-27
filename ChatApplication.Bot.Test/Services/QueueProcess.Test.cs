using Xunit;

namespace ChatApplication.Bot.Test.Services
{
    public class QueueProcessTest
    {
        private const string MSG_OK = "{0}|{1} quote is ${2} per share.";
        private const string MSG_EMPTY = "No results for the code.";
        private const string MSG_MALFORMED = "Wrong query format.";

        QueueProcess queueProcess = new QueueProcess(new QueueConfiguration {
            Server = "localhost",
            IncomingQueue = "TestIncoming",
            OutgoingQueue = "TestOutgoing",
            AllowedCommands = new string[] {"/stock"}
        });

        [Fact]
        public async void ShouldReturnMalformedMessageWhenReceiveEmptyMessage()
        {
            //arrange
            var message = "";
            //act
            var result = await queueProcess.ProcessMessage(message);
            //assert
            Assert.Equal(result, MSG_MALFORMED);
        }

        [Fact]
        public async void ShouldReturnMalformedMessageWhenReceiveMalformedMessage()
        {
            //arrange
            var message = "room|code=aapl.us";
            //act
            var result = await queueProcess.ProcessMessage(message);
            //assert
            Assert.Equal(result, MSG_MALFORMED);
        }

        [Fact]
        public async void ShouldReturnEmptyMessageWhenReceiveNoValidCode()
        {
            //arrange
            var message = "room|/stock=test";
            //act
            var result = await queueProcess.ProcessMessage(message);
            //assert
            Assert.Equal(result, MSG_EMPTY);
        }

        [Fact]
        public async void ShouldReturnOkMessageWhenReceiveValidCodeAndFormat()
        {
            //arrange
            var message = "room|/stock=aapl.us";
            //act
            var result = await queueProcess.ProcessMessage(message);
            //assert
            Assert.Equal(result, MSG_OK);
        }
    }
}
