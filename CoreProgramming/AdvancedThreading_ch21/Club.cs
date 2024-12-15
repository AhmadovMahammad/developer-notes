
namespace AdvancedThreading_ch21;

public class Club
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(5); // maximum capacity of 5

    public Club()
    {
        // Simulate 5 people trying to enter the club
        for (int i = 1; i <= 10; i++)
        {
            int personId = i; // Capture the loop variable
            new Thread(() => Enter(personId)).Start();
        }
    }

    private void Enter(int id)
    {
        Console.WriteLine($"Person {id} wants to enter.");

        // Wait for permission to enter (decrement the semaphore count)
        _semaphore.Wait();

        Console.WriteLine($"Person {id} is in!");
        Thread.Sleep(1000 * id); // Simulate time spent in the club

        Console.WriteLine($"Person {id} is leaving.");

        // Release the semaphore (increment the count)
        _semaphore.Release();
    }
}