# Threading Samples

This is a simple guide for developers to get started with multi threading in .net enviroment

This guide should make you familiar with the fundamentals of multi-threading and explains how the CLR implement the multi threading in .net

## Geting started

The code samples is structored in a way to assit you learning, each section in this guide has a name space in the code so that you can jump easily between patterns and implementation of multi threading

### Process vs Thread 

First of all we need to understand this vocabulary, the word process on Windows based platforms refers to an inert container which means an address in memory where many exusion paths happen while the word Thread is like path of execution within a single process.

### Threading use case

The multithreading has sevral benefits here are some:

- Can be used as away to scale by parallelizing CPU-bound operations for multi-core hardware
- I/O operations can be done in the background while CPU compute the IO data
- Maintain application UI responsivness

### Threading caveats

Altough multi-threading can be very benefital there are some caveats assoiciated with it:

- Introduce code complexicty, code can easily become not readable 
- Not easy to debug as excusion happens in parallel
- Bugs can be hidden under context switching which means that paralel exusion paths has diffrent context


### Thread example

Class that hold data subject computing 

```csharp
using System;
using System.Threading;

public class ThreadExample
{
    private readonly int _number;
    private readonly string _title;
    private readonly string _description;

    public ThreadExample(int number, string title, string description)
    {
        _number = number;
        _title = title;
        _description = description;
    }

    // the method where the thread compute the data
    public void Compute()
    {
        Console.WriteLine("Thread number started {0}",Thread.CurrentThread.ManagedThreadId);
        
        //Sleep semulate computation of data
        Thread.Sleep(10);
        
        Console.WriteLine("Access to instance private data:");
        Console.WriteLine("Number {0}",_number);
        Console.WriteLine("Title {0}",_title);
        Console.WriteLine("Description {0}",_description);
    }
}
```

In the example bellow we start 10 threads, each thread refer to an instance method of the object subject to computing 

```csharp
Console.WriteLine("Main thread started");
for (int i = 1; i < 11; i++)
{
    var threadExample = new ThreadExample(i, string.Format("title {0}",i), string.Format("description",i));

    Thread thread = new Thread(threadExample.Compute);
    thread.Start();
}
Console.WriteLine("Main thread ends");
```

The excusion should print something like that:

```
Main started
Thread number started 3
Thread number started 4
Thread number started 5
Thread number started 6
Thread number started 7
Thread number started 8
Thread number started 9
Thread number started 10
Access to instance private data:
Number 7
Title title 7
Description description
Access to instance private data:
Number 4
Title title 4
Description description
Access to instance private data:
Number 8
Title title 8
Description description
Thread number started 11
Access to instance private data:
Number 3
Title title 3
Description description
Access to instance private data:
Number 2
Title title 2
Description description
Main thread ends
Access to instance private data:
Number 5
Title title 5
Description description
Thread number started 12
Access to instance private data:
Number 6
Title title 6
Description description
Access to instance private data:
Number 1
Title title 1
Description description
Access to instance private data:
Number 9
Title title 9
Description description
Access to instance private data:
Number 10
Title title 10
Description description
```

**Note** here couple of things:

- The order is unpredictable since the CLR thread scheduler decide when to each thread and when to pause it
- The main thread ends before the other threads get acomplished