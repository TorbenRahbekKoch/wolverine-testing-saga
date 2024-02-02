# The Wolverine testing saga

No, there is no reason to marvel at this. It is not *that* Wolverine. And the saga is nothing but a short story.

## No wolves, just tests

I was fooling around with [Wolverine](https://wolverine.netlify.app/)
because, well, I felt like it ;)

In my application I am using it as a simple in-process [Mediator](https://en.wikipedia.org/wiki/Mediator_pattern). This enables me to decouple
the business logic from e.g. the API-controllers or - in my case - the SignalR hub.

The application in question uses a SignalR hub, which puts a message on the queue. The queue
then - in the other end - hands that off to some business logic, which then produces some events
to the queue which sends those back to the SignalR hub.

This flow would be worth testing, I thought. And set out on a quest to do 
exactly that. How hard
can it be, you might then say. Well, It's software. [Software is hard](https://en.wikiquote.org/wiki/Donald_Knuth) as Donald (Knuth, not Duck) have told us.

I'm new to Wolverine so I opted to peruse the Wolverine documentation, which - honestly - is a somewhat dissapointing 
experience (there's a very helpful Discord channel, though!). But
as it turns out, I really didn't need the Wolverine documentation (much). Wolverine relies on ASP.NET so testing Wolverine boils down
to how to actually test that.

So how hard can that be? Well, it's software, so you know...

In the modern ASP.NET world with Minimal APIs and what have you
it is surprisingly difficult to bootstrap ASP.NET for a test.
At least it is, when you have no idea of where to start.

It took me some research (aka haphazard web searching) but eventually I came across
[WebApplicationFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1?view=aspnetcore-8.0) - which sounds lovely, but is still not that
easy to figure out.

I even looked at the code for it, which just led me onto 
wrong, and possibly hazardous, paths.

It seems that if you want to configure your test ASP.NET server 
exactly the same way as when running it normally, you are mostly
set to go. 

When you create an instance of `WebApplicationFactory` you should
provide it with a type of _a_ class in the assembly that configures
ASP.NET.

You typically configure it in Program.cs, but if you are using
Minimal APIs there is no such class available, per say. 

You can [as described here](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0) make the 
implicitly generated `Program`-class visible by adding a
partial declaration to Program.cs:

```` C#
public partial class Program {}
````

This assumes that you do want to configure your test-web server
the exactly same. This is, of course, not always the case.

## Configuring your test for testing

I wanted to have - in my test assembly - a specific configuration
of ASP.NET. How hard could that be? (You know the answer by now, right?)
In my test assembly I created a class with a `Main` method, because
WebApplicationFactory seems to be hell-bent on calling `Main`
to configure the application. 

No problem, I thought. This class looks something like this:


```` C#
public class TestConfiguration
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(
        ...
    
        var app = builder.Build();
        app.Run();
    }
````

Of course there was a problem with this - you can only have one
`Main` method (I think it's the signature, not the name, but I am not sure) in an assembly. The test assembly is a class library, but
nevertheless the compiler in all its programmed wisdom chooses
to autogenerate that method. Of course...

That took me a while to figure out how to do. Someone at Microsoft
seems to have had the same problem so you can actually configure
the name of the class containing the `Main`-method.

In the project file you add `StartupObject` to a property group:

```` xml
    <PropertyGroup>
        <StartupObject>Infrastructure.Tests.TestConfiguration</StartupObject>
    </PropertyGroup>

````

And voila - you can now use that class for your configuration.

Software is easy-peasy! 

## Disclaimer

The code for this test project is [available on Github](https://github.com/TorbenRahbekKoch/wolverine-testing-saga), but
please do NOT assume that this is the correct way to use Wolverine! 
Firstly, I am by no means an expert in Wolverine, secondly the code is
there to show how to configure `WebApplicationFactory`!