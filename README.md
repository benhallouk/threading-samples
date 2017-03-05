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

### Thread life time example

Thread has by default long life time which means can run even when the main thread is terminated as shown in the example above. But when the property IsBackground set to true `thread.IsBackground = true;` thread can be terminated as it is considered not important so the sample example would print this output

```
Main thread started
Thread number started 3
Thread number started 4
Thread number started 5
Thread number started 6
Thread number started 7
Thread number started 8
Thread number started 9
Thread number started 10
Main thread ends
```

**Note** that because os Sleep that simulate computing work the threads never get a chance to complete the computation in this example, if you would run it couple of times you may see some thread may get change to complete before the main thread get terminated

### Thread shutdown cordination example

Sometimes is needed that you will want to cordinate the thread shutdown which mean controlling when a thread needs to be shutdown, in this example you will learn how to do that

In order to do that you will need a simple flag that is safe to share between threads we use boolean with the c# keyword `volatile` 

```csharp
public volatile static bool Cancel = false;
```

The compute action would use then this flag in a while loop as shown below

```csharp
public void Compute()
{
    while (!ThreadShutdownCordinationExampleRunner.Cancel)
    {
        Console.WriteLine("Computting...");
        Thread.Sleep(2000);                
    }
}
```

When the thread is started the volatile flag cancel is set to false, and when its set to cancel we join the thread and wait for it to be shutdown

```csharp
Thread thread = new Thread(threadExample.Compute);
thread.Start();

//simulating some work on the main thread
Thread.Sleep(1000);

//set the volatile flag cancel to true, which will exit the computing loop
Cancel = true;

// join will block the current thread and wait until the thread is shutdown
thread.Join();
```

### Lock example

In multi-threading enviroment it is possible to have 2 threads accessing the same resource at the same time, and this is the bug factory in multithreading and it is one of the caviates we highlite above, consider this example:

Consider the following class 

```csharp
public class ThreadExample
{
    private int _number = 1;

    public void Compute()
    {
        Thread.Sleep(10);
        _number++;             
    }

    public void ShowNumber()
    {
        Console.WriteLine("The final value of number is {0}",_number);
    }
}
```

The code bellow run 10 threads that does a comuptation by calling compute method which increment the number by one on each thread

```csharp
var threadExample = new ThreadExample();
for (int i = 1; i < 11; i++)
{
    Thread thread = new Thread(threadExample.Compute);
    thread.Start();
}
threadExample.ShowNumber();
```

So we would expect the instructions above to print 10 however in this case show number will print unpredictable value and here is the reason way:

Each thread before incrementing the number it will read the current value which may have been changed before its inremented so running this code will produce diffrent results each time.

To fix that we would use `lock` keyword over an object which then ensure that the number is locked for other threads and after it get incremented it will be released to be used by other threads, the code then would look like that

```csharp
public class ThreadExample
{
    //lock object
    private object _lockObject = new object();
    private int _number = 1;

    public void Compute()
    {
        Thread.Sleep(10);
        //use the lock keyword before incrementing the number
        lock(_lockObject)
        {
            _number++;
        }     
    }

    public void ShowNumber()
    {
        Console.WriteLine("The final value of number is {0}",_number);
    }
}
```

Here is a complete example of queing system that uses `lock` note that in the ComputeDequeing method we are using `Monitor.Wait(_objectTray);` which keep waiting for a change on the _objectTray in sleep mode, while ComputeEnqueing uses `Monitor.PulseAll(_objectTray);` which wake all the sleeping threads who waits for _objectTray

```csharp
public class LockExample
{
    private Queue<string> _objectTray = new Queue<string>();

    public void ComputeDequeing()
    {
        Console.WriteLine("Thread number started {0}",Thread.CurrentThread.ManagedThreadId);

        lock (_objectTray)
        {
            while (_objectTray.Count == 0)
            {
                //wait for _objectTray and sleep
                Monitor.Wait(_objectTray);
            }

            var obj = _objectTray.Dequeue();
            Console.WriteLine("Processing {0} ...", obj);
        }
    }

    public void ComputeEnqueing()
    {
        Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);

        lock (_objectTray)
        {
            for (int i = 0; i < 5; i++)
            {
                _objectTray.Enqueue(string.Format("Item {0} from thread {1}",i, Thread.CurrentThread.ManagedThreadId));
            }
            // wake all the the threads that wait for _objectTray
            Monitor.PulseAll(_objectTray);
        }
    }

}
```

