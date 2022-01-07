# ProductivityTools.GetTask3.Sdk


```c#
        public GetTaskHttpClient(string url, IConfigurationRoot configuration, Action<string> log)
        {
            this.URL = url;
            this.Log = log;
            this.Configuration = configuration;
            
        }
```

Client require configuration object to be passed. In the implementation it uses **GetTask3Cmdlet** value from the config to access the api

I should change it to pass password.

Configuration["GetTask3Cmdlet"]
