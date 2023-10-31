# HackerStories

The web service downloads story details from Hacker News.

The application can be run straight from Visual Studio via the "Debug | Start Without Debugging". This will lauch a browser that will display a Swagger UI that will allow you to make calls to the web service.

Alternatively, once you are running the web service you can explicitly specify a url. For example, to view the top 10 stories you would navigate to:

    http://localhost:5158/HackerStories?count=10

To avoid putting too much load on the Hacker News website the server caches the best stories information it downloads, and the story information. The data is stored in an expiring cache that specified how long to hold onto the data before we should re-download it from the server.


The application is configured via the **appsettings.json** file. There are two components you can configure, the **AllStoriesCache** loader, which is responsible for downloading all the stories available, and the **StoryCache** which caches the details of individual stories. The following settings are available:

### AllStoriesCache
| Setting  | Description |
|----------|-------------|
| Endpoint | The url where the best stories json can be loaded from |
| Expiry   | How long to cache the data before re-downloading it from the endpoint |

### StoryCache
| Setting      | Description |
|--------------|-------------|
| EndpointMask | A C# format string that takes a single value (in {0}) that is used to generate the endpoint for a story |
| Expiry       | How long to cache the story before re-downloading it from the endpoint |


## Assumptions Made
It is assumed that it is reasonable to cache the Hacker News data locally for a period of time. If you do not want to cache the data locally then setting **Expiry** to "00:00:00" will disable caching the data.

Calling the web service without a **count** will return an empty array. Instead of doing this we could default to a given number of stories so that something is always returned.