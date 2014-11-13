A very efficient top-k sequential pattern miner.

# Supported Platforms
Developed using C#, and tested under Windows, OS X (w/ Mono), and Linux (w/ Mono). 

# Usage
Run without any argument to show help. 

# Features
- Mining top-k sequence patterns (w/ or w/o gap) from text or any sequential data. 
- Supports stop-words. 
- Supports minimum and maximum pattern length constraints.
    - With corresponding pattern search space pruning. 
- Based on state-of-the-art [PrefixSpan](http://www.cs.sfu.ca/~jpei/publications/span.pdf) algorithm. 
    - Integrated with top-k mining optimization.
