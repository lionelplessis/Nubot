﻿namespace Nubot.Interfaces
{
    using System;
    using System.IO;
    using System.Reflection;

    public class AdapterBase : IAdapter
    {
        protected readonly IRobot Robot;

        public AdapterBase(string adapterName, IRobot robot)
        {
            Name = adapterName;

            Robot = robot;
        }

        static AdapterBase()
        {
            ExecutingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            BaseAdapterDirectory = Path.Combine(ExecutingDirectory, "adapters");
        }

        public string Name { protected set; get; }

        public static string ExecutingDirectory { get; private set; }

        public static string BaseAdapterDirectory { get; private set; }

        public virtual void Start()
        {
        }

        public virtual void Message(string message)
        {
        }

        public virtual bool SendNotification(string roomName, string authToken, string htmlMessage, bool notify = false)
        {
            return false;
        }

        public virtual string MakeConfigFileName()
        {
            var adapterName = this.Name;
            var file = string.Format("{0}.config", adapterName);
            var configFileName = Path.Combine(BaseAdapterDirectory, file);

            return configFileName;
        }
    }
}