The main thread would look something like that

```csharp
var lockExample = new LockExample();

// Enqueing
for (int i = 0; i < 10; i++)
{            
    Thread thread = new Thread(lockExample.ComputeEnqueing);
    thread.Start();
}

//Dequeing
for (int i = 0; i < 10; i++)
{
    Thread thread = new Thread(lockExample.ComputeDequeing);
    thread.Start();
}
```

### Deadock example

When 2 threads lock each other will remain in locked state forever this is known as deadlock, the code bellow simulate a deadlock situation  

Cosider the class bellow, we have lockObject that is used to lock any thread who tries to change the number field, and since the compute method uses as parameter same type and operate on its number field it must have 2 locks one for the current instance and the other for the instance passes as parameter

```csharp
public class DeadLockExample
{
    private int _number;
    public object _lockObject = new object();

    public DeadLockExample(int number)
    {
        _number = number;
    }


    public int GetNumber()
    {
        return _number;
    }

    public void Compute(DeadLockExample deadLockExample)
    {
        lock (_lockObject)
        {
            Thread.Sleep(1);
            lock (deadLockExample._lockObject)
            {
                Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);
                _number = deadLockExample.GetNumber();
            }
        }
    }

}
```

This situation produces a deadlock when running 2 paralelel threads as bellow

```csharp
var lockExample1 = new DeadLockExample(1);
var lockExample2 = new DeadLockExample(2);

for (int i = 0; i < 100; i++)
{            
    Thread thread = new Thread(()=> {
        if (i % 2 == 0)
        {
            lockExample1.Compute(lockExample2);
        }
        else
        {
            lockExample2.Compute(lockExample1);
        }     
    });
    thread.Start();
}

Console.WriteLine("lockExample1 {0}", lockExample1.GetNumber());
Console.WriteLine("lockExample2 {0}", lockExample2.GetNumber()););
```

by running the code couple of times you may see 1 or 2 threads survivde the deadlock

### Deadlock solution

One of the classcial solution is to use a stable order of which the locks take place, for example always the thread dealing with object that has smaller id would run first that way the threads are not colliding with each other, and an implementation of that may look like below

```csharp
public void Compute(DeadLockSolution deadLockSolution)
{
    var firstLock = _lockObject;
    var secondLock = deadLockSolution._lockObject;

    if (Id > deadLockSolution.Id)
    {
        firstLock = deadLockSolution._lockObject;
        secondLock = _lockObject;
    }
        
    lock (firstLock)
    {
        Thread.Sleep(1);
        lock (secondLock)
        {
            Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);
            _number = deadLockSolution.GetNumber();
        }
    }
}
```

### Deadlock solution using mutex

You can use the windows kernel wrapper arround mutex to prevent dead locking by letting the windows handel the lock for you by using `mutex.WaitOne()` and releasing the mutex using `mutex.ReleaseMutex()`, the code implementation would then look like below

```csharp
public class DeadLockSolutionUsingMutex
{
    private int _number;
    Mutex mutex = new Mutex();

    public DeadLockSolutionUsingMutex(int number)
    {
        _number = number;
    }


    public int GetNumber()
    {
        return _number;
    }

    public void Compute(DeadLockSolutionUsingMutex deadLockSolution)
    {
        if (mutex.WaitOne())
        {
            try
            {
                Thread.Sleep(1);
                Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);
                _number = deadLockSolution.GetNumber();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }            
    }

}
```

**Note** that when using Mutex object it is important to use try finally to release the Mutex object, `WaitOne()` method and `ReleaseMutex()` accept also a timeout as an argument

One of the advantages of using mutex is the multiple locks feature that it provides, to do that you can use something like this

```csharp
// create an array using the mutex objects subject to lock
Mutex[] locks = {mutex1, mutex2}

// using WaitHandel.WaitAll to use all the locks provided as argument
if(WaitHandel.WaitAll(locks))
{
    try
    {
        //do something
    }
    finally
    {
        //no implementation provide such as ReleaseAll
        foreach(var mutex in locks)
        {
            mutex.ReleaseMutex();
        }
    }    
}
```