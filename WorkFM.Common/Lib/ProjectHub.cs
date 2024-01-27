using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data;
using WorkFM.Common.Data.Projects;

namespace WorkFM.Common.Lib
{
    public class ProjectHub : Hub
    {
        private readonly IDictionary<string, UserProjectConnection> _connection;
        public ProjectHub(IDictionary<string, UserProjectConnection> connection)
        {
            _connection = connection;
        }
        public async Task JoinProject(UserProjectConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ProjectId);
            _connection[Context.ConnectionId] = userConnection;
        }

        public async Task SendProject(ProjectSender projectSender)
        {
            if (_connection.TryGetValue(Context.ConnectionId, out UserProjectConnection userConnection))
            {
                await Clients.Group(userConnection.ProjectId)
                    .SendAsync("ReceiveMessage", projectSender, DateTime.Now);
            }
        }
        public override Task OnDisconnectedAsync(Exception? exp)
        {
            if (!_connection.TryGetValue(Context.ConnectionId, out UserProjectConnection roomConnection))
            {
                return base.OnDisconnectedAsync(exp);
            }

            _connection.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exp);
        }

        public Task SendConnectedUser(string projectId)
        {
            var users = _connection.Values
                .Where(u => u.ProjectId == projectId)
                .Select(s => s.UserId);
            return Clients.Group(projectId).SendAsync("ConnectedUser", users);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceviceMessage", $"{Context.ConnectionId} has connected");
        }
    }
}
