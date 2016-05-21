using Dapper;
using Reflux.Model;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Reflux.DataAccess
{
    public class FlowMessageStore
    {
        private const string FileName = "FlowMessageStore.sqlite";
        private const string ConnectionString = "Data Source=" + FileName + ";Version=3;";

        public FlowMessageStore()
        {
            if (!File.Exists(FileName))
            {
                CreateEmptyDatabase();
            }
        }

        private void CreateEmptyDatabase()
        {
            SQLiteConnection.CreateFile(FileName);

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute(
                    @"
                    CREATE TABLE FlowMessages (
                    Id TEXT NOT NULL,
                    LastRemindedMessageId TEXT NOT NULL
                    )");
            }
        }

        public FlowMessage Find(string id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var flowResult = connection.Query<FlowMessage>(
                    @"SELECT Id, LastRemindedMessageId
                    FROM FlowMessages
                    WHERE Id = @Id", new
                    {
                        Id = id
                    }).FirstOrDefault();

                return flowResult;
            }
        }

        public void Add(string flowId, string messageId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute("DELETE FROM FlowMessages WHERE Id = @Id", new
                {
                    Id = flowId
                });

                connection.Execute(@"INSERT INTO FlowMessages (Id, LastRemindedMessageId)
                    VALUES (@Id, @LastRemindedMessageId)", new
                {
                    Id = flowId,
                    LastRemindedMessageId = messageId
                });
            }
        }
    }
}
