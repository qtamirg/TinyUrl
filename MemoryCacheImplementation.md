# Memory Cache Implementation

The requirements for a Custom Memory Cache implementation:
- The cache should be size-limited to protect memory from overflow.
- Don't Use any external libraries. (Including **Microsoft.Extensions.Caching.Memory.MemoryCache**)
- Consider Concurrency and multi-threading.
- Consider Request Collapsing to minimize database access as much as possible.

In my approach, I created a cache with a specified maximum size (should be DB configurable / AppSettings setting). My main deliberation was in going with a **LRU** (Least Recently Used) or **LFU** (Least Frequently Used) approach.

I decided on the LRU approach. I created a component with `Dictionary` to cache the short to long conversion, and a `Queue` to maintain the order in which entries were cached.

This approach provides a memory efficient way to store frequently used urls with a maximum size limit, minimizes database access. 

The only disadvantage is that in peak usage times, the approach has overhead to evict the least recently used entry. 

Also, due to the implementation enabling multiple requests to enter the common resources (by using semaphore), there are cases that order of least recently used becomes unordered, but in my thinking, the implementation works under the assumption that urls will be most used only in the starting hours of creation of the short url. Therefore, there shouldn't be a lot of usage when a url was cached for a long time already.