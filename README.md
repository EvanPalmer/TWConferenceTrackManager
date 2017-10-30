# ConferenceTrackManager

## Run instructions For Mac

Install [.Net Core for MacOS](https://www.microsoft.com/net/core#macos)

``` 
git clone https://github.com/EvanPalmer/TWConferenceTrackManager.git
cd TWConferenceTrackManager/src/
dotnet restore
cd ThoughtWorks.ConferenceTrackManager.Tests
dotnet xunit
cd ../ThoughtWorks.ConferenceTrackManager
dotnet run talks.txt
```

## Architecture Decisions/Design Explanations
1. Domain models: Conference, ConferenceSession and Talk
2. Value models are immutable
3. Any complex logic from model is moved into a dependency
3. Use Factory methods for model instantiation where required
4. Factory methods handle downstream dependencies
5. Factory methods are not under test to avoid integration testing
6. AppConfiguration is not under test (but should be)
7. Console app starts with a single service locater call for IOC
8. Basic handling of command line arguments
9. Use ThoughtWorks.ConferenceTrackManager.Access namespace to build seams around system dependencies. Sometimes there's a bit of untested logic in there, which isn't ideal.

## Talk Distribution Algorithm Design
Allocating talks to sessions, efficiently, and with optimal use of space was a difficult problem.

The implemented algorithm tackles this by sorting the talks from longest to lightning, then allocating them over the sessions one talk per session at a time. This means the longest 
talks are at the beginning of each session, and the shorter ones are at the end. Since at the end of each session when we're (potentially) running out of space, it's more
likely that we could fit in a smaller talk than a longer one.

In other words, we select the longest talk that will fit in the available space in the session.

There are surely better algorithms out there, and to override the default algorithm, a new ITalkDistributor can be implemented and injected.

## Assumptions
1. If the application's algorithm can't fit all the talks in, it display's an error message and outputs the best effort
2. If the file can't be read, output an error and display an empty conference.
3. Output is written to Console, but is abstracted away so replace with any type of IOutputWriter you like
4. Rules state no external dependencies, but I assume an IOC container is permissible along with my testing (Xunit) and mocking (Moq) frameworks.

## Actual output of application, based on given input
Track 1:  
09:00AM Writing fast Tests Against Enterprise Rails 60min  
10:00AM Ruby on Rails Legacy App Maintenance 60min  
11:00AM Accounting-Driven Development 45min  
12:00PM Lunch  
01:00PM Communicating Over Distance 60min  
02:00PM Overdoing it in Python 45min  
02:45PM Pair Programming vs Noise 45min  
03:30PM Woah 30min  
04:00PM Ruby vs. Clojure for Back-End Development 30min  
04:30PM User Interface CSS in Rails Apps 30min  
05:00PM Networking Event  

Track 2:  
09:00AM Rails Magic 60min  
10:00AM Ruby Errors from Mismatched Gem Versions 45min  
10:45AM Clojure Ate Scala (on my project) 45min  
11:30AM Sit Down and Write 30min  
12:00PM Lunch  
01:00PM Ruby on Rails: Why We Should Move On 60min  
02:00PM Common Ruby Errors 45min  
02:45PM Lua for the Masses 30min  
03:15PM Programming in the Boondocks of Seattle 30min  
03:45PM A World Without HackerNews 30min  
04:15PM Rails for Python Developers lightning  
05:00PM Networking Event  

# Requirements

## Thoughtworks Coding Assignment 2

> You are planning a big programming conference and have received many proposals which have passed the initial screen process but you're having trouble fitting them into the time constraints of the day -- there are so many possibilities! So you write a program to do it for you.
> 
> The conference has multiple tracks each of which has a morning and afternoon session.
> 
> Each session contains multiple talks.
> 
> Morning sessions begin at 9am and must finish by 12 noon, for lunch.
> 
> Afternoon sessions begin at 1pm and must finish in time for the networking event.
> 
> The networking event can start no earlier than 4:00 and no later than 5:00.
> 
> No talk title has numbers in it.
> 
> All talk lengths are either in minutes (not hours) or lightning (5 minutes).
> 
> Presenters will be very punctual; there needs to be no gap between sessions.
>  
> Note that depending on how you choose to complete this problem, your solution may give a different ordering or combination of talks into tracks. This is acceptable; you don't need to exactly duplicate the sample output given here.

## Test input:

Writing Fast Tests Against Enterprise Rails 60min  
Overdoing it in Python 45min  
Lua for the Masses 30min  
Ruby Errors from Mismatched Gem Versions 45min  
Common Ruby Errors 45min  
Rails for Python Developers lightning  
Communicating Over Distance 60min  
Accounting-Driven Development 45min  
Woah 30min  
Sit Down and Write 30min  
Pair Programming vs Noise 45min  
Rails Magic 60min  
Ruby on Rails: Why We Should Move On 60min  
Clojure Ate Scala (on my project) 45min  
Programming in the Boondocks of Seattle 30min  
Ruby vs. Clojure for Back-End Development 30min  
Ruby on Rails Legacy App Maintenance 60min  
A World Without HackerNews 30min  
User Interface CSS in Rails Apps 30min  
 
## Test output: 
Track 1:  
09:00AM Writing Fast Tests Against Enterprise Rails 60min  
10:00AM Overdoing it in Python 45min  
10:45AM Lua for the Masses 30min  
11:15AM Ruby Errors from Mismatched Gem Versions 45min  
12:00PM Lunch  
01:00PM Ruby on Rails: Why We Should Move On 60min  
02:00PM Common Ruby Errors 45min  
02:45PM Pair Programming vs Noise 45min  
03:30PM Programming in the Boondocks of Seattle 30min  
04:00PM Ruby vs. Clojure for Back-End Development 30min  
04:30PM User Interface CSS in Rails Apps 30min  
05:00PM Networking Event  
 
Track 2:  
09:00AM Communicating Over Distance 60min  
10:00AM Rails Magic 60min  
11:00AM Woah 30min  
11:30AM Sit Down and Write 30min  
12:00PM Lunch  
01:00PM Accounting-Driven Development 45min  
01:45PM Clojure Ate Scala (on my project) 45min  
02:30PM A World Without HackerNews 30min  
03:00PM Ruby on Rails Legacy App Maintenance 60min  
04:00PM Rails for Python Developers lightning  
05:00PM Networking Event  

