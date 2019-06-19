using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Microsoft.AspNet.SignalR;

namespace BookingEnginePMS.Hubs
{
    public class ChatHub : Hub
    {
        public void requireUserOnline()
        {
            Clients.All.requireUserOnline();
        }
        public void getAllUserOnline(string username)
        {
            Clients.All.getAllUserOnline(username);
        }
        public void sendMessage(string firstUser, string secondUser)
        {
            Clients.All.broadcastMessage(secondUser, firstUser);
        }
        public void getNotification()
        {
            Clients.Others.getNotification();
        }
    }
}