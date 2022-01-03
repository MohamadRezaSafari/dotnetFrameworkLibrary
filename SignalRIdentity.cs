using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Providers
{
    public class User
    {
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    public class SignalRIdentity : Hub
    {
        private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        private User GetUser(string username)
        {
            User user;
            Users.TryGetValue(username, out user);

            return user;
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName, _ => new User
            {
                Name = userName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {

                user.ConnectionIds.Add(connectionId);

                // TODO: Broadcast the connected user

                //Clients.All.broadcastMessage(connectionId);
                Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);
                //if (user.ConnectionIds.Count == 1)
                //{
                //    Clients.Others.userConnected(userName);
                //}
            }

            return base.OnConnected();
        }

        public async Task Send(string message, string to)
        {
            await Task.Run(() =>
            {
                User receiver;
                if (Users.TryGetValue(to, out receiver))
                {

                    User sender = GetUser(Context.User.Identity.Name);

                    IEnumerable<string> allReceivers;
                    lock (receiver.ConnectionIds)
                    {
                        lock (sender.ConnectionIds)
                        {

                            allReceivers = receiver.ConnectionIds.Concat(
                                sender.ConnectionIds);
                        }
                    }

                    foreach (var cid in allReceivers)
                    {
                        Clients.Client(cid).received(new
                        {
                            sender = sender.Name,
                            message = message,
                            isPrivate = true
                        });
                    }
                }
            });
        }

        public void Send(string message)
        {

            string sender = Context.User.Identity.Name;

            Clients.All.received(new
            {
                sender = sender,
                message = message,
                isPrivate = false
            });
        }

    }
}