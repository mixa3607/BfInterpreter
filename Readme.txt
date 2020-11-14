Project include test data. 
BfExamples => brainfick files (helloWorld_1 work only with 8 bits word and memPtr)
Images => archives with files, extract and pass dir path as arg

Compute factorial dotnet 
	Bf.dll execute -f fractal.bfk
Render BadApple in 20 fps 256x192 (w=BitsInWord*WordsInRow h=Rows)
	dotnet BF.dll draw -b 64 -w 4 -r 196 -d Images/20fps/ -f 20
	
Usage - BF <action> -options

GlobalOption   Description
Help (-?)      Shows this help

Actions
  DrawImages (draw) -options -
    Option                                 Description
    BitsInWord* (-b)
    WordsInRow* (-w)
    Rows* (-r)
    ImagesDir* (-d)
    ImagesLoadParallelism (-loadThreads)   [Default='8']
    FramesPerSec (-f, -fps)                [Default='10']
  ExecuteBf (execute) -options -
    Option                     Description
    BitsInWord (-b)            [Default='8']
    Words (-w)                 [Default='30000']
    BitsInMemPtr (-mem-bits)   [Default='8']
    BfFile* (-f)
